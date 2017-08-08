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
        
        public SDictionary<int, Vegetable> _vegetables = new SDictionary<int, Vegetable>();
        public SDictionary<int, Diary> _diaries = new SDictionary<int, Diary>();
        public SDictionary<int, Picnic> _juices = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _snacks = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _cakes = new SDictionary<int, Picnic>();
        public SDictionary<int, Picnic> _iceCreams = new SDictionary<int, Picnic>();
        public SDictionary<int, Vegetable> _exotics = new SDictionary<int, Vegetable>();
        public SDictionary<int, Product> _oils = new SDictionary<int, Product>();



        static private Store store = new Store();

        static public  SDictionary<int, Product> Products {
            get {
                var products = new SDictionary<int, Product>();
                foreach (var prod in store._vegetables)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._diaries)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._juices)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._snacks)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._cakes)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._iceCreams)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._exotics)
                    products.Add(prod.Key, prod.Value);
                foreach (var prod in store._oils)
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
        static public SDictionary<int, Picnic> Juices {
            get { return store._juices; }
            set { store._juices = value; }
        }
        static public SDictionary<int, Picnic> Snacks {
            get { return store._snacks; }
            set { store._snacks = value; }
        }
        static public SDictionary<int, Picnic> Cakes {
            get { return store._cakes; }
            set { store._cakes = value; }
        }
        static public SDictionary<int, Picnic> IceCreams {
            get { return store._iceCreams; }
            set { store._iceCreams = value; }
        }
        static public SDictionary<int, Vegetable> Exotics {
            get { return store._exotics; }
            set { store._exotics = value; }
        }
        static public SDictionary<int, Product> Oils {
            get { return store._oils; }
            set { store._oils = value; }
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
