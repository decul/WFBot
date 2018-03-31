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

                var dates = Browser.GetElementsByClass("contracts_list_time", null);
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
            updateWoods();
            updateProductInfo(Store.Timbers, "newhelp_menue_item_forestrybld1", "f_m_symbol41");
            updateProductInfo(Store.WoodenProducts, "newhelp_menue_item_forestrybld2", "f_m_symbol101");

            // Complete Picnics
            Browser.Click("newhelp_menue_item_products_fw");
            Browser.WaitForClass("kp130", null);

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

        private static void updateProductInfo <T> (SDictionary<int, T> dict, string categoryButtonId, string expectedClass, int firstRow = 0, int rowsNo = 9999) where T : Product {
            Browser.Click(categoryButtonId);
            Browser.WaitForClass(expectedClass, null);

            var rows = HelpTableRow.getRows();

            int stopIndex = Math.Min(rows.Count, rowsNo + firstRow);

            for (int r = firstRow; r < stopIndex; r++) {
                var row = rows[r];
                var pinfo = Activator.CreateInstance<T>();
                pinfo.ID = row.ID;
                if (dict.ContainsKey(pinfo.ID))
                    pinfo = dict[pinfo.ID];
                else
                    dict.Add(pinfo.ID, pinfo);

                pinfo.Name = row.Name;
                pinfo.HarvestFromIndividual = row.HarvestPerIndividual;
                pinfo.GrowthTime = row.GrowthTime;
                pinfo.Ingredients = row.Ingredients;
                pinfo.BonusPointsPerSquare = row.BonusPoints;
                pinfo.BasePrice = row.Price;

                if (typeof(T) == typeof(Vegetable))
                    ((Vegetable)(object)pinfo).Size = row.Size;
            }
        }
        
        private static void updateWoods() {
            updateProductInfo(Store.Seedlings, "newhelp_menue_item_products_f", "f_m_symbol1", 0, 8);
            updateProductInfo(Store.Woods, "newhelp_menue_item_products_f", "f_m_symbol1", 8, 8);

            foreach(var wood in Store.Woods.Values) {
                var subName = wood.Name.Substring(6, 3).ToLower();
                var seedling = Store.Seedlings.Values.Where(s => s.Name.ToLower().Contains(subName)).First();

                wood.GrowthTime = seedling.GrowthTime;
                wood.BonusPointsPerSquare = seedling.BonusPointsPerSquare;
                wood.Ingredients.Add(new Ingredient(seedling.ID, 1));
            }
        }

        public static void hideAds() {
            Browser.SetPermanentStyle("newsbox", "display: none");
            Browser.SetPermanentStyle("globaltransp", "display: none");
            Browser.SetPermanentStyle("bubble_adtext", "display: none");
            Browser.SetPermanentStyle("banner_right", "display: none");
            
            Browser.SetPermanentStyle("uptoolbar", "display: none");
            Browser.SetPermanentStyle("main_body", "margin: 0");
            Browser.GetElementById("main_body").Children[3].Id = "wyjebana_stopa";
            Browser.SetPermanentStyle("wyjebana_stopa", "display: none");
            Browser.SetPermanentStyle("game_control", "display: none");

        }
        
    }
}
