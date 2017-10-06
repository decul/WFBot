﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace WFManager {

    [Serializable]
    public class Product {
        public Product() { }
        public Product(int id) {
            ID = id;
        }


        public int ID;
        public string Name;

        [XmlIgnore]
        public TimeSpan GrowthTime;
        public int HarvestFromIndividual;
        public int BonusPointsPerSquare;
        
        public double BasePrice;

        [XmlElement("PriceRecord")]
        public List<PriceRecord> PriceHistory = new List<PriceRecord>();



        // Used only for serialization
        [XmlElement("GrowthTime")]
        public long GrowthTimeTicks {
            get { return GrowthTime.Ticks; }
            set { GrowthTime = new TimeSpan(value); }
        }

        
        public bool IsAvailable {
            get { return PriceHistory.Any(); }
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

        public double NpcPrice {
            get { return BasePrice * 0.5; }
        }

        /// <exception cref="AvailabilityException"></exception>
        public double MarketPrice {
            get {
                if (!IsAvailable)
                    throw new LvlAvailabilityException();
                PriceRecord record = PriceHistory.Last();
                if (!record.price.HasValue)
                    throw new MarketAvailabilityException();
                return record.price.Value;
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public double LastMarketPrice {
            get {
                if (!IsAvailable)
                    throw new LvlAvailabilityException();
                PriceRecord record = PriceHistory.LastOrDefault(r => r.price.HasValue);
                if (record == null)
                    throw new MarketAvailabilityException();
                return record.price.Value;
            }
        }



        /// <exception cref="AvailabilityException"></exception>
        public virtual double BuyPrice {
            get {
                return MarketPrice;
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public virtual double LastBuyPrice {
            get {
                return LastMarketPrice;
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public virtual double SellPrice {
            get {
                return MarketPrice * 0.95;
            }
        }

        /// <exception cref="AvailabilityException"></exception>
        public virtual double LastSellPrice {
            get {
                return LastMarketPrice * 0.95;
            }
        }




        /// <summary> Unit is one Field (10x12), or one animal, or one factory-building, or whatever </summary>
        public virtual double? DailyIncomePerUnit(double? price) {
            if (price == null)
                return null;
            return price * 0.95 * HarvestFromIndividual / GrowthTime.TotalDays;
        }

        /// <exception cref="AvailabilityException"></exception>
        public virtual double DailyIncomePerUnit() {
            return DailyIncomePerUnit(LastMarketPrice).Value;
        }

        public virtual double DailyBonusPerUnit {
            get { return BonusPointsPerSquare / GrowthTime.TotalDays; }
        }




        public void AddPrice(double? price) {
            PriceHistory.Add(new PriceRecord(DateTime.Now, price));
        }

    }
}
