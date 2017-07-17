using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFStats.ProductNS {
    public class Diary : Product {

        public double HourlyHarvestPerAnimal {
            get {
                return HarvestFromIndividual / GrowthTime.TotalHours;
            }
        }

        //public int bestFood {
        //    get {
        //        switch (ID) {
        //            case 9:
        //                // Calculate feeding strategy
        //                var egg = Product.Get(9);
        //                var grain = Product.Vegetables[1];
        //                var corn = Product.Vegetables[2];

        //                var cheaperFood = (2 * grain.LowestPriceNow < corn.LowestPriceNow) ? grain : corn;
        //                bool feedFull = true;

        //                if (egg.LastNotNullMarketPrice * 0.9 < cheaperFood.LowestPriceNow * (cheaperFood.ID == 1 ? 12 : 6)) {
        //                    cheaperFood = (grain.LowestPriceNow < corn.LowestPriceNow ? grain : corn);
        //                    feedFull = false;
        //                }
        //                break;
        //        }
        //    }
        //}

        public double? HourlyProfitPerAnimal(double? price) {
            if (!price.HasValue)
                return null;
            return price * 0.9 * HourlyHarvestPerAnimal;
        }

    }
}
