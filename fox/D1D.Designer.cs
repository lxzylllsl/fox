namespace fox
{
    partial class D1D
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.分区 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.数据信息 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.zjzb = new System.Windows.Forms.RadioButton();
            this.jzb = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.数据信息)).BeginInit();
            this.SuspendLayout();
            // 
            // dis
            // 
            chartArea1.Name = "ChartArea1";
            this.dis.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.dis.Legends.Add(legend1);
            this.dis.Location = new System.Drawing.Point(-3, -3);
            this.dis.Name = "dis";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.dis.Series.Add(series1);
            this.dis.Size = new System.Drawing.Size(587, 510);
            this.dis.TabIndex = 0;
            this.dis.Text = "chart1";
            // 
            // 分区
            // 
            this.分区.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.分区.FormattingEnabled = true;
            this.分区.Location = new System.Drawing.Point(653, 131);
            this.分区.Name = "分区";
            this.分区.Size = new System.Drawing.Size(121, 21);
            this.分区.TabIndex = 1;
            this.分区.SelectedIndexChanged += new System.EventHandler(this.分区_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(590, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "分区数目";
            // 
            // 数据信息
            // 
            this.数据信息.AllowUserToAddRows = false;
            this.数据信息.AllowUserToDeleteRows = false;
            this.数据信息.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.数据信息.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.数据信息.Location = new System.Drawing.Point(653, 185);
            this.数据信息.Name = "数据信息";
            this.数据信息.RowHeadersVisible = false;
            this.数据信息.Size = new System.Drawing.Size(194, 220);
            this.数据信息.TabIndex = 3;
            this.数据信息.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.数据信息_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "显示";
            this.Column1.Name = "Column1";
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "值属性";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(590, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据信息";
            // 
            // zjzb
            // 
            this.zjzb.AutoSize = true;
            this.zjzb.Checked = true;
            this.zjzb.Location = new System.Drawing.Point(593, 90);
            this.zjzb.Name = "zjzb";
            this.zjzb.Size = new System.Drawing.Size(73, 17);
            this.zjzb.TabIndex = 5;
            this.zjzb.TabStop = true;
            this.zjzb.Text = "直角坐标";
            this.zjzb.UseVisualStyleBackColor = true;
            this.zjzb.CheckedChanged += new System.EventHandler(this.zjzb_CheckedChanged);
            // 
            // jzb
            // 
            this.jzb.AutoSize = true;
            this.jzb.Location = new System.Drawing.Point(695, 90);
            this.jzb.Name = "jzb";
            this.jzb.Size = new System.Drawing.Size(61, 17);
            this.jzb.TabIndex = 6;
            this.jzb.Text = "极坐标";
            this.jzb.UseVisualStyleBackColor = true;
            this.jzb.CheckedChanged += new System.EventHandler(this.jzb_CheckedChanged);
            // 
            // D1D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 519);
            this.Controls.Add(this.jzb);
            this.Controls.Add(this.zjzb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.数据信息);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.分区);
            this.Controls.Add(this.dis);
            this.Name = "D1D";
            this.Text = "D1D";
            ((System.ComponentModel.ISupportInitialize)(this.dis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.数据信息)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart dis;
        private System.Windows.Forms.ComboBox 分区;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView 数据信息;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton zjzb;
        private System.Windows.Forms.RadioButton jzb;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;

    }
}