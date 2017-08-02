using System;
using System.Collections.Generic;

namespace WFManager {
    class Picnic : Product {
        List<ShopListItem> Ingredients;




        /// <exception cref="ProductNotAvailableException">
        ///     Thrown when one of ingredients is not available on current level
        /// </exception>
        /// <exception cref="ProductNotAccessibleException">
        ///     Thrown when one of ingredients is not currently accessible on market
        /// </exception>
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
