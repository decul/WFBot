using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {

    [Serializable]
    public class Npc {
        public List<Ingredient> ShoppingList = new List<Ingredient>();
        public double Payment;
        public bool Served;


        //public double IngredientsCost {
        //    get {
        //        ShoppingList.Select(i => i.Cost).Sum();
        //    }
        //}
    }
}
