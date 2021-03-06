﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WFManager {
    static public class Timer {
        static private bool stopped = false;

        static private List<Event> events = new List<Event>();
        
        static public void Stop() {
            stopped = true;
        }

        static public void Run() {
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

            if (events.Find(ev => ev.type == EventType.PLANT_TREES) == null)
                events.Add(new Event(DateTime.Now, EventType.PLANT_TREES));

            if (events.Find(ev => ev.type == EventType.SERVE_WOOD_NPC) == null)
                events.Add(new Event(DateTime.Now, EventType.SERVE_WOOD_NPC));

            if (events.Find(ev => ev.type == EventType.CUT_WOOD) == null)
                events.Add(new Event(DateTime.Now, EventType.CUT_WOOD));

            

            while (!stopped) {
                Browser.Wait(1000);
                
                foreach (var ev in events) {
                    try {
                        if (ev.date <= DateTime.Now) {
                            WF.LogIn();
                            switch (ev.type) {
                                case EventType.PRICES_UPDATE:
                                    ElFarmado.UpdatePrices();
                                    if (ev.date > DateTime.Now - TimeSpan.FromHours(0.5))
                                        ev.date += TimeSpan.FromHours(1);
                                    else
                                        ev.date = DateTime.Now + TimeSpan.FromHours(1);
                                    break;

                                case EventType.CROP:
                                    var strategy = SowStrategy.Load();
                                    int productId = strategy.Last().ProductId;
                                    foreach (var st in strategy) {
                                        if (st.EndHour > DateTime.Now.Hour) {
                                            productId = st.ProductId;
                                            break;
                                        }
                                    }
                                    ev.date = DateTime.Now + Farm.SowFields(productId).Add(TimeSpan.FromMinutes(1));
                                    break;

                                case EventType.FEED_CHICKENS:
                                    ev.date = DateTime.Now + Farm.FeedChickens();
                                    break;

                                case EventType.CHECK_MAIL:
                                    WF.CheckMail();
                                    ev.date = DateTime.Now + TimeSpan.FromMinutes(1);
                                    break;

                                case EventType.LOTTERY:
                                    ElFishado.CollectFreeProducts();
                                    while (ev.date <= DateTime.Now)
                                        ev.date = ev.date.AddDays(1);
                                    break;

                                case EventType.SERVE_WOOD_NPC:
                                    ElWoodo.ServeCustomers();
                                    ev.date = DateTime.Now + TimeSpan.FromHours(1);
                                    break;

                                case EventType.PLANT_TREES:
                                    ev.date = DateTime.Now + ElWoodo.PlantTrees();
                                    break;

                                case EventType.CUT_WOOD:
                                    ev.date = DateTime.Now + ElWoodo.CutTheWood();
                                    break;

                            }
                            SerializeEvents();
                        }
                    } 
                    catch (ObjectDisposedException e) {
                        stopped = true;
                    } 
                    catch (QuietException e) {
                        Logger.Info(e.Message);
                    }
                    catch (Exception e) {
                        var type = e.GetType();
                        Logger.Error(e);
                    }

                    if (stopped)
                        break;
                }
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
