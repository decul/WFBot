using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WFManager;

namespace WFManager {
    public static class Serializer {
        public static void Serialize <T> (List<T> list, Stream stream) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("List"));
            using (StreamWriter writer = new StreamWriter(stream))
                serializer.Serialize(writer, list);
        }

        public static List<T> Deserialize <T> (Stream stream) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("List"));
            return (List<T>) serializer.Deserialize(stream);
        }
    }
}
