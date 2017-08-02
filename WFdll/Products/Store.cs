﻿using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace WFManager {
    public class Store {
        
        public SDictionary<int, Vegetable> _vegetables = new SDictionary<int, Vegetable>();
        public SDictionary<int, Diary> _diaries = new SDictionary<int, Diary>();

        

        static private Store store = new Store();

        static public  SDictionary<int, Product> Products {
            get {
                var products = new SDictionary<int, Product>();
                foreach (var prod in store._vegetables)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._diaries)
                    products.Add(prod.Key, prod.Value);
                return products;
            }
        }
        static public SDictionary<int, Vegetable> Vegetables {
            get { return store._vegetables; }
            set { store._vegetables = value; }
        }
        static public SDictionary<int, Diary> Diaries {
            get { return store._diaries; }
            set { store._diaries = value; }
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




        static public void Serialize(Stream stream) {
            Serializer.Serialize(store, stream);
        }

        static public void Deserialize(Stream stream) {
            store = Serializer.Deserialize<Store>(stream);
        }




        static public void XmlSerialize(string dir) {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("Vegetables"));
            using (TextWriter writer = new StreamWriter(dir + "\\Vegetables.xml"))
                serializer.Serialize(writer, Vegetables.Select(p => p.Value).ToList());

            serializer = new XmlSerializer(typeof(List<Diary>), new XmlRootAttribute("Diaries"));
            using (TextWriter writer = new StreamWriter(dir + "\\Diaries.xml"))
                serializer.Serialize(writer, Diaries.Select(p => p.Value).ToList());
        }

        static public void XmlDeserialize(string dir) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("Vegetables"));
            try {
                using (FileStream file = new FileStream(dir + "\\Vegetables.xml", FileMode.Open))
                    Vegetables = SDictionary.FromList((List<Vegetable>)serializer.Deserialize(file));
            } catch (FileNotFoundException exc) { } catch (DirectoryNotFoundException exc) { }

            serializer = new XmlSerializer(typeof(List<Diary>), new XmlRootAttribute("Diaries"));
            try {
                using (FileStream file = new FileStream(dir + "\\Diaries.xml", FileMode.Open))
                    Diaries = SDictionary.FromList((List<Diary>)serializer.Deserialize(file));
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