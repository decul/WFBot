namespace WFManager {
    partial class GraphForm {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DiaryPriceButton = new System.Windows.Forms.Button();
            this.ExcessButton = new System.Windows.Forms.Button();
            this.DiaryButton = new System.Windows.Forms.Button();
            this.VegetableButton = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.DiariesButton = new System.Windows.Forms.Button();
            this.VegButton = new System.Windows.Forms.Button();
            this.GridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BonusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DiaryPriceButton);
            this.splitContainer1.Panel1.Controls.Add(this.ExcessButton);
            this.splitContainer1.Panel1.Controls.Add(this.DiaryButton);
            this.splitContainer1.Panel1.Controls.Add(this.VegetableButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart);
            this.splitContainer1.Size = new System.Drawing.Size(932, 626);
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.TabIndex = 0;
            // 
            // DiaryPriceButton
            // 
            this.DiaryPriceButton.Location = new System.Drawing.Point(267, 3);
            this.DiaryPriceButton.Name = "DiaryPriceButton";
            this.DiaryPriceButton.Size = new System.Drawing.Size(78, 23);
            this.DiaryPriceButton.TabIndex = 3;
            this.DiaryPriceButton.Text = "Zwierz. cena";
            this.DiaryPriceButton.UseVisualStyleBackColor = true;
            this.DiaryPriceButton.Click += new System.EventHandler(this.DiaryPriceButton1_Click);
            // 
            // ExcessButton
            // 
            this.ExcessButton.Location = new System.Drawing.Point(84, 3);
            this.ExcessButton.Name = "ExcessButton";
            this.ExcessButton.Size = new System.Drawing.Size(75, 23);
            this.ExcessButton.TabIndex = 2;
            this.ExcessButton.Text = "Przebicie";
            this.ExcessButton.UseVisualStyleBackColor = true;
            this.ExcessButton.Click += new System.EventHandler(this.ExcessButton_Click);
            // 
            // DiaryButton
            // 
            this.DiaryButton.Location = new System.Drawing.Point(186, 3);
            this.DiaryButton.Name = "DiaryButton";
            this.DiaryButton.Size = new System.Drawing.Size(75, 23);
            this.DiaryButton.TabIndex = 1;
            this.DiaryButton.Text = "Zwierzęce";
            this.DiaryButton.UseVisualStyleBackColor = true;
            this.DiaryButton.Click += new System.EventHandler(this.DiaryButton_Click);
            // 
            // VegetableButton
            // 
            this.VegetableButton.Location = new System.Drawing.Point(3, 3);
            this.VegetableButton.Name = "VegetableButton";
            this.VegetableButton.Size = new System.Drawing.Size(75, 23);
            this.VegetableButton.TabIndex = 0;
            this.VegetableButton.Text = "Warzywa";
            this.VegetableButton.UseVisualStyleBackColor = true;
            this.VegetableButton.Click += new System.EventHandler(this.VegetableButton_Click);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.ScrollBar.BackColor = System.Drawing.Color.Red;
            chartArea2.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Yellow;
            chartArea2.AxisX.ScrollBar.LineColor = System.Drawing.Color.Blue;
            chartArea2.AxisX.TitleForeColor = System.Drawing.Color.Maroon;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.BorderColor = System.Drawing.Color.White;
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(932, 595);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(946, 658);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(938, 632);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Wykres";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(938, 632);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tabela";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.DiariesButton);
            this.splitContainer2.Panel1.Controls.Add(this.VegButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.GridView);
            this.splitContainer2.Size = new System.Drawing.Size(932, 626);
            this.splitContainer2.SplitterDistance = 26;
            this.splitContainer2.TabIndex = 1;
            // 
            // DiariesButton
            // 
            this.DiariesButton.Location = new System.Drawing.Point(84, 3);
            this.DiariesButton.Name = "DiariesButton";
            this.DiariesButton.Size = new System.Drawing.Size(75, 23);
            this.DiariesButton.TabIndex = 1;
            this.DiariesButton.Text = "Zwierzęce";
            this.DiariesButton.UseVisualStyleBackColor = true;
            // 
            // VegButton
            // 
            this.VegButton.Location = new System.Drawing.Point(3, 3);
            this.VegButton.Name = "VegButton";
            this.VegButton.Size = new System.Drawing.Size(75, 23);
            this.VegButton.TabIndex = 0;
            this.VegButton.Text = "Warzywa";
            this.VegButton.UseVisualStyleBackColor = true;
            this.VegButton.Click += new System.EventHandler(this.VegButton_Click);
            // 
            // GridView
            // 
            this.GridView.AllowUserToAddRows = false;
            this.GridView.AllowUserToDeleteRows = false;
            this.GridView.AllowUserToOrderColumns = true;
            this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.IncomeColumn,
            this.BonusColumn,
            this.TimeColumn});
            this.GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridView.Location = new System.Drawing.Point(0, 0);
            this.GridView.Name = "GridView";
            this.GridView.ReadOnly = true;
            this.GridView.RowHeadersVisible = false;
            this.GridView.Size = new System.Drawing.Size(932, 596);
            this.GridView.TabIndex = 0;
            this.GridView.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Nazwa";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // IncomeColumn
            // 
            this.IncomeColumn.HeaderText = "Zarobek / 24h";
            this.IncomeColumn.Name = "IncomeColumn";
            this.IncomeColumn.ReadOnly = true;
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
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 658);
            this.Controls.Add(this.tabControl1);
            this.Name = "GraphForm";
            this.Text = "WF Manager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Button DiaryButton;
        private System.Windows.Forms.Button VegetableButton;
        private System.Windows.Forms.Button ExcessButton;
        private System.Windows.Forms.Button DiaryPriceButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button DiariesButton;
        private System.Windows.Forms.Button VegButton;
        private System.Windows.Forms.DataGridView GridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BonusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
    }
}