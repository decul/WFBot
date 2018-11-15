using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WFManager {
    public class ElFarmado {

        static private void goToMarket() {
            // Go to Map
            Browser.Click("mainmenue1");
            Browser.WaitForId("map_city1");

            // Go to El Farmado
            Browser.Click("map_city1");
            Browser.WaitForBus();
            Browser.WaitForId("cityzone_1_5");

            // Go to Market
            Browser.Click("cityzone_1_5");
            Browser.WaitForId("market_navi5");
        }

        static public void UpdatePrices() {
            goToMarket();

            // Go to All products
            Browser.Click("market_navi5");
            Browser.WaitForId("marketcategories");

            for (int cat = 1; cat <= 8; cat++) {
                // Check only vegetables, diary and picnics
                if (!new List<int>(){ 1, 2, 4 }.Contains(cat))
                    continue;

                // Go to Product category
                Browser.Click("market_navi_cat" + cat);
                switch (cat) {
                    case 1:     Browser.WaitForClass("tt1", "marketcategories");    break;
                    case 2:     Browser.WaitForClass("tt9", "marketcategories");    break;
                    case 3:     Browser.WaitForClass("tt130", "marketcategories");  break;
                    default:    Browser.Wait(2 * Browser.WAIT_TIME);                break;
                }
                
                int productsCount = Browser.GetChildrenByClass(Browser.GetElementById("marketcategories"), "market_pframe").Count;
                for (int p = 0; p < productsCount; p++) {
                    // Get product id
                    var productElement = Browser.GetChildrenByClass(Browser.GetElementById("marketcategories"), "market_pframe")[p];
                    int productId = int.Parse(productElement.OuterHtml
                                       .Split(new string[] { "market_filter_pid=" }, StringSplitOptions.None)[1]
                                       .Split(';')[0]);

                    // Continue if product definition doesn't exist
                    if (!Store.Products.ContainsKey(productId)) {
                        Logger.Error("Cannot save price of product with id " + productId + " because its definition does not exist");
                        continue;
                    }

                    // Go to Product
                    productElement.InvokeMember("click");
                    Browser.WaitForId("marktoffers_rows");

                    // Save price
                    Store.Products[productId].AddPrice(getPriceOfFirstRow());

                    // Go back to All products
                    Browser.Click("market_navi5");
                    Browser.WaitForId("marketcategories");
                }
            }

            Store.Save(WF.storagePath);

            // Close market
            Browser.InvokeScript("closeMarket");
        }

        private static double? getPriceOfFirstRow() {
            var firstRow = Browser.GetElementById("marktoffers_rows").FirstChild;
            if (firstRow.All.Count < 2)
                return null;
            return Util.ParsePrice(Browser.GetChildrenByClass(firstRow, "market_price").First().InnerText);
        }



        public static void sellHarvest() {
            goToMarket();

            foreach (var veg in SowStrategy.Vegetables) {
                // Go to All products
                Browser.Click("market_navi5");
                Browser.WaitForId("marketcategories");

                // Go to Make offer
                Browser.Click("market_navi2");
                Browser.WaitForId("market_select_control");

                Browser.InvokeScript("selectMarketProduct", veg.ID.ToString());
                Browser.WaitForId("marketcreateoffer_stockamount");

                int storeQty = Util.ParseQty(Browser.GetElementById("marketcreateoffer_stockamount").InnerText);
                int minQty = 6 * 120 / veg.Size;
                int sellQty = storeQty - minQty;

                if (sellQty > 0) {
                    string price = (veg.BuyPrice - 0.01).ToString();

                    Browser.SetValue("marketnewoffer_amount", sellQty.ToString());
                    Browser.SetValue("marketnewoffer_price1", price.Split(',')[0]);
                    Browser.SetValue("marketnewoffer_price2", price.Split(',')[1]);
                    Browser.Click("market_new_button", "bigme");
                }
                else {
                    Browser.InvokeScript("closeMarketNewOffer");
                }

                Browser.WaitForIdGone("marketnewoffer_setup");
            }
        }

    }
}
