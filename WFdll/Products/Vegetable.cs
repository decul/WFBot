using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    public class Vegetable : Product {
        public Vegetable() { }
        public Vegetable(int id) : base(id) { }

        public int Size;




        /// <exception cref="AvailabilityException"></exception>
        public override double BuyPrice {
            get {
                try {
                    return Math.Min(MarketPrice, BasePrice);
                } catch (MarketAvailabilityException) {
                    return BasePrice;
                }
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public override double LastBuyPrice {
            get {
                return BuyPrice;
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public override double SellPrice {
            get {
                try {
                    return Math.Min(MarketPrice, BasePrice) * 0.95;
                } catch (MarketAvailabilityException) {
                    return BasePrice * 0.95;
                }
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public override double LastSellPrice {
            get {
                return SellPrice;
            }
        }




        public double DailyHarvestPerField {
            get { return (HarvestFromIndividual - 1) * 120.0 / Size / GrowthTime.TotalDays; }
        }

        public override double DailyBonusPerUnit {
            get {  return BonusPointsPerSquare * 120.0 / Size / GrowthTime.TotalDays; }
        }


        public override double? DailyIncomePerUnit(double? givenPrice) {
            double lowPrice = givenPrice == null ? BasePrice : Math.Min(givenPrice.Value, BasePrice);
            return lowPrice * 0.95 * DailyHarvestPerField;
        }

        /// <exception cref="LvlAvailabilityException"></exception>
        public override double DailyIncomePerUnit() {
            try {
                return DailyIncomePerUnit(MarketPrice).Value;
            } catch (MarketAvailabilityException) {
                return DailyIncomePerUnit(null).Value;
            }
        }

    }
}
