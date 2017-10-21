using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    public class Wood : Product {
        public override bool IsAvailable {
            get {
                if (Quantity > 0)
                    return true;
                else if (Ingredients.Any())
                    return Ingredients.All(i => i.Product.IsAvailable);
                else
                    return false;
            }
        }
    }
}
