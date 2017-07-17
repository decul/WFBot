using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFStats.Places {
    class ElFishado {

        private static void travelToElFishado() {
            // Go to Map
            Browser.Click("mainmenue1");
            Browser.WaitForId("map_city2");

            // Go to Farm
            Browser.Click("map_city2");
            Browser.WaitForBus();
            Browser.WaitForId("cityzone_2_8");
        }


        public static void collectFreeProducts() {
            travelToElFishado();

            // Go to Lotery
            Browser.Click("cityzone_2_8");

            if (Browser.TryWaitForId("dailylot")) {
                // Get Ticket
                Browser.TryClick("dailylot");
                Browser.WaitForId("prizeslotgetprize");

                // Trade it for Products
                Browser.Click("prizeslotgetprize");
                Browser.WaitForClass("lotteryprize_info");

                // Log Info
                string report = string.Join(", ", Browser.GetElementsByClass("lotteryprize_info").Select(p => p.InnerText).ToList());
                Logger.Info("Wygrano na loterii: " + report);

                // Close dialog
                Browser.Click("globalbox_button1");
            }
        }
    }
}
