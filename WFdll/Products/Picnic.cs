using System;
using System.Collections.Generic;

namespace WFManager {
    public class Picnic : Product {
        public Picnic() { }
        public Picnic(int id) : base(id) { }


        public List<Ingredient> Ingredients;




        /// <exception cref="AvailabilityException"></exception>
        public double productionCost {
            get {
                double cost = 0.0;
                foreach (var ingredient in Ingredients)
                    cost += ingredient.Product.BuyPrice * ingredient.Quantity;
                return cost;
            }
        }
    }
}
