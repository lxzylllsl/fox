namespace fox
{
    partial class Prepare
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.fq = new System.Windows.Forms.DataGridView();
            this.分区名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.fq)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "请在下面的每一行输入一个分区的名称：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(272, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "数据预处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fq
            // 
            this.fq.AllowUserToResizeColumns = false;
            this.fq.AllowUserToResizeRows = false;
            this.fq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fq.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.分区名称});
            this.fq.Location = new System.Drawing.Point(15, 53);
            this.fq.Name = "fq";
            this.fq.RowHeadersVisible = false;
            this.fq.Size = new System.Drawing.Size(342, 261);
            this.fq.TabIndex = 5;
            // 
            // 分区名称
            // 
            this.分区名称.HeaderText = "分区名称";
            this.分区名称.Name = "分区名称";
            this.分区名称.Width = 335;
            // 
            // Prepare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 349);
            this.Controls.Add(this.fq);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Prepare";
            this.Text = "Prepare";
            ((System.ComponentModel.ISupportInitialize)(this.fq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView fq;
        private System.Windows.Forms.DataGridViewTextBoxColumn 分区名称;
    }
}