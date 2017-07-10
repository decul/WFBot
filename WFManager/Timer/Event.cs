using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {

    [Serializable]
    public class Event {
        public DateTime date;
        public EventType type;

        public Event() { }

        public Event(DateTime date, EventType type) {
            this.date = date;
            this.type = type;
        }
    }
}
