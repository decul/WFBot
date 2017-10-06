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
            var table = Browser.GetChildrenByClass(helpContent, "newhelp_table", "table").Last();

            var cols = new List<string>();
            var headers = Browser.getOffspringByClass(table, "newhelp_table_head", "td")[0].Children;
            for (int i = 0; i < headers.Count; i++)
                cols.Add(headers[i].InnerText);

            var result = new List<HelpTableRow>();
            var rows = Browser.getOffspringByClass(table, "newhelp_line", "td");
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
                int id = int.Parse(cells["Produkt"].Children[0].GetAttribute("className")
                        .Split(new string[] { "kp" }, StringSplitOptions.None)[1].Split(' ')[0]);
                if (id == 0)
                    id = -1;
                return id;
            }
        }

        public string Name {
            get { return cells["Produkt"].Children[1].InnerText.Trim(); }
        }

        public TimeSpan GrowthTime {
            get {
                var time = Regex.Replace(cells["Czas"].InnerText, "[^0-9:]", "").Split(':');
                return new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
            }
        }

        public int HarvestPerUnit {
            get { return int.Parse(cells["Zbiór"].InnerText); }
        }

        public int BonusPoints {
            get {
                string bonus = cells["Punkty"].InnerText.Replace(".", "").Trim();
                if (bonus.Any())
                    return int.Parse(bonus);
                else
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
                var sizeStr = cells["Pola"].InnerText.Trim();
                return sizeStr == "1x1" ? 1 : (sizeStr == "2x2" ? 4 : 2);
            }
        }

        public List<Ingredient> Ingredients {
            get {
                var ingredients = new List<Ingredient>();
                var ingRows = cells["Potrzebne"].Children;
                for (int i = 0; i < ingRows.Count; i += 3) {
                    string id = ingRows[i].GetAttribute("className")
                            .Split(new string[] { "kp" }, StringSplitOptions.None)[1]
                            .Split(' ')[0];
                    string count = ingRows[i + 1].InnerText.Split('x').First();
                    ingredients.Add(new Ingredient(int.Parse(id), int.Parse(count)));
                }
                return ingredients;
            }
        }

    }
}
