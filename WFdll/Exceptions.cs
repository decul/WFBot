using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    class AvailabilityException : Exception { }

    class MarketAvailabilityException : AvailabilityException { }

    class LvlAvailabilityException : AvailabilityException { }
}
