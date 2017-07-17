using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WFdll;
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

            if (request.HttpMethod == "GET") {
                switch (request.Headers.Get("requested_resource")) {
                    case "AvailableVegetables":
                        Serializer.Serialize(Store.AvailableVegetables, context.Response.OutputStream);
                        break;

                    case "AvailableDiaries":
                        Serializer.Serialize(Store.AvailableDiaries, context.Response.OutputStream);
                        break;
                }

                
            }

            //StreamReader reader = new StreamReader(request.InputStream);
            //string content = reader.ReadToEnd();

            context.Response.Close();
            listener.BeginGetContext(Recive, listener);
        }
    }
}
