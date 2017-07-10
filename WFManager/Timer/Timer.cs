using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using WFManager.Places;
using WFManager.ProductNS;

namespace WFManager {
    static public class Timer {
        static private bool stopped = true;
        static private object processing = new object();

        static private List<Event> events = new List<Event>();
        
        static public void Stop() {
            stopped = true;
        }

        static public void Run() {
            stopped = false;

            while (!stopped) {
                try {
                    Monitor.Enter(processing);

                    if (events.Find(ev => ev.type == EventType.CROP) == null) 
                        events.Add(new Event(DateTime.Now, EventType.CROP));

                    if (events.Find(ev => ev.type == EventType.FEED_CHICKENS) == null)
                        events.Add(new Event(DateTime.Now, EventType.FEED_CHICKENS));

                    if (events.Find(ev => ev.type == EventType.PRICES_UPDATE) == null) 
                        events.Add(new Event(DateTime.Now, EventType.PRICES_UPDATE));

                    foreach (var ev in events) {
                        if (ev.date <= DateTime.Now) {
                            switch(ev.type) {
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
                                    int pId = V.Cebule;
                                    if (DateTime.Now.Hour >= 6)
                                        pId = V.Rzepak;
                                    if (DateTime.Now.Hour >= 17)
                                        pId = V.Marchewki;
                                    if (DateTime.Now.Hour >= 22)
                                        pId = V.Zboże;
                                    Farm.SowFields(pId);
                                    ev.date = DateTime.Now + TimeSpan.FromSeconds(Product.Vegetables[pId].GrowthTime.TotalSeconds /* 0.95*/);
                                    break;

                                case EventType.FEED_CHICKENS:
                                    WF.LogIn();
                                    ev.date = DateTime.Now + Farm.FeedChickens();
                                    break;
                            }

                            SerializeEvents();
                        }
                    }
                } catch (Exception e) {
                    Logger.Error(e.Message + "\n\n" + e.StackTrace);

                } finally {
                    Monitor.Exit(processing);
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
