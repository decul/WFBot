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

        private static DateTime lastContractDate = DateTime.MinValue;


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

                    Logger.SlackAlert("System :: " + subject.InnerText + " :: " + message, "system");
                }

                // Close Mail box
                Browser.InvokeScript("messagesClose");
                Browser.Wait(1000);
            }

            exclamation = Browser.GetElementById("mainmenue3_incoming");
            if (exclamation != null && Browser.isVisible(exclamation)) {
                // Open Contracts
                exclamation.Parent.InvokeMember("click");
                Browser.WaitForId("contracts_navi_overview_in");

                var dates = Browser.GetElementsByClass("contracts_list_time", "div");
                if (dates.Any()) {
                    DateTime date = DateTime.ParseExact(dates.First().InnerText, "dd.MM.yy, HH:mm", CultureInfo.InvariantCulture);
                    if (date > lastContractDate) {
                        Logger.SlackAlert("You have recived new contract", "contracts");
                        lastContractDate = date;
                    }
                }

                // Close Contracts
                Browser.InvokeScript("contractsClose");
                Browser.Wait(1000);
            }
        }



        static public void UpdateProductsInfo() {
            // Go to Help
            Browser.Click("mainmenue5");
            Browser.WaitForId("newhelp_menue_item_products_v");
            
            // Update products info
            updateProductInfo(Store.Vegetables, "newhelp_menue_item_products_v", "kp17");
            updateProductInfo(Store.Diaries, "newhelp_menue_item_products_e", "kp9");
            updateProductInfo(Store.Exotics, "newhelp_menue_item_products_ex", "kp351");
            updateProductInfo(Store.Oils, "newhelp_menue_item_products_o", "kp124");
            updateProductInfo(Store.Juices, "newhelp_menue_item_foodworldbld1", "kp130");
            updateProductInfo(Store.Snacks, "newhelp_menue_item_foodworldbld2", "kp136");
            updateProductInfo(Store.Cakes, "newhelp_menue_item_foodworldbld3", "kp161");
            updateProductInfo(Store.IceCreams, "newhelp_menue_item_foodworldbld4", "kp450");

            // Complete Picnics
            Browser.Click("newhelp_menue_item_products_fw");
            Browser.WaitForClass("kp130", "div");

            foreach (var row in HelpTableRow.getRows()) {
                Picnic pinfo;
                if (Store.Juices.ContainsKey(row.ID))
                    pinfo = Store.Juices[row.ID];
                else if (Store.Snacks.ContainsKey(row.ID))
                    pinfo = Store.Snacks[row.ID];
                else if (Store.Cakes.ContainsKey(row.ID))
                    pinfo = Store.Cakes[row.ID];
                else if (Store.IceCreams.ContainsKey(row.ID))
                    pinfo = Store.IceCreams[row.ID];
                else {
                    Logger.Error("Cannot complete Picnic's info cause it cannot be found");
                    continue;
                }
                   
                pinfo.BonusPointsPerSquare = row.BonusPoints;
                pinfo.BasePrice = row.Price;
            }

            Store.Save(WF.storagePath);

            // Close Help
            Browser.GetChildrenByClass(Browser.GetElementById("newhelp"), "mini_close")[0].InvokeMember("click");
        }

        private static void updateProductInfo <T> (SDictionary<int, T> dict, string categoryButtonId, string expectedClass) where T : Product {
            Browser.Click(categoryButtonId);
            Browser.WaitForClass(expectedClass, "div");

            foreach (var row in HelpTableRow.getRows()) {
                var pinfo = Activator.CreateInstance<T>();
                pinfo.ID = row.ID;
                if (dict.ContainsKey(pinfo.ID))
                    pinfo = dict[pinfo.ID];
                else
                    dict.Add(pinfo.ID, pinfo);

                pinfo.Name = row.Name;
                pinfo.HarvestFromIndividual = row.HarvestPerUnit;
                pinfo.GrowthTime = row.GrowthTime;

                if (typeof(T) == typeof(Picnic)) {
                    ((Picnic)(object)pinfo).Ingredients = row.Ingredients;
                } 
                else {
                    pinfo.BonusPointsPerSquare = row.BonusPoints;
                    pinfo.BasePrice = row.Price;
                }

                if (typeof(T) == typeof(Vegetable))
                    ((Vegetable)(object)pinfo).Size = row.Size;
            }
        }
        
        //static private void updateVegetablesInfo() {
        //    // Go to Vegetables Products (must be in help first)
        //    Browser.Click("newhelp_menue_item_products_v");
        //    Browser.WaitForClass("kp17", "div");

        //    var content = Browser.GetElementById("newhelp_content");
        //    var rows = Browser.getOffspringByClass(content, "newhelp_line", "td");
        //    foreach (HtmlElement row in rows) {
        //        try {
        //            Vegetable pinfo = new Vegetable();
        //            pinfo.ID = int.Parse(row.Children[0].Children[0].GetAttribute("className")
        //                          .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);

        //            if (Store.Vegetables.ContainsKey(pinfo.ID))
        //                pinfo = Store.Vegetables[pinfo.ID];
        //            else
        //                Store.Vegetables.Add(pinfo.ID, pinfo);

        //            pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

        //            var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
        //            pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

        //            pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);

        //            var sizeStr = row.Children[3].InnerText.Trim();
        //            pinfo.Size = (sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2));

        //            var points = row.Children[4].InnerText.Replace(".", "").Trim();
        //            int.TryParse(points, out pinfo.BonusPointsPerSquare);

        //            var price = Regex.Replace(row.Children[5].InnerText, "[^0-9,]", "").Replace(',', '.');
        //            double.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out pinfo.BasePrice);

        //        } catch (Exception exc) {
        //            Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
        //        }
        //    }
        //}

        //static private void updateProductsInfo<T>() where T : Product {
        //    foreach (var row in HelpTableRow.getRows()) {
        //        try {
        //            // Read ID of product
        //            T pinfo = Activator.CreateInstance<T>();
        //            pinfo.ID = row.ID;

        //            // Find product on list, or add new one
        //            try {
        //                pinfo = (T)Store.Products[pinfo.ID];
        //            } catch (KeyNotFoundException) {
        //                if (typeof(T).Equals(typeof(Vegetable)))
        //                    Store.Vegetables.Add(pinfo.ID, (Vegetable)(object)pinfo);
        //                else if (typeof(T).Equals(typeof(Diary)))
        //                    Store.Diaries.Add(pinfo.ID, (Diary)(object)pinfo);
        //                else
        //                    throw new Exception("Product info not found");
        //            }

        //            // Default column indexes
        //            int bCol = 3;
        //            int pCol = 4;

        //            // Vegetable fields
        //            if (typeof(T).Equals(typeof(Vegetable))) {
        //                bCol = 4;
        //                pCol = 5;

        //                var sizeStr = row.Children[3].InnerText.Trim();
        //                ((Vegetable)(object)pinfo).Size = (sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2));
        //            }

        //            // Common fields
        //            pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

        //            var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
        //            pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

        //            pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);

        //            var bonus = row.Children[bCol].InnerText.Replace(".", "").Trim();
        //            int.TryParse(bonus, out pinfo.BonusPointsPerSquare);

        //            var price = Regex.Replace(row.Children[pCol].InnerText, "[^0-9,]", "").Replace(',', '.');
        //            double.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out pinfo.BasePrice);

        //        } catch (Exception exc) {
        //            Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
        //        }
        //    }
        //}

        

        
    }
}
