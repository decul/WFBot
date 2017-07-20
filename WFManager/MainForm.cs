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

namespace WFManager {
    public partial class MainForm : Form {
        public MainForm() {
            RegistrySetup.SetBrowserFeatureControl();
            InitializeComponent();

            Browser.Initialize(webBrowser);
            Store.XmlDeserialize();
            Timer.DeserializeEvents();
            Logger.Initialize(infoLabel);
            HttpServer.StartListenning();

            graphButton.Enabled = true;
            versionLabel.Text = Utility.AssemblyDate.ToString();
        }

        private void startButton_Click(object sender, EventArgs e) {
            startButton.Enabled = false;

            Logger.Label("Hello");
            WF.LogIn("owczy_farmer", "farmernia", 10);
            
            if (!Store.Vegetables.Any())
                Store.UpdateProductsInfo();

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
