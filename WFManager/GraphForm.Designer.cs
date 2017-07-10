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
            this.ExcessButton = new System.Windows.Forms.Button();
            this.DiaryButton = new System.Windows.Forms.Button();
            this.VegetableButton = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.DiaryPriceButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(946, 658);
            this.splitContainer1.SplitterDistance = 29;
            this.splitContainer1.TabIndex = 0;
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
            this.chart.Size = new System.Drawing.Size(946, 625);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
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
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 658);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GraphForm";
            this.Text = "WF Manager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Button DiaryButton;
        private System.Windows.Forms.Button VegetableButton;
        private System.Windows.Forms.Button ExcessButton;
        private System.Windows.Forms.Button DiaryPriceButton;
    }
}