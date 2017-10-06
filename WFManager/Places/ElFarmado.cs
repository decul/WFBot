using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WFManager {
    public class ElFarmado {

        static public void UpdatePrices() {
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

            // Go to All products
            Browser.Click("market_navi5");
            Browser.WaitForId("marketcategories");

            for (int cat = 1; cat <= 8; cat++) {
                // Check only vegetables, diary and picnics
                if (!new List<int>(){ 1, 2, 4 }.Contains(cat))
                    continue;

                // Go to Product category
                Browser.Click("market_navi_cat" + cat);
                Browser.Wait(2000);
                
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
            string priceStr = Browser.GetChildrenByClass(firstRow, "market_price").First().InnerText
                        .Split(new string[] { "ft" }, StringSplitOptions.RemoveEmptyEntries).First().Trim();
            priceStr = Regex.Replace(priceStr, "[^0-9,]", "").Replace(',', '.');
            return double.Parse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture);
        }


    }
}
