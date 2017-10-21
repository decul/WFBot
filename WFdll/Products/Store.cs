using MsgPack.Serialization;
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

        const string saveFileName = "\\Store.xml";

        
        public SDictionary<int, Vegetable> _vegetables = new SDictionary<int, Vegetable>();
        public SDictionary<int, Diary> _diaries = new SDictionary<int, Diary>();
        public SDictionary<int, Picnic> _juices = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _snacks = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _cakes = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _iceCreams = new SDictionary<int, Picnic>();
        public SDictionary<int, Vegetable> _exotics = new SDictionary<int, Vegetable>();
        public SDictionary<int, Product> _oils = new SDictionary<int, Product>();
        public SDictionary<int, Wood> _seedlings = new SDictionary<int, Wood>();
        public SDictionary<int, Wood> _woods = new SDictionary<int, Wood>();
        public SDictionary<int, Wood> _timbers = new SDictionary<int, Wood>();
        public SDictionary<int, Wood> _woodenProducts = new SDictionary<int, Wood>();



        static private Store instance = new Store();

        static public  SDictionary<int, Product> Products {
            get {
                var products = new SDictionary<int, Product>();
                foreach (var prod in instance._vegetables)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._diaries)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._juices)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._snacks)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._cakes)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._iceCreams)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._exotics)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._oils)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._seedlings)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._woods)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._timbers)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in instance._woodenProducts)
                    products.Add(prod.Key, prod.Value);
                return products;
            }
        }
        static public SDictionary<int, Vegetable> Vegetables {
            get { return instance._vegetables; }
            set { instance._vegetables = value; }
        }
        static public SDictionary<int, Diary> Diaries {
            get { return instance._diaries; }
            set { instance._diaries = value; }
        }
        static public SDictionary<int, Picnic> Juices {
            get { return instance._juices; }
            set { instance._juices = value; }
        }
        static public SDictionary<int, Picnic> Snacks {
            get { return instance._snacks; }
            set { instance._snacks = value; }
        }
        static public SDictionary<int, Picnic> Cakes {
            get { return instance._cakes; }
            set { instance._cakes = value; }
        }
        static public SDictionary<int, Picnic> IceCreams {
            get { return instance._iceCreams; }
            set { instance._iceCreams = value; }
        }
        static public SDictionary<int, Vegetable> Exotics {
            get { return instance._exotics; }
            set { instance._exotics = value; }
        }
        static public SDictionary<int, Product> Oils {
            get { return instance._oils; }
            set { instance._oils = value; }
        }
        static public SDictionary<int, Wood> Seedlings {
            get { return instance._seedlings; }
            set { instance._seedlings = value; }
        }
        static public SDictionary<int, Wood> Woods {
            get { return instance._woods; }
            set { instance._woods = value; }
        }
        static public SDictionary<int, Wood> Timbers {
            get { return instance._timbers; }
            set { instance._timbers = value; }
        }
        static public SDictionary<int, Wood> WoodenProducts {
            get { return instance._woodenProducts; }
            set { instance._woodenProducts = value; }
        }



        static public List<Product> AvailableProducts {
            get { return Products.Values.Where(p => p.IsAvailable).ToList(); }
        }

        static public List<Vegetable> AvailableVegetables {
            get { return Vegetables.Values.Where(v => v.IsAvailable).ToList(); }
        }

        static public List<Diary> AvailableDiaries {
            get { return Diaries.Values.Where(d => d.IsAvailable).ToList(); }
        }

        static public List<Picnic> AvailableJuices {
            get { return Juices.Values.Where(j => j.IsAvailable).ToList(); }
        }

        static public List<Picnic> AvailableSnacks {
            get { return Snacks.Values.Where(s => s.IsAvailable).ToList(); }
        }

        static public List<Picnic> AvailableCakes {
            get { return Cakes.Values.Where(c => c.IsAvailable).ToList(); }
        }

        static public List<Picnic> AvailableIceCreams {
            get { return IceCreams.Values.Where(i => i.IsAvailable).ToList(); }
        }

        static public List<Vegetable> AvailableExotics {
            get { return Exotics.Values.Where(e => e.IsAvailable).ToList(); }
        }

        static public List<Product> AvailableOils {
            get { return Oils.Values.Where(o => o.IsAvailable).ToList(); }
        }

        static public List<Wood> AvailableSeedlings {
            get { return Seedlings.Values.Where(w => w.IsAvailable).ToList(); }
        }

        static public List<Wood> AvailableWoods {
            get { return Woods.Values.Where(w => w.IsAvailable).ToList(); }
        }

        static public List<Wood> AvailableTimbers {
            get { return Timbers.Values.Where(w => w.IsAvailable).ToList(); }
        }

        static public List<Wood> AvailableWoodenProducts {
            get { return WoodenProducts.Values.Where(w => w.IsAvailable).ToList(); }
        }






        static public void Serialize(Stream stream) {
            Serializer.Serialize(instance, stream);
        }

        static public void Deserialize(Stream stream) {
            instance = Serializer.Deserialize<Store>(stream);
        }


        static public void Save(string dir) {
            Serializer.SaveToFile(instance, dir + saveFileName);
        }

        static public void Load(string dir) {
            instance = Serializer.LoadFromFile<Store>(dir + saveFileName);
        }




        //static public void createConstants() {
        //    string qwer = "";
        //    foreach (Diary veg in Diaries.Values)
        //        qwer += "public const int " + veg.Name.Replace(" ", "_") + "_id = " + veg.ID + ";\n";
        //    Logger.Info(qwer);
        //}
    }
}
