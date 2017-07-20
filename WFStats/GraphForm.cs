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
    }
}
