using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    public static class WF {
        
        public const string storagePath = ".\\data";

        private static string username;
        private static string password;
        private static int server;


        public static bool IsLogedIn() {
            try {
                return Browser.GetElementById("logoutbutton") != null;
            } catch (NullReferenceException e) {
                return false;
            }
        }

        public static void LogIn() {
            if (!IsLogedIn())
                LogIn(username, password, server);
        }

        public static void LogIn(string user, string pass, int serv) {
            username = user;
            password = pass;
            server = serv;

            if (IsLogedIn())
                return;

            Browser.Navigate("http://wolnifarmerzy.pl");
            Browser.GetElementById("loginusername").SetAttribute("Value", username);
            Browser.GetElementById("loginpassword").SetAttribute("Value", password);
            Browser.GetElementById("loginserver").SetAttribute("Value", server.ToString());
            Browser.GetElementById("loginbutton").InvokeMember("click");
            Browser.WaitForId("farm1_pos1_click");
        }

        public static void LogOut() {
            var button = Browser.GetElementById("logoutbutton");
            if (button != null)
                button.InvokeMember("click");
        }



        public static void CheckMail() {
            var exclamation = Browser.GetElementById("mainmenue2_incoming");
            if (exclamation != null && Browser.isVisible(exclamation)) {
                // Open Mail box
                exclamation.Parent.InvokeMember("click");
                Browser.WaitForId("messages_navi_in");

                while (true) {
                    // Click last unread message
                    var subject = Browser.GetElementsByClass("messages_list_unread").Where(e => e.GetAttribute("className").Split(' ').Contains("link")).LastOrDefault();
                    if (subject == null)
                        break;
                    subject.InvokeMember("click");
                    Browser.Wait(2000);

                    string message = Browser.GetSiblingsByClass(subject, "messages_list_body").First().Children[0].InnerText.Replace("\n", " ");
                    string sender = Browser.GetSiblingsByClass(subject, "messages_list_name").First().InnerText;

                    Logger.SlackAlert(sender + " :: " + subject.InnerText + " :: " + message, "mail");
                }

                // Go to System messages
                Browser.Click("messages_navi_system");
                Browser.WaitForClass("messages_list_inner_withoutinfo");

                while (true) {
                    // Click last unread message
                    var subject = Browser.GetElementsByClass("messages_list_unread").Where(e => e.GetAttribute("className").Split(' ').Contains("link")).LastOrDefault();
                    if (subject == null)
                        break;
                    subject.InvokeMember("click");
                    Browser.Wait(2000);

                    string message = Browser.GetSiblingsByClass(subject, "messages_list_body").First().Children[0].InnerText.Replace("\n", " ");

                    Logger.SlackAlert("System :: " + subject.InnerText + " :: " + message, "mail");
                }

                // Close Mail box
                Browser.Click("messages_main", "mini_close");
                Browser.Wait(1000);
            }
        }



        static public void UpdateProductsInfo() {
            // Go to Help
            Browser.Click("mainmenue5");
            Browser.WaitForId("newhelp_menue_item_products_v");

            updateVegetablesInfo();
            updateProductsInfo("newhelp_menue_item_products_e", "kp9");

            Store.XmlSerialize(WF.storagePath);

            // Close Help
            Browser.GetChildrenByClass(Browser.GetElementById("newhelp"), "mini_close")[0].InvokeMember("click");
        }
        
        static private void updateVegetablesInfo() {
            // Go to Vegetables Products (must be in help first)
            Browser.Click("newhelp_menue_item_products_v");
            Browser.WaitForClass("kp17", "div");

            var content = Browser.GetElementById("newhelp_content");
            var rows = Browser.getOffspringByClass(content, "newhelp_line", "td");
            foreach (HtmlElement row in rows) {
                try {
                    Vegetable pinfo = new Vegetable();
                    pinfo.ID = int.Parse(row.Children[0].Children[0].GetAttribute("className")
                                  .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);

                    if (Store.Vegetables.ContainsKey(pinfo.ID))
                        pinfo = Store.Vegetables[pinfo.ID];
                    else
                        Store.Vegetables.Add(pinfo.ID, pinfo);

                    pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

                    var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
                    pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

                    pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);

                    var sizeStr = row.Children[3].InnerText.Trim();
                    pinfo.Size = (sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2));

                    var points = row.Children[4].InnerText.Replace(".", "").Trim();
                    int.TryParse(points, out pinfo.BonusPointsPerSquare);

                    var price = Regex.Replace(row.Children[5].InnerText, "[^0-9,]", "").Replace(',', '.');
                    double.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out pinfo.BasePrice);

                } catch (Exception exc) {
                    Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
                }
            }
        }

        static private void updateProductsInfo(string buttonId, string expectedElementClass) {
            // Go to Products Category (must be in help first)
            Browser.Click(buttonId);
            Browser.WaitForClass(expectedElementClass, "div");

            var content = Browser.GetElementById("newhelp_content");
            var rows = Browser.getOffspringByClass(content, "newhelp_line", "td");
            foreach (HtmlElement row in rows) {
                try {
                    var pinfo = new Diary();
                    pinfo.ID = int.Parse(row.Children[0].Children[0].GetAttribute("className")
                                       .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);

                    if (Store.Diaries.ContainsKey(pinfo.ID))
                        pinfo = Store.Diaries[pinfo.ID];
                    else
                        Store.Diaries.Add(pinfo.ID, pinfo);

                    pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

                    var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
                    pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

                    pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);

                    var points = row.Children[3].InnerText.Replace(".", "").Trim();
                    int.TryParse(points, out pinfo.BonusPointsPerSquare);

                } catch (Exception exc) {
                    Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
                }
            }
        }
    }





}
