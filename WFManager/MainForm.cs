using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WFManager {
    public partial class MainForm : Form {
        public MainForm(string[] args) {
            try {
                RegistrySetup.SetBrowserFeatureControl();
                InitializeComponent();

                if (!args.Contains("-h"))
                    Show();

                notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", (o, e) => Application.Exit()),
                    new MenuItem("Safe Exit", (o, e) => Timer.Stop())
                });

                Browser.Initialize(webBrowser);
                Store.Load(WF.storagePath);
                NpcHistory.Load(WF.storagePath);
                Timer.DeserializeEvents();
                Logger.Initialize(infoLabel);
                GraphInit();
                //HttpServer.StartListenning();

                while (true) {
                    try {
                        Logger.Label("hello");
                        WF.LogIn("owczy_farmer", "farmernia", 10);
                        WF.hideAds();
                        break;
                    } catch (ObjectDisposedException) {
                        break;
                    } catch (Exception) {
                        Browser.Wait(10000);
                    }
                }

                if (!Store.Seedlings.ContainsKey(10010))
                    WF.UpdateProductsInfo();

                Timer.Run();
            }
            catch (Exception e) {
                Logger.Error(e);
            }
            Application.Exit();
        }
        
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            Show();
            Activate();
        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Hide();
                notifyIcon.ShowBalloonTip(1000, "WF Manager", "WF Manager is still running in backgroung", ToolTipIcon.Info);
            } else {
                Timer.Stop();
            }
        }




        const string VEGETABLES = "Warzywa";
        const string DIARIES = "Zwierzęce";
        const string JUICES = "Soki";

        const string INCOME = "Zarobek";
        const string PRICE = "Cena";




        public void GraphInit() {
            //typeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            typeComboBoxLel.Items.Add(INCOME);
            typeComboBoxLel.Items.Add(PRICE);
            typeComboBoxLel.SelectedIndex = 0;

            categoryComboBox.BringToFront();
            //categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryComboBox.Items.Add(VEGETABLES);
            categoryComboBox.Items.Add(DIARIES);
            categoryComboBox.Items.Add(JUICES);
            categoryComboBox.SelectedIndex = 0;

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

            //HttpClient.UpdateStore();

            refreshStats(new object(), new EventArgs());
        }



        private void refreshStats(object sender, EventArgs e) {
            List<Product> products = new List<Product>();

            switch ((string)categoryComboBox.SelectedItem) {
                case VEGETABLES:
                    products = Store.AvailableVegetables.ConvertAll(p => (Product)p);
                    break;

                case DIARIES:
                    products = Store.AvailableDiaries.ConvertAll(p => (Product)p);
                    break;

                case JUICES:
                    products = Store.AvailableJuices.ConvertAll(p => (Product)p);
                    break;

                case null:
                    return;

                default:
                    throw new Exception("Product category handler not defined");
            }

            fillTable(products);

            switch ((string)typeComboBoxLel.SelectedItem) {
                case PRICE:
                    plotGraph(products, (product, record) => {
                        return record.price;
                    });
                    break;

                case INCOME:
                    plotGraph(products, (product, record) => {
                        return product.DailyIncomePerUnit(record.price);
                    });
                    break;

                case null:
                    return;

                default:
                    throw new Exception("Chart type handler not defined");
            }
        }



        private void plotGraph<T>(List<T> products, Func<T, PriceRecord, double?> valueCalculator) where T : Product {
            List<Series> series = new List<Series>();

            try {
                foreach (var product in products) {

                    Series seria = new Series(product.Name);
                    seria.ChartType = SeriesChartType.Line;
                    seria.MarkerStyle = MarkerStyle.Circle;

                    foreach (var record in product.PriceHistory)
                        seria.Points.AddXY(record.date, valueCalculator(product, record));

                    series.Add(seria);
                }
            } catch (NotImplementedException) { }

            chart.Series.Clear();
            series = series.OrderByDescending(s => s.Points.Last().YValues.Last()).ToList();
            foreach (var seria in series)
                chart.Series.Add(seria);

            chart.ChartAreas[0].RecalculateAxesScale();

            chart.Refresh();
        }



        private void fillTable<T>(List<T> products) where T : Product {
            GridView.Rows.Clear();
            if (products.Any())
                GridView.Rows.Add(products.Count);

            for (int i = 0; i < products.Count; i++) {
                GridView.Rows[i].Cells[NameColumn.Index].Value = products[i].Name;
                GridView.Rows[i].Cells[BuyPriceColumn.Index].Value = products[i].LastBuyPrice;
                try {
                    GridView.Rows[i].Cells[IncomeColumn.Index].Value = products[i].DailyIncomePerUnit();
                } catch (MarketAvailabilityException) { }
                GridView.Rows[i].Cells[BonusColumn.Index].Value = products[i].DailyBonusPerUnit;
                GridView.Rows[i].Cells[TimeColumn.Index].Value = products[i].GrowthTime.TotalHours;
            }
            
            //double minIncome = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[IncomeColumn.Index].Value);
            //double maxIncome = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[IncomeColumn.Index].Value);
            //double minBonus = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[BonusColumn.Index].Value);
            //double maxBonus = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[BonusColumn.Index].Value);
            ////double minTime = GridView.Rows.Cast<DataGridViewRow>().Min(r => (double)r.Cells[TimeColumn.Index].Value);
            ////double maxTime = GridView.Rows.Cast<DataGridViewRow>().Max(r => (double)r.Cells[TimeColumn.Index].Value);

            //for (int i = 0; i < vegs.Count; i++) {
            //    double income = (double)GridView.Rows[i].Cells[IncomeColumn.Index].Value;
            //    GridView.Rows[i].Cells[IncomeColumn.Index].Style.BackColor = ColorScale.getColor(income, minIncome, maxIncome);
            //    double bonus = (double)GridView.Rows[i].Cells[BonusColumn.Index].Value;
            //    GridView.Rows[i].Cells[BonusColumn.Index].Style.BackColor = ColorScale.getColor(bonus, minBonus, maxBonus);
            //    //double time = (double)GridView.Rows[i].Cells[TimeColumn.Index].Value;
            //    //GridView.Rows[i].Cells[TimeColumn.Index].Style.BackColor = ColorScale.getColor(time, minTime, maxTime);
            //}
        }




        private void GridView_SelectionChanged(object sender, EventArgs e) {
            GridView.ClearSelection();
        }
    }
}
