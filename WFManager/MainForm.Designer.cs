namespace WFManager {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.infoLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.BotPage = new System.Windows.Forms.TabPage();
            this.ChartPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.typeComboBoxLel = new System.Windows.Forms.ComboBox();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TablePage = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.GridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyPriceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BonusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.BotPage.SuspendLayout();
            this.ChartPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.TablePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(1073, 596);
            this.webBrowser.TabIndex = 0;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Location = new System.Drawing.Point(146, 3);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(25, 13);
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "Info";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "WF Manager";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.BotPage);
            this.tabControl1.Controls.Add(this.ChartPage);
            this.tabControl1.Controls.Add(this.TablePage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1081, 622);
            this.tabControl1.TabIndex = 1;
            // 
            // BotPage
            // 
            this.BotPage.Controls.Add(this.webBrowser);
            this.BotPage.Location = new System.Drawing.Point(4, 22);
            this.BotPage.Margin = new System.Windows.Forms.Padding(0);
            this.BotPage.Name = "BotPage";
            this.BotPage.Size = new System.Drawing.Size(1073, 596);
            this.BotPage.TabIndex = 2;
            this.BotPage.Text = "Bot";
            this.BotPage.UseVisualStyleBackColor = true;
            // 
            // ChartPage
            // 
            this.ChartPage.Controls.Add(this.splitContainer1);
            this.ChartPage.Location = new System.Drawing.Point(4, 22);
            this.ChartPage.Name = "ChartPage";
            this.ChartPage.Size = new System.Drawing.Size(1073, 596);
            this.ChartPage.TabIndex = 0;
            this.ChartPage.Text = "Wykres";
            this.ChartPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.typeComboBoxLel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart);
            this.splitContainer1.Size = new System.Drawing.Size(1073, 596);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 0;
            // 
            // typeComboBoxLel
            // 
            this.typeComboBoxLel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeComboBoxLel.FormattingEnabled = true;
            this.typeComboBoxLel.Location = new System.Drawing.Point(949, 2);
            this.typeComboBoxLel.Name = "typeComboBoxLel";
            this.typeComboBoxLel.Size = new System.Drawing.Size(121, 21);
            this.typeComboBoxLel.TabIndex = 1;
            this.typeComboBoxLel.SelectedIndexChanged += new System.EventHandler(this.refreshStats);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.Red;
            chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Yellow;
            chartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Blue;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.Maroon;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.BorderColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(1073, 567);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // TablePage
            // 
            this.TablePage.Controls.Add(this.splitContainer2);
            this.TablePage.Location = new System.Drawing.Point(4, 22);
            this.TablePage.Name = "TablePage";
            this.TablePage.Size = new System.Drawing.Size(1073, 596);
            this.TablePage.TabIndex = 1;
            this.TablePage.Text = "Tabela";
            this.TablePage.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.GridView);
            this.splitContainer2.Size = new System.Drawing.Size(1073, 596);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 1;
            // 
            // GridView
            // 
            this.GridView.AllowUserToAddRows = false;
            this.GridView.AllowUserToDeleteRows = false;
            this.GridView.AllowUserToOrderColumns = true;
            this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.BuyPriceColumn,
            this.IncomeColumn,
            this.BonusColumn,
            this.TimeColumn});
            this.GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridView.Location = new System.Drawing.Point(0, 0);
            this.GridView.Name = "GridView";
            this.GridView.ReadOnly = true;
            this.GridView.RowHeadersVisible = false;
            this.GridView.Size = new System.Drawing.Size(1073, 567);
            this.GridView.TabIndex = 0;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Nazwa";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // BuyPriceColumn
            // 
            this.BuyPriceColumn.HeaderText = "Cena / szt";
            this.BuyPriceColumn.Name = "BuyPriceColumn";
            this.BuyPriceColumn.ReadOnly = true;
            // 
            // IncomeColumn
            // 
            this.IncomeColumn.HeaderText = "Zarobek / 24h";
            this.IncomeColumn.Name = "IncomeColumn";
            this.IncomeColumn.ReadOnly = true;
            this.IncomeColumn.Width = 110;
            // 
            // BonusColumn
            // 
            this.BonusColumn.HeaderText = "Punkty / 24h";
            this.BonusColumn.Name = "BonusColumn";
            this.BonusColumn.ReadOnly = true;
            // 
            // TimeColumn
            // 
            this.TimeColumn.HeaderText = "Czas";
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.ReadOnly = true;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Location = new System.Drawing.Point(950, 0);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(121, 21);
            this.categoryComboBox.TabIndex = 5;
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.refreshStats);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 622);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "WF Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.BotPage.ResumeLayout(false);
            this.ChartPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.TablePage.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage BotPage;
        private System.Windows.Forms.TabPage ChartPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox typeComboBoxLel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.TabPage TablePage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView GridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyPriceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BonusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.ComboBox categoryComboBox;
    }
}

