using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WFManager {
    public partial class GraphForm : Form {
        public GraphForm() {
            InitializeComponent();

            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(0x20ffffff);
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(0x20ffffff);
            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            chart.BackColor = Color.FromArgb(31, 31, 31);
            chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            chart.ChartAreas[0].AxisY.ScrollBar.ButtonColor =
            chart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.FromArgb(127, 127, 127);

            chart.ChartAreas[0].AxisX.ScrollBar.BackColor =
            chart.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.FromArgb(63, 63, 63);

            chart.ChartAreas[0].AxisX.ScrollBar.LineColor =
            chart.ChartAreas[0].AxisX.ScrollBar.LineColor = Color.FromArgb(15, 15, 15);

            chart.Legends[0].BackColor = Color.Black;
            chart.Legends[0].ForeColor = Color.White;

            VegetableButton_Click(new object(), new EventArgs());
        }
        


        private void VegetableButton_Click(object sender, EventArgs e) {
            plotGraph(HttpClient.AvailableVegetables, (product, record) => {
                return product.HourlyProfitPerField(record.price);
            });
        }

        private void DiaryButton_Click(object sender, EventArgs e) {
            plotGraph(HttpClient.AvailableDiaries, (product, record) => {
                return product.HourlyProfitPerAnimal(record.price);
            });
        }

        private void ExcessButton_Click(object sender, EventArgs e) {
            plotGraph(HttpClient.AvailableVegetables, (product, record) => {
                return product.PercentageMarketPriceExcess(record.price);
            });
        }

        private void DiaryPriceButton1_Click(object sender, EventArgs e) {
            plotGraph(HttpClient.AvailableDiaries, (product, record) => {
                return record.price;
            });
        }



        private void plotGraph <T> (List<T> products, Func<T, PriceRecord, double?> valueCalculator) where T : Product {
            List<Series> series = new List<Series>();

            foreach (var product in products) {

                Series seria = new Series(product.Name);
                seria.ChartType = SeriesChartType.Line;
                seria.MarkerStyle = MarkerStyle.Circle;
                
                foreach (var record in product.PriceHistory) 
                    seria.Points.AddXY(record.date, valueCalculator(product, record));

                series.Add(seria);
            }

            chart.Series.Clear();
            series = series.OrderByDescending(s => s.Points.Last().YValues.Last()).ToList();
            foreach (var seria in series)
                chart.Series.Add(seria);

            chart.ChartAreas[0].RecalculateAxesScale();
        }







        private void VegButton_Click(object sender, EventArgs e) {
            List<Vegetable> vegs = HttpClient.AvailableVegetables;

            GridView.Rows.Clear();
            if (vegs.Any())
                GridView.Rows.Add(vegs.Count);
            for (int i = 0; i < vegs.Count; i++) {
                GridView.Rows[i].Cells[NameColumn.Index].Value = vegs[i].Name;
                GridView.Rows[i].Cells[IncomeColumn.Index].Value = vegs[i].HourlyProfitPerField(vegs[i].BasePrice) * 24;
                GridView.Rows[i].Cells[BonusColumn.Index].Value = vegs[i].HourlyBonusPerField * 24;
                GridView.Rows[i].Cells[TimeColumn.Index].Value = vegs[i].GrowthTime.TotalHours;
            }

            double minIncome = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[IncomeColumn.Index].Value);
            double maxIncome = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[IncomeColumn.Index].Value);
            double minBonus = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[BonusColumn.Index].Value);
            double maxBonus = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[BonusColumn.Index].Value);
            double minTime = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[TimeColumn.Index].Value);
            double maxTime = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[TimeColumn.Index].Value);

            for (int i = 0; i < vegs.Count; i++) {
                double income = (double)GridView.Rows[i].Cells[IncomeColumn.Index].Value;
                GridView.Rows[i].Cells[IncomeColumn.Index].Style.BackColor = ColorScale.getColor(income, minIncome, maxIncome);
                double bonus = (double)GridView.Rows[i].Cells[BonusColumn.Index].Value;
                GridView.Rows[i].Cells[BonusColumn.Index].Style.BackColor = ColorScale.getColor(bonus, minBonus, maxBonus);
                double time = (double)GridView.Rows[i].Cells[TimeColumn.Index].Value;
                GridView.Rows[i].Cells[TimeColumn.Index].Style.BackColor = ColorScale.getColor(time, minTime, maxTime);
            }
        }




        private void GridView_SelectionChanged(object sender, EventArgs e) {
            GridView.ClearSelection();
        }
    }
}
