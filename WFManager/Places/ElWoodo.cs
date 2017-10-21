using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    public static class ElWoodo {
        private static int woodCapacity;
        private static int timberCapacity;
        private static int productsCapacity;



        private static void travelToElWoodo() {
            // Go to Map
            Browser.Click("mainmenue1");
            Browser.WaitForId("map_farm1");

            // Go to El Woodo
            Browser.Click("map_forestry");
            Browser.WaitForBus();
            Browser.WaitForId("forestry_forest");
        }




        public static TimeSpan PlantTrees() {
            travelToElWoodo();

            // Harvest trees
            foreach (HtmlElement square in Browser.GetElementsByClass("forestry_pos", "forestry_forest")) {
                if (canBeHarvested(square)) {
                    square.InvokeMember("click");
                }
            }
            waitForAllTreesToHarvest();

            updateSeedlingsQuantity();
            updateWoodQuantity();

            // Choose tree to plant
            Wood chosenWood = null;
            foreach (var wood in Store.AvailableWoods) {
                if ((chosenWood == null || chosenWood.Quantity > wood.Quantity) && wood.Quantity <= woodCapacity - 25)
                    chosenWood = wood;
            }
            if (chosenWood == null)
                return TimeSpan.FromHours(2);

            Browser.Click(Browser.GetElementsByClass("f_symbol" + chosenWood.Ingredients[0].Product.ID % 10000, "forestry_stock1")[0].Parent);
            Browser.WaitForId("forestry_stock1_select");
            
            // Plant trees
            foreach (HtmlElement square in Browser.GetElementsByClass("forestry_pos", "forestry_forest")) {
                if (canBePlanted(square)) {
                    square.InvokeMember("click");
                }
            }
            waitForAllTreesToPlant();

            return maxRemainingTime();
        }




        private static bool canBePlanted (HtmlElement square) {
            return !square.GetAttribute("className").Contains("tree");
        }

        private static bool canBeHarvested(HtmlElement square) {
            // Check if tree is ready to harvest
            if (!Browser.GetClassName(square).Contains("_3"))
                return false;
            if (Browser.GetClassName(square).Contains("_2"))
                return false;

            // Check if nothing is blocking the tree
            return Browser.GetChildrenByClass(square, "forestry_pos_block")[0].Style.Contains("display: none");
        }

        private static void waitForAllTreesToHarvest(int postWaitTime = 1000) {
            Func<bool> predicate = () => {
                return !Browser.GetElementsByClass("forestry_pos").Where(s => canBeHarvested(s)).Any();
            };
            if (!Browser.WaitFor(predicate, postWaitTime, 10000))
                throw new QuietException("Not all trees were harvested");
        }

        private static void waitForAllTreesToPlant(int postWaitTime = 1000) {
            Func<bool> predicate = () => {
                return !Browser.GetElementsByClass("forestry_pos").Where(s => canBePlanted(s)).Any();
            };
            Browser.WaitFor(predicate, postWaitTime, 10000);
        }

        private static TimeSpan maxRemainingTime() {
            var maxTime = TimeSpan.Zero;
            foreach (HtmlElement info in Browser.GetElementsByClass("forestry_pos_info")) {
                try {
                    var time = Util.ParseTimeSpan(info.InnerText);
                    if (maxTime < time)
                        maxTime = time;
                } catch (Exception) { }
            }
            return maxTime;
        }




        public static TimeSpan CutTheWood() {
            travelToElWoodo();

            // Open Sawmill
            Browser.Click("forestry_building_click1");
            Browser.WaitForId("forestry_building_inner_slot_info1");

            var harvestButton = Browser.GetElementById("forestry_building_inner_slot_info1");
            if (harvestButton.InnerText.Contains("Gotowe")) {
                // Harvest product
                Browser.Click(harvestButton);
                Browser.WaitFor(() => Browser.GetElementById("forestry_building_inner_slot_info1").InnerText.Contains("Rozpocznij produkcję"));
            } else {
                // Return if not ready
                try {
                    TimeSpan time = Util.ParseTimeSpan(harvestButton.InnerText);
                    Browser.InvokeScript("closeForestryBuildingInner");
                    return time;
                } catch (Exception) { }
            }

            ServeCustomers(true);
            updateSeedlingsQuantity();
            updateWoodQuantity();
            updateTimberQuantity();

            // Click on Start production
            Browser.Click("forestry_building_inner_slot_info1");
            Browser.WaitForId("forestry_selectproduction_scrollcontent");

            // Select first product requested by client
            Wood selectedTimber = null;
            foreach (HtmlElement prodElement in Browser.GetElementsByClass("forestry_farmi_uncomplete", "forestry_farmiline")) {
                int productId = Util.ParseProductId(Browser.GetClassName(prodElement.Parent.Children[0]));
                if (Store.Timbers.ContainsKey(productId) && Store.Timbers[productId].EnaughIngredients) {
                    selectedTimber = Store.Timbers[productId];
                    break;
                }
            }

            // Select product with the least quantity if nothing's selected
            if (selectedTimber == null) {
                foreach (var timber in Store.AvailableTimbers) {
                    if ((selectedTimber == null || selectedTimber.Quantity > timber.Quantity) && timber.Quantity <= timberCapacity - timber.HarvestFromIndividual && timber.EnaughIngredients)
                        selectedTimber = timber;
                }
            }
            if (selectedTimber == null) {
                Browser.InvokeScript("closeForestryBuildingInner");
                return TimeSpan.FromHours(2);
            }

            var buttonImgs = Browser.GetElementsByClass("f_symbol" + selectedTimber.ID % 10000, "forestry_selectproduction_scrollcontent");
            Browser.Click(buttonImgs[0].Parent);
            
            // Confirm
            Browser.Click("globalbox_button1");
            Browser.WaitFor(() => Browser.GetElementById("forestry_building_inner_slot_info1").InnerText.Contains(":"));
            
            // Read time remaining
            var timeInfo = Browser.GetElementById("forestry_building_inner_slot_info1").InnerText;
            Browser.InvokeScript("closeForestryBuildingInner");
            return Util.ParseTimeSpan(timeInfo);
        }




        public static void ServeCustomers(bool skipTravel = false) {
            if (!skipTravel)
                travelToElWoodo();

            for (int c = 1; c <= 10; c++) {
                // Check if there is customer in this place
                if (!Browser.isVisible("forestry_farmi" + c)) 
                    break;

                // Check if customer can be served
                if (Browser.isHidden("forestry_farmi_info_button1" + c))
                    continue;
                
                // Gather info about transaction
                var npc = new WoodNpc();
                npc.Payment = Util.ParsePrice(Browser.GetElementById("forestry_farmi_info_price" + c).InnerText);
                npc.BonusPoints = int.Parse(Browser.GetElementById("forestry_farmi_info_points" + c).InnerText);
                npc.Served = true;

                for (int i = 1; i <= 10; i++) {
                    if (Browser.isHidden("forestry_farmi_info_product_line" + c + "_" + i))
                        break;

                    string idString = Browser.GetClassName("forestry_farmi_info_productimg" + c + "_" + i);

                    string qtyString = Browser.GetElementById("forestry_farmi_info_amount" + c + "_" + i).InnerText;
                    qtyString = Regex.Match(qtyString, "[0-9]+").Value;
                        
                    npc.ShoppingList.Add(new Ingredient(Util.ParseProductId(idString), int.Parse(qtyString)));
                }

                Browser.Click("forestry_farmi_info_button1" + c);
                Browser.Wait(2000);
                NpcHistory.AddTransaction(npc);

                var list = string.Join(", ", npc.ShoppingList.Select(i => i.Quantity + "x " + i.ProductId));
                Logger.Info("Wood NPC bougth: " + list + " for " + npc.Payment + "ft and " + npc.BonusPoints + " bonus points");

                c--;
            }
        }




        private static void updateTimberQuantity() {
            // Open timber store
            Browser.Click("forestry_stock3_object");
            Browser.WaitForId("globalbox_content");

            // Read quantities
            var progresBars = Browser.GetElementsByClass("forestry_stockitembar", "globalbox_content");
            foreach (var bar in progresBars) {
                var qtyStr = Regex.Match(bar.Parent.InnerText, "[0-9]+/[0-9]+").Value.Split('/').First();
                int productId = Util.ParseProductId(Browser.GetClassName(bar.Parent.Parent.Children[0]));

                Store.Timbers[productId].Quantity = int.Parse(qtyStr);
            }

            // Read capacity
            var info = Browser.GetElementsByClass("bonusinfo", "globalbox_content")[0].InnerText;
            timberCapacity = int.Parse(Regex.Match(info, "[0-9]+").Value);

            // Close timber store
            Browser.Click("globalbox_close");
            Browser.Wait(1000);
        }

        private static void updateWoodQuantity() {
            // Open wood store
            Browser.Click("forestry_stock2_object");
            Browser.WaitForId("globalbox_content");

            // Read quantities
            var progresBars = Browser.GetElementsByClass("forestry_stockitembar", "globalbox_content");
            foreach (var bar in progresBars) {
                var qtyStr = Regex.Match(bar.Parent.InnerText, "[0-9]+/[0-9]+").Value.Split('/').First();
                int productId = Util.ParseProductId(Browser.GetClassName(bar.Parent.Parent.Children[0]));

                Store.Woods[productId].Quantity = int.Parse(qtyStr);
            }

            // Read capacity
            var info = Browser.GetElementsByClass("bonusinfo", "globalbox_content")[0].InnerText;
            woodCapacity = int.Parse(Regex.Match(info, "[0-9]+").Value);

            // Close wood store
            Browser.Click("globalbox_close");
            Browser.Wait(1000);
        }

        private static void updateSeedlingsQuantity() {
            foreach (var qtyElement in Browser.GetElementsByIdLike("f_stock_amount_", "forestry_stock1")) {
                int productId = Util.ParseProductId(Browser.GetClassName(qtyElement.Parent.Parent.Parent.Children[0]));
                Store.Seedlings[productId].Quantity = int.Parse(qtyElement.InnerText);
            }
        }

    }
}
