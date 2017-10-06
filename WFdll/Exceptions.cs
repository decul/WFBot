using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    public class AvailabilityException : Exception {
        public AvailabilityException() : base() { }
        public AvailabilityException(string msg) : base(msg) { }
    }

    public class MarketAvailabilityException : AvailabilityException {
        public MarketAvailabilityException() : base() { }
        public MarketAvailabilityException(string msg) : base(msg) { }
    }

    public class LvlAvailabilityException : AvailabilityException {
        public LvlAvailabilityException() : base() { }
        public LvlAvailabilityException(string msg) : base(msg) { }
    }
}
