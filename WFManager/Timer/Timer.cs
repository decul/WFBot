using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using WFManager;
using WFManager.ProductNS;

namespace WFManager {
    static public class Timer {
        static private bool stopped = true;

        static private List<Event> events = new List<Event>();
        
        static public void Stop() {
            stopped = true;
        }

        static public void Run() {
            stopped = false;

            if (events.Find(ev => ev.type == EventType.CROP) == null) 
                events.Add(new Event(DateTime.Now, EventType.CROP));

            if (events.Find(ev => ev.type == EventType.FEED_CHICKENS) == null)
                events.Add(new Event(DateTime.Now, EventType.FEED_CHICKENS));

            if (events.Find(ev => ev.type == EventType.CHECK_MAIL) == null) 
                events.Add(new Event(DateTime.Now, EventType.CHECK_MAIL));

            if (events.Find(ev => ev.type == EventType.PRICES_UPDATE) == null)
                events.Add(new Event(DateTime.Now, EventType.PRICES_UPDATE));

            if (events.Find(ev => ev.type == EventType.LOTTERY) == null)
                events.Add(new Event(DateTime.Now, EventType.LOTTERY));


            while (!stopped) {
                foreach (var ev in events) {
                    try {
                        if (ev.date <= DateTime.Now) {
                            switch (ev.type) {
                                case EventType.PRICES_UPDATE:
                                    WF.LogIn();
                                    ElFarmado.UpdatePrices();
                                    if (ev.date > DateTime.Now - TimeSpan.FromHours(0.5))
                                        ev.date += TimeSpan.FromHours(1);
                                    else
                                        ev.date = DateTime.Now + TimeSpan.FromHours(1);
                                    break;

                                case EventType.CROP:
                                    WF.LogIn();
                                    int pId = V.Kalafiory;
                                    //if (DateTime.Now.Hour >= 12)
                                    //    pId = V.Rzepak;
                                    //if (DateTime.Now.Hour >= 19)
                                    //    pId = V.Marchewki;
                                    //if (DateTime.Now.Hour >= 22)
                                    //    pId = V.Zboże;
                                    Farm.SowFields(pId);
                                    ev.date = DateTime.Now + TimeSpan.FromSeconds(Store.Vegetables[pId].GrowthTime.TotalSeconds /* 0.95*/);
                                    break;

                                case EventType.FEED_CHICKENS:
                                    WF.LogIn();
                                    ev.date = DateTime.Now + Farm.FeedChickens();
                                    break;

                                case EventType.CHECK_MAIL:
                                    WF.CheckMail();
                                    ev.date = DateTime.Now + TimeSpan.FromMinutes(1);
                                    break;

                                case EventType.LOTTERY:
                                    ElFishado.CollectFreeProducts();
                                    ev.date = ev.date.AddDays(1);
                                    break;
                            }
                            SerializeEvents();
                        }
                    } catch (ObjectDisposedException e) {
                        stopped = true;
                    } catch (Exception e) {
                        var type = e.GetType();
                        Logger.Error(e.Message + "\n\n" + e.StackTrace);
                    }
                }
                Browser.Wait(1000);
            }
        }



        public static void SerializeEvents() {
            if (!Directory.Exists(WF.storagePath))
                Directory.CreateDirectory(WF.storagePath);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Event>), new XmlRootAttribute("events"));
            using (TextWriter writer = new StreamWriter(WF.storagePath + "\\Events.xml"))
                serializer.Serialize(writer, events);
        }

        public static void DeserializeEvents() {
            events.Clear();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Event>), new XmlRootAttribute("events"));
            try { 
                using (FileStream file = new FileStream(WF.storagePath + "\\Events.xml", FileMode.Open))
                    events = (List<Event>)serializer.Deserialize(file);
            } catch (FileNotFoundException exc) { } catch (DirectoryNotFoundException exc) { }
        }
    }
}
