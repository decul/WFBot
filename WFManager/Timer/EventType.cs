using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {

    [Serializable]
    public enum EventType {
        PRICES_UPDATE, CROP, FEED_CHICKENS, CHECK_MAIL, LOTTERY, PLANT_TREES, SERVE_WOOD_NPC
    }
}
