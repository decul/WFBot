using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace WFManager {
    [XmlRoot("SDictionary")]
    public class SDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable {

        public System.Xml.Schema.XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader) {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys) {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        public Dictionary<TKey, TValue> ToDictionary() {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (var pos in this)
                dict.Add(pos.Key, pos.Value);
            return dict;
        }
    }


    public class SDictionary {
        public static SDictionary<int, T> FromList<T>(List<T> list) where T : Product {
            SDictionary<int, T> dict = new SDictionary<int, T>();
            foreach (var pos in list)
                dict.Add(pos.ID, pos);
            return dict;
        }
    }
}