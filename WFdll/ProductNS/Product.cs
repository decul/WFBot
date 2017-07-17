using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace WFStats.ProductNS {

    [Serializable]
    public class Product {
        
        public int ID;
        public string Name;

        [XmlIgnore]
        public TimeSpan GrowthTime;
        public int HarvestFromIndividual;
        public int BonusPointsPerSquare;

        [XmlElement("PriceRecord")]
        public List<PriceRecord> PriceHistory = new List<PriceRecord>();



        // Used only for serialization
        [XmlElement("GrowthTime")]
        public long GrowthTimeTicks {
            get { return GrowthTime.Ticks; }
            set { GrowthTime = new TimeSpan(value); }
        }
        


        public double? LastNotNullMarketPrice {
            get {
                PriceRecord record = PriceHistory.LastOrDefault(r => r.price.HasValue);
                if (record != null)
                    return record.price;
                return null;
            }
        }

        //public double? AvgPriceDay() {
        //    int count = 0;
        //    double sum = 0.0;
        //    for (int p = prices.Count - 1; p >= 0; p--) {
        //        if (prices[p].date < DateTime.Now - TimeSpan.FromHours(24))
        //            break;
        //        count++;
        //        double? price = prices[p].price;
        //        var prod = Product.VegetablesList[productId];
        //        if (prod != null && (!price.HasValue || price.Value > prod.BasePrice))
        //            price = prod.BasePrice;

        //        if (price.HasValue)
        //            sum += price.Value;
        //        else
        //            count--;
        //    }
        //    if (count > 0)
        //        return sum / count;
        //    else
        //        return null;
        //}

        public void AddPrice(double? price) {
            PriceHistory.Add(new PriceRecord(DateTime.Now, price));
        }


    }
}
