namespace fox
{
    partial class D3D
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
            this.info = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.数据信息 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.分区 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.js = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.max = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.min = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.openGLControl1 = new SharpGL.OpenGLControl();
            this.ColorNum = new System.Windows.Forms.TrackBar();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.label2 = new System.Windows.Forms.Label();
            this.网格 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.SuspendLayout();
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(729, 214);
            this.info.Multiline = true;
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(220, 163);
            this.info.TabIndex = 54;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.数据信息);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.分区);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(729, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 166);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "部件信息";
            // 
            // 数据信息
            // 
            this.数据信息.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.数据信息.FormattingEnabled = true;
            this.数据信息.Location = new System.Drawing.Point(67, 60);
            this.数据信息.Name = "数据信息";
            this.数据信息.Size = new System.Drawing.Size(100, 21);
            this.数据信息.TabIndex = 27;
            this.数据信息.SelectedIndexChanged += new System.EventHandler(this.数据信息_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "数据信息";
            // 
            // 分区
            // 
            this.分区.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.分区.FormattingEnabled = true;
            this.分区.Location = new System.Drawing.Point(69, 33);
            this.分区.Name = "分区";
            this.分区.Size = new System.Drawing.Size(98, 21);
            this.分区.TabIndex = 25;
            this.分区.SelectedIndexChanged += new System.EventHandler(this.分区_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "分区";
            // 
            // js
            // 
            this.js.Location = new System.Drawing.Point(673, 395);
            this.js.Name = "js";
            this.js.ReadOnly = true;
            this.js.Size = new System.Drawing.Size(44, 20);
            this.js.TabIndex = 52;
            this.js.Text = "30";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(347, 412);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(47, 23);
            this.button4.TabIndex = 49;
            this.button4.Text = "Set";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // max
            // 
            this.max.Location = new System.Drawing.Point(229, 414);
            this.max.Name = "max";
            this.max.Size = new System.Drawing.Size(100, 20);
            this.max.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(196, 417);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 47;
            this.label7.Text = "Max";
            // 
            // min
            // 
            this.min.Location = new System.Drawing.Point(85, 414);
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(100, 20);
            this.min.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-69, 458);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 45;
            this.label6.Text = "Min";
            // 
            // openGLControl1
            // 
            this.openGLControl1.BitDepth = 24;
            this.openGLControl1.DrawFPS = false;
            this.openGLControl1.FrameRate = 20;
            this.openGLControl1.Location = new System.Drawing.Point(-76, 50);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl1.Size = new System.Drawing.Size(70, 73);
            this.openGLControl1.TabIndex = 44;
            // 
            // ColorNum
            // 
            this.ColorNum.Location = new System.Drawing.Point(563, 395);
            this.ColorNum.Maximum = 150;
            this.ColorNum.Minimum = 10;
            this.ColorNum.Name = "ColorNum";
            this.ColorNum.Size = new System.Drawing.Size(104, 45);
            this.ColorNum.TabIndex = 43;
            this.ColorNum.TickFrequency = 20;
            this.ColorNum.Value = 30;
            this.ColorNum.Scroll += new System.EventHandler(this.ColorNum_Scroll);
            // 
            // openGLControl
            // 
            this.openGLControl.BitDepth = 24;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.FrameRate = 20;
            this.openGLControl.Location = new System.Drawing.Point(-2, 0);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl.Size = new System.Drawing.Size(719, 377);
            this.openGLControl.TabIndex = 42;
            this.openGLControl.OpenGLDraw += new System.Windows.Forms.PaintEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl_KeyDown);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(426, 417);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "显示网格";
            // 
            // 网格
            // 
            this.网格.AutoSize = true;
            this.网格.Location = new System.Drawing.Point(487, 417);
            this.网格.Name = "网格";
            this.网格.Size = new System.Drawing.Size(15, 14);
            this.网格.TabIndex = 50;
            this.网格.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Min";
            // 
            // D3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 447);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.info);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.js);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.max);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.min);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.openGLControl1);
            this.Controls.Add(this.ColorNum);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.网格);
            this.Name = "D3D";
            this.Text = "D3D";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox info;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox 数据信息;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 分区;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox js;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox max;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox min;
        private System.Windows.Forms.Label label6;
        private SharpGL.OpenGLControl openGLControl1;
        private System.Windows.Forms.TrackBar ColorNum;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox 网格;
        private System.Windows.Forms.Label label4;
    }
}