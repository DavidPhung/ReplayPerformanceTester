namespace PerformanceTester
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.respondTimeChart = new LiveCharts.WinForms.CartesianChart();
            this.respondTimeBgWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.responseTimeChartLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.responseTime_confidenceIntervalLabel = new System.Windows.Forms.Label();
            this.responseTime_averageLabel = new System.Windows.Forms.Label();
            this.responseTime_standardErrorLabel = new System.Windows.Forms.Label();
            this.responseTime_nValueLabel = new System.Windows.Forms.Label();
            this.responseTime_averageValueLabel = new System.Windows.Forms.Label();
            this.responseTime_standardErrorValueLabel = new System.Windows.Forms.Label();
            this.responseTime_confidenceIntervalValueLabel = new System.Windows.Forms.Label();
            this.responseTime_nLabel = new System.Windows.Forms.Label();
            this.averageMemory_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.averageMemory_confidenceIntervalLabel = new System.Windows.Forms.Label();
            this.averageMemory_averageLabel = new System.Windows.Forms.Label();
            this.averageMemory_standardErrorLabel = new System.Windows.Forms.Label();
            this.averageMemory_nValueLabel = new System.Windows.Forms.Label();
            this.averageMemory_averageValueLabel = new System.Windows.Forms.Label();
            this.averageMemory_standardErrorValueLabel = new System.Windows.Forms.Label();
            this.averageMemory_confidenceIntervalValueLabel = new System.Windows.Forms.Label();
            this.averageMemory_nLabel = new System.Windows.Forms.Label();
            this.averageMemoryChart = new LiveCharts.WinForms.CartesianChart();
            this.averageMemoryBgWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(720, 545);
            this.dataGridView1.TabIndex = 0;
            // 
            // respondTimeChart
            // 
            this.respondTimeChart.BackColor = System.Drawing.SystemColors.Window;
            this.respondTimeChart.Location = new System.Drawing.Point(217, 38);
            this.respondTimeChart.Name = "respondTimeChart";
            this.respondTimeChart.Size = new System.Drawing.Size(489, 215);
            this.respondTimeChart.TabIndex = 1;
            this.respondTimeChart.Text = "cartesianChart1";
            // 
            // respondTimeBgWorker
            // 
            this.respondTimeBgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.respondTimeBgWorker_DoWork);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.responseTimeChartLabel);
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.respondTimeChart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.averageMemory_label);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel2.Controls.Add(this.averageMemoryChart);
            this.splitContainer1.Size = new System.Drawing.Size(720, 545);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 2;
            // 
            // responseTimeChartLabel
            // 
            this.responseTimeChartLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.responseTimeChartLabel.AutoSize = true;
            this.responseTimeChartLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTimeChartLabel.Location = new System.Drawing.Point(358, 9);
            this.responseTimeChartLabel.Name = "responseTimeChartLabel";
            this.responseTimeChartLabel.Size = new System.Drawing.Size(210, 25);
            this.responseTimeChartLabel.TabIndex = 2;
            this.responseTimeChartLabel.Text = "Response Time (s)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.46018F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.53982F));
            this.tableLayoutPanel1.Controls.Add(this.responseTime_confidenceIntervalLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_averageLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_standardErrorLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_nValueLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_averageValueLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_standardErrorValueLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_confidenceIntervalValueLabel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.responseTime_nLabel, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 48);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(199, 188);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // responseTime_confidenceIntervalLabel
            // 
            this.responseTime_confidenceIntervalLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.responseTime_confidenceIntervalLabel.AutoSize = true;
            this.responseTime_confidenceIntervalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_confidenceIntervalLabel.Location = new System.Drawing.Point(3, 118);
            this.responseTime_confidenceIntervalLabel.Name = "responseTime_confidenceIntervalLabel";
            this.responseTime_confidenceIntervalLabel.Size = new System.Drawing.Size(72, 45);
            this.responseTime_confidenceIntervalLabel.TabIndex = 2;
            this.responseTime_confidenceIntervalLabel.Text = "95% Confidence Interval";
            // 
            // responseTime_averageLabel
            // 
            this.responseTime_averageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.responseTime_averageLabel.AutoSize = true;
            this.responseTime_averageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_averageLabel.Location = new System.Drawing.Point(3, 34);
            this.responseTime_averageLabel.Name = "responseTime_averageLabel";
            this.responseTime_averageLabel.Size = new System.Drawing.Size(51, 15);
            this.responseTime_averageLabel.TabIndex = 2;
            this.responseTime_averageLabel.Text = "Average";
            // 
            // responseTime_standardErrorLabel
            // 
            this.responseTime_standardErrorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.responseTime_standardErrorLabel.AutoSize = true;
            this.responseTime_standardErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_standardErrorLabel.Location = new System.Drawing.Point(3, 59);
            this.responseTime_standardErrorLabel.Name = "responseTime_standardErrorLabel";
            this.responseTime_standardErrorLabel.Size = new System.Drawing.Size(60, 30);
            this.responseTime_standardErrorLabel.TabIndex = 2;
            this.responseTime_standardErrorLabel.Text = "Standard Error";
            // 
            // responseTime_nValueLabel
            // 
            this.responseTime_nValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.responseTime_nValueLabel.AutoSize = true;
            this.responseTime_nValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_nValueLabel.Location = new System.Drawing.Point(138, 6);
            this.responseTime_nValueLabel.Name = "responseTime_nValueLabel";
            this.responseTime_nValueLabel.Size = new System.Drawing.Size(15, 15);
            this.responseTime_nValueLabel.TabIndex = 2;
            this.responseTime_nValueLabel.Text = "--";
            // 
            // responseTime_averageValueLabel
            // 
            this.responseTime_averageValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.responseTime_averageValueLabel.AutoSize = true;
            this.responseTime_averageValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_averageValueLabel.Location = new System.Drawing.Point(138, 34);
            this.responseTime_averageValueLabel.Name = "responseTime_averageValueLabel";
            this.responseTime_averageValueLabel.Size = new System.Drawing.Size(15, 15);
            this.responseTime_averageValueLabel.TabIndex = 2;
            this.responseTime_averageValueLabel.Text = "--";
            // 
            // responseTime_standardErrorValueLabel
            // 
            this.responseTime_standardErrorValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.responseTime_standardErrorValueLabel.AutoSize = true;
            this.responseTime_standardErrorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_standardErrorValueLabel.Location = new System.Drawing.Point(138, 67);
            this.responseTime_standardErrorValueLabel.Name = "responseTime_standardErrorValueLabel";
            this.responseTime_standardErrorValueLabel.Size = new System.Drawing.Size(15, 15);
            this.responseTime_standardErrorValueLabel.TabIndex = 2;
            this.responseTime_standardErrorValueLabel.Text = "--";
            // 
            // responseTime_confidenceIntervalValueLabel
            // 
            this.responseTime_confidenceIntervalValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.responseTime_confidenceIntervalValueLabel.AutoSize = true;
            this.responseTime_confidenceIntervalValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_confidenceIntervalValueLabel.Location = new System.Drawing.Point(138, 133);
            this.responseTime_confidenceIntervalValueLabel.Name = "responseTime_confidenceIntervalValueLabel";
            this.responseTime_confidenceIntervalValueLabel.Size = new System.Drawing.Size(15, 15);
            this.responseTime_confidenceIntervalValueLabel.TabIndex = 2;
            this.responseTime_confidenceIntervalValueLabel.Text = "--";
            // 
            // responseTime_nLabel
            // 
            this.responseTime_nLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.responseTime_nLabel.AutoSize = true;
            this.responseTime_nLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTime_nLabel.Location = new System.Drawing.Point(3, 6);
            this.responseTime_nLabel.Name = "responseTime_nLabel";
            this.responseTime_nLabel.Size = new System.Drawing.Size(16, 15);
            this.responseTime_nLabel.TabIndex = 2;
            this.responseTime_nLabel.Text = "N";
            // 
            // averageMemory_label
            // 
            this.averageMemory_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.averageMemory_label.AutoSize = true;
            this.averageMemory_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_label.Location = new System.Drawing.Point(344, 9);
            this.averageMemory_label.Name = "averageMemory_label";
            this.averageMemory_label.Size = new System.Drawing.Size(246, 25);
            this.averageMemory_label.TabIndex = 2;
            this.averageMemory_label.Text = "Average Memory (MB)";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.46018F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.53982F));
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_confidenceIntervalLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_averageLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_standardErrorLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_nValueLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_averageValueLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_standardErrorValueLabel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_confidenceIntervalValueLabel, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.averageMemory_nLabel, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 52);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(199, 188);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // averageMemory_confidenceIntervalLabel
            // 
            this.averageMemory_confidenceIntervalLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.averageMemory_confidenceIntervalLabel.AutoSize = true;
            this.averageMemory_confidenceIntervalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_confidenceIntervalLabel.Location = new System.Drawing.Point(3, 118);
            this.averageMemory_confidenceIntervalLabel.Name = "averageMemory_confidenceIntervalLabel";
            this.averageMemory_confidenceIntervalLabel.Size = new System.Drawing.Size(72, 45);
            this.averageMemory_confidenceIntervalLabel.TabIndex = 2;
            this.averageMemory_confidenceIntervalLabel.Text = "95% Confidence Interval";
            // 
            // averageMemory_averageLabel
            // 
            this.averageMemory_averageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.averageMemory_averageLabel.AutoSize = true;
            this.averageMemory_averageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_averageLabel.Location = new System.Drawing.Point(3, 34);
            this.averageMemory_averageLabel.Name = "averageMemory_averageLabel";
            this.averageMemory_averageLabel.Size = new System.Drawing.Size(51, 15);
            this.averageMemory_averageLabel.TabIndex = 2;
            this.averageMemory_averageLabel.Text = "Average";
            // 
            // averageMemory_standardErrorLabel
            // 
            this.averageMemory_standardErrorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.averageMemory_standardErrorLabel.AutoSize = true;
            this.averageMemory_standardErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_standardErrorLabel.Location = new System.Drawing.Point(3, 59);
            this.averageMemory_standardErrorLabel.Name = "averageMemory_standardErrorLabel";
            this.averageMemory_standardErrorLabel.Size = new System.Drawing.Size(60, 30);
            this.averageMemory_standardErrorLabel.TabIndex = 2;
            this.averageMemory_standardErrorLabel.Text = "Standard Error";
            // 
            // averageMemory_nValueLabel
            // 
            this.averageMemory_nValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.averageMemory_nValueLabel.AutoSize = true;
            this.averageMemory_nValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_nValueLabel.Location = new System.Drawing.Point(138, 6);
            this.averageMemory_nValueLabel.Name = "averageMemory_nValueLabel";
            this.averageMemory_nValueLabel.Size = new System.Drawing.Size(15, 15);
            this.averageMemory_nValueLabel.TabIndex = 2;
            this.averageMemory_nValueLabel.Text = "--";
            // 
            // averageMemory_averageValueLabel
            // 
            this.averageMemory_averageValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.averageMemory_averageValueLabel.AutoSize = true;
            this.averageMemory_averageValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_averageValueLabel.Location = new System.Drawing.Point(138, 34);
            this.averageMemory_averageValueLabel.Name = "averageMemory_averageValueLabel";
            this.averageMemory_averageValueLabel.Size = new System.Drawing.Size(15, 15);
            this.averageMemory_averageValueLabel.TabIndex = 2;
            this.averageMemory_averageValueLabel.Text = "--";
            // 
            // averageMemory_standardErrorValueLabel
            // 
            this.averageMemory_standardErrorValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.averageMemory_standardErrorValueLabel.AutoSize = true;
            this.averageMemory_standardErrorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_standardErrorValueLabel.Location = new System.Drawing.Point(138, 67);
            this.averageMemory_standardErrorValueLabel.Name = "averageMemory_standardErrorValueLabel";
            this.averageMemory_standardErrorValueLabel.Size = new System.Drawing.Size(15, 15);
            this.averageMemory_standardErrorValueLabel.TabIndex = 2;
            this.averageMemory_standardErrorValueLabel.Text = "--";
            // 
            // averageMemory_confidenceIntervalValueLabel
            // 
            this.averageMemory_confidenceIntervalValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.averageMemory_confidenceIntervalValueLabel.AutoSize = true;
            this.averageMemory_confidenceIntervalValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_confidenceIntervalValueLabel.Location = new System.Drawing.Point(138, 133);
            this.averageMemory_confidenceIntervalValueLabel.Name = "averageMemory_confidenceIntervalValueLabel";
            this.averageMemory_confidenceIntervalValueLabel.Size = new System.Drawing.Size(15, 15);
            this.averageMemory_confidenceIntervalValueLabel.TabIndex = 2;
            this.averageMemory_confidenceIntervalValueLabel.Text = "--";
            // 
            // averageMemory_nLabel
            // 
            this.averageMemory_nLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.averageMemory_nLabel.AutoSize = true;
            this.averageMemory_nLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.averageMemory_nLabel.Location = new System.Drawing.Point(3, 6);
            this.averageMemory_nLabel.Name = "averageMemory_nLabel";
            this.averageMemory_nLabel.Size = new System.Drawing.Size(16, 15);
            this.averageMemory_nLabel.TabIndex = 2;
            this.averageMemory_nLabel.Text = "N";
            // 
            // averageMemoryChart
            // 
            this.averageMemoryChart.BackColor = System.Drawing.SystemColors.Window;
            this.averageMemoryChart.Location = new System.Drawing.Point(217, 37);
            this.averageMemoryChart.Name = "averageMemoryChart";
            this.averageMemoryChart.Size = new System.Drawing.Size(489, 203);
            this.averageMemoryChart.TabIndex = 1;
            this.averageMemoryChart.Text = "cartesianChart1";
            // 
            // averageMemoryBgWorker
            // 
            this.averageMemoryBgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.averageMemoryBgWorker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 545);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private LiveCharts.WinForms.CartesianChart respondTimeChart;
        private System.ComponentModel.BackgroundWorker respondTimeBgWorker;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label responseTimeChartLabel;
        private System.Windows.Forms.Label responseTime_confidenceIntervalLabel;
        private System.Windows.Forms.Label responseTime_averageLabel;
        private System.Windows.Forms.Label responseTime_standardErrorLabel;
        private System.Windows.Forms.Label responseTime_nValueLabel;
        private System.Windows.Forms.Label responseTime_averageValueLabel;
        private System.Windows.Forms.Label responseTime_standardErrorValueLabel;
        private System.Windows.Forms.Label responseTime_confidenceIntervalValueLabel;
        private System.Windows.Forms.Label responseTime_nLabel;
        private System.Windows.Forms.Label averageMemory_label;
        private LiveCharts.WinForms.CartesianChart averageMemoryChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label averageMemory_confidenceIntervalLabel;
        private System.Windows.Forms.Label averageMemory_averageLabel;
        private System.Windows.Forms.Label averageMemory_standardErrorLabel;
        private System.Windows.Forms.Label averageMemory_nValueLabel;
        private System.Windows.Forms.Label averageMemory_averageValueLabel;
        private System.Windows.Forms.Label averageMemory_standardErrorValueLabel;
        private System.Windows.Forms.Label averageMemory_confidenceIntervalValueLabel;
        private System.Windows.Forms.Label averageMemory_nLabel;
        private System.ComponentModel.BackgroundWorker averageMemoryBgWorker;
    }
}

