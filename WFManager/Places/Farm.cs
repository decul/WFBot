using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
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
            
            var vegetable = Store.Vegetables[productId];
            TimeSpan timeToHarvest = TimeSpan.Zero;

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
                    tryToClickAllSquares(1, canBeHarvested);
                }

                // Sow
                selectProductFromShelf(productId);
                tryToClickAllSquares(vegetable.Size, canBeSown);
                
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
            //    } else {
            //        Browser.Click("giessen");
            //        Browser.Wait(1000);

            //        var square = Browser.GetElementById("f1");
            //        Browser.MoveCursorToElement(square);
            //        Browser.WaitFor(() => square.InnerHtml.Contains("http://mff.wavecdn.de/mff/cursors/"));

            //        tryToClickAllSquares(vegetable.Size, canBeWatered);
            //    }

            //    Browser.Document.InvokeScript("parent.show_built", new object[] { 120, "out" });
            //    if (Browser.TryWaitForId("gtt_zeit", 1000)) {
            //        var time = Util.ParseTimeSpan(Browser.GetElementById("gtt_zeit").InnerText);
            //        if (timeToHarvest < time)
            //            timeToHarvest = time;
            //    }

            //    // Close field
            //    Browser.Click("gardencancel");
            //    Browser.Wait(1000);
            //}

            if (timeToHarvest > TimeSpan.Zero)
                return timeToHarvest;
            else {
                //Logger.Error("Nie można odczytać czasu pozostałego do zbiorów");
                return vegetable.GrowthTime;
            }
        }

        public static void selectProductFromShelf(int productId) {
            Browser.InvokeScript("selectRackItem", productId.ToString());
            Browser.WaitFor(() => Browser.GetClassName("lager_pic") == "l" + productId);
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
        
        private static bool canBeWatered(HtmlElement square) {
            assertInnerSquare(square);

            if (!isHarvestable(square))
                return false;
            else
                return !square.InnerHtml.Contains("http://mff.wavecdn.de/mff/garten/gegossen");
        }

        /// <summary>Try to click all squares on field satisfying squareFilter, 3 times, each time waiting till no square satisfy squareFilter anymore</summary>
        /// <param name="vegSize">Vegetable size</param>
        /// <param name="squareFilter">Determines both witch squares to click, and if click was successful (returned value has to change to false after successful clik</param>
        /// <returns>Value indicating if all fields was clicked successfully</returns>
        private static bool tryToClickAllSquares(int vegSize, Func<HtmlElement, bool> squareFilter) {
            for (int attempt = 0; attempt < 2; attempt++) {
                for (int col = 0; col < 12; col += (vegSize > 1 ? 2 : 1)) {
                    for (int row = 0; row < 10; row += (vegSize > 2 ? 2 : 1)) {
                        int index = row * 12 + col + 1;
                        var square = Browser.GetElementById("f" + index);
                        if (squareFilter(square))
                            Browser.Click(square);
                    }
                }
                Func<bool> successAssertion = () => Browser.GetElementsByIdLike("f[0-9]+", "gardenarea").All(s => !squareFilter(s));
                if (Browser.WaitFor(successAssertion, 1000, 8000))
                    return true;
            }
            return false;
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
            var egg = Store.Products[9];
            var grain = Store.Vegetables[1];
            var corn = Store.Vegetables[2];

            var cheaperFood = (2 * grain.BuyPrice < corn.BuyPrice) ? grain : corn;
            bool feedFull = true;

            if (egg.LastSellPrice * 0.9 < cheaperFood.BuyPrice * (cheaperFood.ID == 1 ? 12 : 6)) { 
                cheaperFood = (grain.BuyPrice < corn.BuyPrice ? grain : corn);
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
