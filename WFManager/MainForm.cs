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
            Store.Load(WF.storagePath);
            NpcHistory.Load(WF.storagePath);
            Timer.DeserializeEvents();
            Logger.Initialize(infoLabel);
            HttpServer.StartListenning();

            versionLabel.Text = Util.AssemblyDate.ToString();
        }

        private void startButton_Click(object sender, EventArgs e) {
            startButton.Enabled = false;

            Logger.Label("Hello");
            WF.LogIn("owczy_farmer", "farmernia", 10);
            WF.removeAds();

            stopButton.Enabled = true;

            if (!Store.Seedlings.Any())
                WF.UpdateProductsInfo();

            Timer.Run();
            Application.Exit();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            Timer.Stop();
        }
    }
}
