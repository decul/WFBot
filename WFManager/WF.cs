using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var subject = Browser.GetElementsByClass("messages_list_unread").LastOrDefault();
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
                    var subject = Browser.GetElementsByClass("messages_list_unread").LastOrDefault();
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
    }
}
