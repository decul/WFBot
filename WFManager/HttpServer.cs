using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace WFManager {
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

            try {
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "GET") {
                    switch (request.Headers.Get("requested_resource")) {
                        case "Store":
                            Store.Serialize(context.Response.OutputStream);
                            using (FileStream file = new FileStream("Shit", FileMode.Create))
                                Store.Serialize(file);
                            break;

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
            }
            catch (Exception e) {
                //string msg = e.Message;
                //for (var exc = e.InnerException; e != null; e = e.InnerException) {
                //    if (exc.Message != null)
                //        msg += "\n\n" + e.Message;
                //}
                //Logger.Error(msg + "\n\n" + e.StackTrace);
                Logger.Error(e.ToString());
            }

            listener.BeginGetContext(Recive, listener);
        }
    }
}
