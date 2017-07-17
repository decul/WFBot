using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WFStats.ProductNS;

namespace WFStats {
    class HttpClient {
        private const string url = "http://192.168.1.53:80/Temporary_Listen_Addresses/WFManager/";

        public static List<Vegetable> Vegetables {
            get {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("List"));
                return( (List<Vegetable>) serializer.Deserialize(RequestResource("Vegetables"))).Where(v => v.PriceHistory.Any()).ToList();
            }
        }

        public static Stream RequestResource(string resourceName) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("HttpWebResponse status code: (" + response.StatusCode + ") " + response.StatusDescription);
            
            return response.GetResponseStream();
        }

        public static void SendResource(Stream resource) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/xml";
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            
            resource.CopyTo(request.GetRequestStream());
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("HttpWebResponse status code: (" + response.StatusCode + ") " + response.StatusDescription);
        }
    }
}
