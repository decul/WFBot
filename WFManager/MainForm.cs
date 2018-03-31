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
        public MainForm(string[] args) {
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
            HttpServer.StartListenning();

            try {
                Logger.Label("Hello");
                WF.LogIn("owczy_farmer", "farmernia", 10);
                WF.hideAds();
            } catch (ObjectDisposedException) { }

            if (!Store.Seedlings.Any())
                WF.UpdateProductsInfo();

            Timer.Run();
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
    }
}
