using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    public class NpcHistory {
        public List<Npc> FarmNpcs = new List<Npc>();
        public List<WoodNpc> WoodNpcs = new List<WoodNpc>();
        public List<PicnicNpc> PicnicNpcs = new List<PicnicNpc>();



        static public NpcHistory instance = new NpcHistory();

        const string saveFileName = "\\NpcHistory.xml";
        static string saveFileDir;



        static public void AddTransaction(Npc npc) {
            instance.FarmNpcs.Add(npc);
            Save(saveFileDir);
        }

        static public void AddTransaction(WoodNpc npc) {
            instance.WoodNpcs.Add(npc);
            Save(saveFileDir);
        }

        static public void AddTransaction(PicnicNpc npc) {
            instance.PicnicNpcs.Add(npc);
            Save(saveFileDir);
        }




        static public void recalculatePrices <T> (List<T> transactions) where T : Npc {

        }




        static public void Serialize(Stream stream) {
            Serializer.Serialize(instance, stream);
        }

        static public void Deserialize(Stream stream) {
            instance = Serializer.Deserialize<NpcHistory>(stream);
        }


        static public void Save(string dir) {
            Serializer.SaveToFile(instance, dir + saveFileName);
        }

        static public void Load(string dir) {
            saveFileDir = dir;
            instance = Serializer.LoadFromFile<NpcHistory>(dir + saveFileName);
        }

    }
}
