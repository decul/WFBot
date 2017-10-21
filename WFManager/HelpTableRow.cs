using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    public class HelpTableRow {
        private Dictionary<string, HtmlElement> cells = new Dictionary<string, HtmlElement>();

        public static List<HelpTableRow> getRows() {
            HtmlElement helpContent = Browser.GetElementById("newhelp_content");
            var table = Browser.GetChildrenByClass(helpContent, "newhelp_table").Last();

            var cols = new List<string>();
            var headers = Browser.getOffspringByClass(table, "newhelp_table_head")[0].Children;
            for (int i = 0; i < headers.Count; i++)
                cols.Add(headers[i].InnerText);

            var result = new List<HelpTableRow>();
            var rows = Browser.getOffspringByClass(table, "newhelp_line");
            for (int r = 0; r < rows.Count; r++) {
                var ptr = new HelpTableRow();
                for (int c = 0; c < cols.Count; c++) 
                    ptr.cells.Add(cols[c], rows[r].Children[c]);
                result.Add(ptr);
            }

            return result;
        }
        
        public int ID {
            get {
                return Util.ParseProductId(cells["Produkt"].Children[0].GetAttribute("className"));
            }
        }

        public string Name {
            get { return cells["Produkt"].Children[1].InnerText.Trim(); }
        }

        public TimeSpan GrowthTime {
            get {
                try {
                    return Util.ParseTimeSpan(cells["Czas"].InnerText);
                } catch (Exception) {
                    return TimeSpan.MaxValue;
                }
            }
        }

        public int HarvestPerIndividual {
            get {
                if (cells.ContainsKey("Zbiór")) {
                    var match = Regex.Match(cells["Zbiór"].InnerText, "[0-9]+");
                    if (match.Success)
                        return int.Parse(match.Value);
                }
                return 1;
            }
        }

        public int BonusPoints {
            get {
                if (cells.ContainsKey("Punkty")) {
                    var match = Regex.Match(cells["Punkty"].InnerText, "[0-9]+(.[0-9]+)?");
                    if (match.Success) {
                        return int.Parse(match.Value.Replace(".", ""));
                    }
                }
                return 0;
            }
        }

        public double Price {
            get {
                try {
                    return Util.ParsePrice(cells["Koszta"].InnerText);
                } catch (Exception) {
                    return 0.0;
                }
            }
        }

        public int Size {
            get {
                if (!cells.ContainsKey("Pola"))
                    return 0;

                var sizeStr = cells["Pola"].InnerText.Trim();
                return sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2);
            }
        }

        public List<Ingredient> Ingredients {
            get {
                var ingredients = new List<Ingredient>();
                if (!cells.ContainsKey("Potrzebne"))
                    return ingredients;

                var ingRows = cells["Potrzebne"].Children;
                for (int i = 0; i < ingRows.Count; i += 3) {
                    int id = Util.ParseProductId(Browser.GetClassName(ingRows[i]));

                    int count = 1;
                    var match = Regex.Match(ingRows[i + 1].InnerText, "[0-9]+x");
                    if (match.Success)
                        count = int.Parse(match.Value.Replace("x", ""));

                    ingredients.Add(new Ingredient(id, count));
                }
                return ingredients;
            }
        }

    }
}
