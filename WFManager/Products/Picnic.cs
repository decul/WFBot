using System;
using System.Collections.Generic;
using System.Linq;

namespace WFManager {
    public class Picnic : Product {
        public Picnic() { }
        public Picnic(int id) : base(id) { }

        

        
        public override double? DailyIncomePerUnit(double? givenPrice) {
            // Not implemented due to not enaugh info about production cost at given time
            // Method call and implementation have to be changed
            throw new NotImplementedException();

            //if (givenPrice == null)
            //    return null;

            //return (givenPrice * 0.95 * HarvestFromIndividual - ProductionCost) / GrowthTime.TotalDays;
        }
        
        /// <exception cref="AvailabilityException"></exception>
        public override double DailyIncomePerUnit() {
            return (LastSellPrice * 0.95 * HarvestFromIndividual - LastProductionCost) / GrowthTime.TotalDays;
        }
        
    }
}
