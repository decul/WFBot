using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFManager.ProductNS;

namespace WFManager.Places {
    public static class Farm {

        private static void travelToFarm() {
            // Go to Map
            Browser.Click("mainmenue1");
            Browser.WaitForId("map_farm1");

            // Go to Farm
            Browser.Click("map_farm1");
            Browser.WaitForBus();
            Browser.WaitForId("farm1_pos1_click");
        }



        public static TimeSpan SowFields(int productId) {
            travelToFarm();
            
            int fieldsCount = Browser.GetElementsByClass("bm1").Count;
            for (int f = 0; f < fieldsCount; f++) {
                // Go to Field
                var field = Browser.GetElementsByClass("bm1")[f];
                Browser.GetSiblingsByClass(field, "farm_pos_click")[0].InvokeMember("click");
                Browser.WaitForId("cropall");

                // Harvest ready products
                if (Browser.GetElementById("cropall").GetAttribute("className").Split(' ').Contains("cropall")) {
                    Browser.Click("cropall");
                    Browser.WaitForId("globalbox_button1", 3000);

                    string report = string.Join(", ", Browser.GetElementsByClass("cropall_dialog_product").Select(p => p.InnerText).ToList());
                    Logger.Info("Zebrano: " + report);

                    Browser.Click("globalbox_button1");
                    Browser.Wait(500);
                } else {
                    Browser.Click("ernten");
                    Browser.Wait(1000);

                    foreach (HtmlElement outerSquare in Browser.GetElementById("gardenarea").Children) {
                        HtmlElement square = outerSquare.Children[0];
                        if (canBeHarvested(square)) {
                            square.InvokeMember("click");
                            Browser.Wait(250);
                        }
                    }
                }

                // Sow
                Browser.Click("rackitem" + productId);
                Browser.Wait(1000);

                foreach (HtmlElement outerSquare in Browser.GetElementById("gardenarea").Children) {
                    HtmlElement square = outerSquare.Children[0];
                    if (canBeSown(square)) {
                        square.InvokeMember("click");
                        Browser.WaitFor(() => !canBeSown(square), 0);
                    }
                }
                waitForAllSquaresToLoad();

                // Close field
                Browser.Click("gardencancel");
                Browser.Wait(1000);
            }
            
            //for (int f = 0; f < fieldsCount; f++) {
            //    // Go to Field
            //    var field = Browser.GetElementsByClass("bm1")[f];
            //    Browser.GetSiblingsByClass(field, "farm_pos_click")[0].InvokeMember("click");
            //    Browser.WaitForId("cropall");

            //    // Water field
            //    if (Browser.GetElementById("waterall").GetAttribute("className").Split(' ').Contains("waterall")) {
            //        Browser.Click("waterall");
            //        Browser.Wait(1000);
            //    }
            //    else {
            //        Browser.Click("giessen");
            //        Browser.Wait(1000);

            //        foreach (HtmlElement outerSquare in Browser.GetElementById("gardenarea").Children) {
            //            HtmlElement square = outerSquare.Children[0];
            //            if (isHarvestable(square)) {
            //                square.InvokeMember("click");
            //                Browser.Wait(300);
            //            }
            //        }
            //        waitForWatering();
            //    }

            //    Browser.Document.InvokeScript("parent.show_built", new object[] { 120, "out" });
            //    Browser.WaitForId("gtt_zeit", 1000);
            //    time = Utility.ParseTimeSpan(Browser.GetElementById("gtt_zeit").InnerText);
            //    Logger.Label(Browser.GetElementById("gtt_zeit").InnerText);

            //    // Close field
            //    Browser.Click("gardencancel");
            //    Browser.Wait(1000);
            //}

            return new TimeSpan();
        }

        private static bool canBeSown(HtmlElement square) {
            assertInnerSquare(square);
            return square.Style.Contains("http://mff.wavecdn.de/mff/produkte/0.gif");
        }


        private static bool canBeHarvested(HtmlElement square) {
            assertInnerSquare(square);
            if (isHarvestable(square)) 
                return Regex.IsMatch(square.Style, "http://mff.wavecdn.de/mff/produkte/.*_04.gif");
            return false;
        }

        private static bool isHarvestable(HtmlElement square) {
            assertInnerSquare(square);

            if (canBeSown(square))
                return false;

            if (square.Style.Contains("http://mff.wavecdn.de/mff/produkte/steine_04.gif"))
                return false;
            if (square.Style.Contains("http://mff.wavecdn.de/mff/produkte/unkraut_04.gif"))
                return false;
            if (square.Style.Contains("http://mff.wavecdn.de/mff/produkte/baumstumpf_04.gif"))
                return false;
            if (square.Style.Contains("http://mff.wavecdn.de/mff/produkte/maulwurf_04.gif"))
                return false;

            return true;
        }

        //private static bool isLoading(HtmlElement square) {
        //    assertInnerSquare(square);
        //    return square.InnerHtml.Contains("http://mff.wavecdn.de/mff/loading.gif");
        //}

        private static bool isWatered(HtmlElement square) {
            assertInnerSquare(square);
            return square.InnerHtml.Contains("http://mff.wavecdn.de/mff/garten/gegossen.gif");
        }

        private static void waitForAllSquaresToLoad(int waitTime = 1000) {
            if (!Browser.WaitFor(() => { return !Browser.GetElementById("gardenarea").Children.Cast<HtmlElement>().Select(os => os.Children[0]).Where(s => canBeSown(s)).Any(); }, waitTime))
                throw new Exception("Wait for all squares to load Timeout");
        }

        private static void waitForWatering(int waitTime = 1000) {
            Func<bool> predicate = () => {
                return !Browser.GetElementById("gardenarea").Children.Cast<HtmlElement>().Select(os => os.Children[0]).Where(s => isHarvestable(s) && !isWatered(s)).Any();
            };
            if (!Browser.WaitFor(predicate, waitTime))
                throw new Exception("Wait for watering Timeout");
        }

        private static HtmlElement assertInnerSquare(HtmlElement square) {
            if (!Regex.IsMatch(square.GetAttribute("id"), "^f[0-9]*$"))
                throw new Exception("Method expected inner square, something else found");
            return square;
        }



        public static TimeSpan FeedChickens() {
            TimeSpan productionTime = TimeSpan.FromHours(1);

            travelToFarm();

            // Calculate feeding strategy
            var egg = Store.Get(9);
            var grain = Store.Vegetables[1];
            var corn = Store.Vegetables[2];

            var cheaperFood = (2 * grain.LowestPriceNow < corn.LowestPriceNow) ? grain : corn;
            bool feedFull = true;

            if (egg.LastNotNullMarketPrice * 0.9 < cheaperFood.LowestPriceNow * (cheaperFood.ID == 1 ? 12 : 6)) { 
                cheaperFood = (grain.LowestPriceNow < corn.LowestPriceNow ? grain : corn);
                feedFull = false;
            }
            
            int henhousesCount = Browser.GetElementsByClass("bm2").Count;
            for (int h = 0; h < henhousesCount; h++) {
                // Go to Henhouse
                var henhouse = Browser.GetElementsByClass("bm2")[h];
                Browser.GetSiblingsByClass(henhouse, "farm_pos_click")[0].InvokeMember("click");
                Browser.WaitForClass("animal2");

                // Collect Eggs
                Browser.TryClick("globalbox_button1");
                Browser.Wait(2000);

                var feedline = Browser.GetElementById("building_inner_feedline_normal");
                if (feedline.Children.Count > 0 && !Browser.GetChildrenByClass(feedline, "transparent").Any()) {
                    // Feed Chickens
                    giveFood(cheaperFood.ID, feedFull);
                    Browser.WaitForId("production_info_time", 5000);
                }

                // Check production time
                var timeEl = Browser.GetElementById("production_info_time");
                if (timeEl != null) {
                    var timeStr = Regex.Replace(timeEl.InnerText, "[^0-9:]", "");
                    productionTime = TimeSpan.Parse(timeStr).Add(TimeSpan.FromMinutes(1));
                }

                // Close Henhouse
                Browser.TryClick("building_inner", "big_close");
                Browser.Wait(1000);
            }

            return productionTime;
        }

        private static void giveFood(int productId, bool full) {
            var sack = Browser.GetElementById("feed_item" + productId + "_normal");
            for (int i = 0; i < 36; i++)
                sack.Children[0].InvokeMember("click");
            //Browser.WaitForId("building_dialogbox_input");
                
            //if (full)
            //    Browser.SetValue("building_dialogbox_input", "9999");

            //Browser.Click("building_dialogbox_submit");
        }
    }
}
