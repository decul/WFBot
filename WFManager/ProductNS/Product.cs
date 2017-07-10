using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WFManager.ProductNS {

    [Serializable]
    public class Product {
        static public Dictionary<int, Vegetable> Vegetables = new Dictionary<int, Vegetable>();
        static public Dictionary<int, Diary> Diaries = new Dictionary<int, Diary>();



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






        static public List<Product> AvailableProducts {
            get {
                List<Product> result = new List<Product>();
                foreach (var product in Vegetables.Values) {
                    if (product.PriceHistory.Any())
                        result.Add(product);
                }
                foreach (var product in Diaries.Values) {
                    if (product.PriceHistory.Any())
                        result.Add(product);
                }
                return result;
            }
        }

        static public List<Vegetable> AvailableVegetables {
            get {
                List<Vegetable> result = new List<Vegetable>();
                foreach (var product in Vegetables.Values) {
                    if (product.PriceHistory.Any())
                        result.Add(product);
                }
                return result;
            }
        }

        static public List<Diary> AvailableDiaries {
            get {
                List<Diary> result = new List<Diary>();
                foreach (var product in Diaries.Values) {
                    if (product.PriceHistory.Any())
                        result.Add(product);
                }
                return result;
            }
        }

        static public Product Get(int productId) {
            if (Vegetables.ContainsKey(productId))
                return Vegetables[productId];
            if (Diaries.ContainsKey(productId))
                return Diaries[productId];
            return null;
        }
        


        static public void UpdateProductsInfo() {
            // Go to Help
            Browser.Click("mainmenue5");
            Browser.WaitForId("newhelp_menue_item_products_v");

            updateVegetablesInfo();
            updateProductsInfo("newhelp_menue_item_products_e", "kp9");

            Serialize();

            // Close Help
            Browser.GetChildrenByClass(Browser.GetElementById("newhelp"), "mini_close")[0].InvokeMember("click");
        }



        static private void updateVegetablesInfo() {
            // Go to Vegetables Products (must be in help first)
            Browser.Click("newhelp_menue_item_products_v");
            Browser.WaitForClass("kp17", "div");
            
            var content = Browser.GetElementById("newhelp_content");
            var rows = Browser.getOffspringByClass(content, "newhelp_line", "td");
            foreach (HtmlElement row in rows) {
                try {
                    Vegetable pinfo = new Vegetable();
                    pinfo.ID = int.Parse(row.Children[0].Children[0].GetAttribute("className")
                                  .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);

                    if (Vegetables.ContainsKey(pinfo.ID))
                        pinfo = Vegetables[pinfo.ID];
                    else
                        Vegetables.Add(pinfo.ID, pinfo);

                    pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

                    var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
                    pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

                    pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);

                    var sizeStr = row.Children[3].InnerText.Trim();
                    pinfo.Size = (sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2));

                    var points = row.Children[4].InnerText.Replace(".", "").Trim();
                    int.TryParse(points, out pinfo.BonusPointsPerSquare);

                    var price = Regex.Replace(row.Children[5].InnerText, "[^0-9,]", "").Replace(',', '.');
                    double.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out pinfo.BasePrice);

                } catch (Exception exc) {
                    Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
                }
            }
        }

        static private void updateProductsInfo(string buttonId, string expectedElementClass) {
            // Go to Products Category (must be in help first)
            Browser.Click(buttonId);
            Browser.WaitForClass(expectedElementClass, "div");
            
            var content = Browser.GetElementById("newhelp_content");
            var rows = Browser.getOffspringByClass(content, "newhelp_line", "td");
            foreach (HtmlElement row in rows) {
                try {
                    var pinfo = new Diary();
                    pinfo.ID = int.Parse(row.Children[0].Children[0].GetAttribute("className")
                                       .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);

                    if (Diaries.ContainsKey(pinfo.ID))
                        pinfo = Diaries[pinfo.ID];
                    else
                        Diaries.Add(pinfo.ID, pinfo);

                    pinfo.Name = row.Children[0].Children[1].InnerText.Trim();

                    var time = Regex.Replace(row.Children[1].InnerText, "[^0-9:]", "").Split(':');
                    pinfo.GrowthTime = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));

                    pinfo.HarvestFromIndividual = int.Parse(row.Children[2].InnerText);
                    
                    var points = row.Children[3].InnerText.Replace(".", "").Trim();
                    int.TryParse(points, out pinfo.BonusPointsPerSquare);

                } catch (Exception exc) {
                    Logger.Error("Cannot load vegtable info: " + exc.Message + "\nat\n" + row.OuterHtml.Trim());
                }
            }
        }



        static public void Serialize() {
            if (!Directory.Exists(WF.storagePath))
                Directory.CreateDirectory(WF.storagePath);
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("Vegetables"));
            using (TextWriter writer = new StreamWriter(WF.storagePath + "\\Vegetables.xml"))
                serializer.Serialize(writer, Vegetables.Select(p => p.Value).ToList());

            serializer = new XmlSerializer(typeof(List<Diary>), new XmlRootAttribute("Diaries"));
            using (TextWriter writer = new StreamWriter(WF.storagePath + "\\Diaries.xml"))
                serializer.Serialize(writer, Diaries.Select(p => p.Value).ToList());
        }

        static public void Deserialize() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("Vegetables"));
            try {
                using (FileStream file = new FileStream(WF.storagePath + "\\Vegetables.xml", FileMode.Open))
                    Vegetables = ((List<Vegetable>)serializer.Deserialize(file)).ToDictionary(p => p.ID, p => p);
            } catch (FileNotFoundException exc) { } catch (DirectoryNotFoundException exc) { }

            serializer = new XmlSerializer(typeof(List<Diary>), new XmlRootAttribute("Diaries"));
            try {
                using (FileStream file = new FileStream(WF.storagePath + "\\Diaries.xml", FileMode.Open))
                    Diaries = ((List<Diary>)serializer.Deserialize(file)).ToDictionary(p => p.ID, p => p);
            } catch (FileNotFoundException exc) { } catch (DirectoryNotFoundException exc) { }
        }





        //static public void createConstants() {
        //    string qwer = "";
        //    foreach (Diary veg in Diaries.Values)
        //        qwer += "public const int " + veg.Name.Replace(" ", "_") + "_id = " + veg.ID + ";\n";
        //    Logger.Info(qwer);
        //}
    }
}
