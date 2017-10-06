using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace WFManager {
    class HttpClient {
        private const string url = "http://192.168.1.88:80/Temporary_Listen_Addresses/WFManager/";

        //public static List<Vegetable> AvailableVegetables {
        //    get {
        //        try {
        //            return Serializer.Deserialize<Vegetable>(RequestResource("AvailableVegetables"));
        //        } catch (Exception) {
        //            return new List<Vegetable>();
        //        }
        //    }
        //}

        //public static List<Diary> AvailableDiaries {
        //    get {
        //        try {
        //            return Serializer.Deserialize<Diary>(RequestResource("AvailableDiaries"));
        //        } catch (Exception) {
        //            return new List<Diary>();
        //        }
        //    }
        //}

        public static void UpdateStore() {
            Store.Deserialize(RequestResource("Store"));
        }

        public static Stream RequestResource(string resourceName) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Set("requested_resource", resourceName);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("HttpWebResponse status code: (" + response.StatusCode + ") " + response.StatusDescription);
            
            return response.GetResponseStream();
        }

        //public static void SendResource(Stream resource) {
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.ContentType = "text/xml";
        //    request.Method = "POST";
        //    request.KeepAlive = true;
        //    request.Credentials = CredentialCache.DefaultCredentials;
            
        //    resource.CopyTo(request.GetRequestStream());
            
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    if (response.StatusCode != HttpStatusCode.OK)
        //        throw new Exception("HttpWebResponse status code: (" + response.StatusCode + ") " + response.StatusDescription);
        //}
    }
}
