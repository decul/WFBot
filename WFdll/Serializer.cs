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
        public static void Serialize <T> (T obj, Stream stream) {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Root"));
            using (StreamWriter writer = new StreamWriter(stream))
                serializer.Serialize(writer, obj);
        }

        public static T Deserialize <T> (Stream stream) {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Root"));
            return (T) serializer.Deserialize(stream);
        }
    }
}
