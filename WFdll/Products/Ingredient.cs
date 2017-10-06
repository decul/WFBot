using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WFManager;

namespace WFManager {

    [Serializable]
    public class Ingredient {
        public Ingredient() { }

        public Ingredient (int productId, int quantity) {
            ProductId = productId;
            Quantity = quantity;
        }



        public int ProductId;
        public int Quantity;
        

        [XmlIgnore]
        public Product Product
        {
            get { return Store.Products[ProductId]; }
            set { ProductId = value.ID; }
        }


        /// <exception cref="AvailabilityException"></exception>
        public double Cost {
            get { return Product.BuyPrice * Quantity; }
        }

        /// <exception cref="AvailabilityException"></exception>
        public double LastCost {
            get { return Product.LastBuyPrice * Quantity; }
        }

    }
}
