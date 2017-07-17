using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WFStats.Places;
using WFStats.ProductNS;

namespace WFStats {
    public static class HttpServer {

        private static HttpListener listener = new HttpListener();
        private static object Vegetables;

        public static void StartListenning() {
            if (listener.IsListening)
                return;

            if (!HttpListener.IsSupported)
                throw new Exception("HttpListener not supported in this OS");

            // Set up listenner
            string url = "http://+:80/Temporary_Listen_Addresses/WFManager/";
            listener.Prefixes.Add(url);

            // Start listenning
            listener.Start();
            listener.BeginGetContext(Recive, listener);
        }

        private static void Recive(IAsyncResult result) {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;

            string requestedContent = request.Headers.Get("requested_resource");
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vegetable>), new XmlRootAttribute("List"));
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                serializer.Serialize(writer, Store.Vegetables.Select(p => p.Value).ToList());
            

            StreamReader reader = new StreamReader(request.InputStream);
            string content = reader.ReadToEnd();

            context.Response.Close();
            listener.BeginGetContext(Recive, listener);
        }
    }
}
