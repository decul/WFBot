using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WFManager;

namespace WFManager {

    [Serializable]
    class ShopListItem {
        [XmlIgnore]
        public Product Product;
        public int Quantity;
        
        // Used only for serialization
        [XmlElement("ProductId")]
        public int productId {
            get { return Product.ID; }
            //set { Product = Store.getProduct(value); }
        }
    }
}
