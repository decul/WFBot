using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFStats.ProductNS {
    public class Vegetable : Product {
        public int Size;
        public double BasePrice;

        public double? LastMarketPrice {
            get {
                if (PriceHistory != null) {
                    PriceRecord pr = PriceHistory.LastOrDefault();
                    if (pr != null)
                        return pr.price;
                }
                return null;
            }
        }

        public double LowestPriceNow {
            get {
                double? pr = LastMarketPrice;
                if (pr.HasValue && pr.Value < BasePrice)
                    return pr.Value;
                else
                    return BasePrice;
            }
        }

        public double HourlyHarvestPerField {
            get {
                return (HarvestFromIndividual - 1) * 120.0 / Size / GrowthTime.TotalHours;
            }
        }

        public double HourlyBonusPerField {
            get {
                return BonusPointsPerSquare * 120.0 / Size / GrowthTime.TotalHours;
            }
        }


        //public double AvgMarketPriceThisDay {
        //    get {

        //    }
        //}
        
        public double HourlyProfitPerField(double? marketPrice) {
            double lowPrice = marketPrice == null ? BasePrice : Math.Min(marketPrice.Value, BasePrice);
            return lowPrice * 0.9 * HourlyHarvestPerField;
        }

        public double? PercentageMarketPriceExcess(double? marketPrice) {
            if (!marketPrice.HasValue)
                return null;
            return marketPrice.Value / BasePrice * 100;
        }

    }
}
