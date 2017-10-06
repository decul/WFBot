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


        public static void SaveToFile <T> (T obj, string path) {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (FileStream file = new FileStream(path, FileMode.Create))
                Serialize(obj, file);
        }

        /// <summary> Returns object loaded from path, or if file does not exist, returns empty object </summary>
        public static T LoadFromFile <T> (string path) {
            if (File.Exists(path)) {
                using (FileStream file = new FileStream(path, FileMode.Open))
                    return Deserialize<T>(file);
            } 
            else
                return Activator.CreateInstance<T>(); ;
        }
    }
}
