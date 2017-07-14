using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager.ProductNS {

    [Serializable]
    public class PriceRecord {
        public DateTime date;
        public double? price;


        public PriceRecord() { }

        public PriceRecord(DateTime date, double? price) {
            this.date = date;
            this.price = price;
        }
    }
}
