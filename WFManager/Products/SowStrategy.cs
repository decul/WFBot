using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WFManager {

    [Serializable]
    public class SowStrategy {

        public int ProductId;
        public int EndHour;


        [XmlIgnore]
        public Vegetable Vegetable {
            get { return Store.Vegetables[ProductId]; }
        }




        static public List<SowStrategy> Load() {
            return Serializer.LoadFromFile<List<SowStrategy>>(WF.storagePath + "\\SowStrategy.xml");
        }

        static public List<Vegetable> Vegetables {
            get { return Load().Select(s => s.Vegetable).Distinct().ToList(); }
        }
    }
}
