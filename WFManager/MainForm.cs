using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFManager.Places;
using WFManager.ProductNS;

namespace WFManager {
    public partial class MainForm : Form {
        public MainForm() {
            RegistrySetup.SetBrowserFeatureControl();
            InitializeComponent();
            Browser.Initialize(webBrowser);
            Product.Deserialize();
            graphButton.Enabled = true;
            Timer.DeserializeEvents();
            Logger.Initialize(infoLabel);
        }

        private void startButton_Click(object sender, EventArgs e) {
            startButton.Enabled = false;

            Logger.Label("Hello");
            WF.LogIn("owczy_farmer", "farmernia", 10);
            
            if (!Product.Vegetables.Any()) 
                Product.UpdateProductsInfo();

            stopButton.Enabled = true;

            Timer.Run();
            Application.Exit();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            Timer.Stop();
        }

        private void button1_Click(object sender, EventArgs e) {
            new GraphForm().Show();
        }
    }
}
