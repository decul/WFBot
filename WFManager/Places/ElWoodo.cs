﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    public static class ElWoodo {
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
            foreach (HtmlElement square in Browser.GetElementsByClass("forestry_pos", "div")) {
                if (canBeHarvested(square)) {
                    square.InvokeMember("click");
                }
            }
            waitForAllTreesToHarvest();


            // Chose tree to plant
            //Browser.MouseOver("forestry_stock1_object");
            //Browser.WaitForId("f_stock_item1");

            // TODO: Chose tree to plant

            Browser.Click("f_stock_item1");
            Browser.WaitForId("forestry_stock1_select");

            //Browser.MouseOut("forestry_stock1_object");
            //Browser.Wait(1000);
            
            // Plant trees
            foreach (HtmlElement square in Browser.GetElementsByClass("forestry_pos", "div")) {
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
            if (!Browser.WaitFor(predicate, postWaitTime, 10000))
                throw new QuietException("Not all trees were planted");
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

            Browser.Click("forestry_building_click1");
            Browser.WaitForId("forestry_building_inner_slot_info1");

            var button = Browser.GetElementById("forestry_building_inner_slot_info1");

            if (button.InnerText.Contains("Gotowe")) {
                // Harvest product
                Browser.Click(button);
                Browser.WaitFor(() => Browser.GetElementById("forestry_building_inner_slot_info1").InnerText.Contains("Rozpocznij produkcję"));
            } else {
                // Return if not ready
                try {
                    return Util.ParseTimeSpan(button.InnerText);
                } catch (Exception) { }
            }

            // Click on Start production
            Browser.Click("forestry_building_inner_slot_info1");
            Browser.WaitForId("forestry_selectproduction_scrollcontent");

            // Select product
            Browser.Click(Browser.GetElementById("forestry_selectproduction_scrollcontent").Children[2]);
            Browser.WaitForId("globalbox_button1");

            // Confirm
            Browser.Click("globalbox_button1");
            Browser.WaitFor(() => Browser.GetElementById("forestry_building_inner_slot_info1").InnerText.Contains(":"));
            
            // Read time remaining
            var timeInfo = Browser.GetElementById("forestry_building_inner_slot_info1").InnerText;
            Browser.InvokeScript("closeForestryBuildingInner");
            return Util.ParseTimeSpan(timeInfo);
        }




        public static void ServeCustomers() {
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

    }
}
