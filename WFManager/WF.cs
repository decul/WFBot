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
    }
}
