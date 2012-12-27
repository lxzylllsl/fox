using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace fox
{
    public partial class Prepare : Form
    {
        public Prepare()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //写数据文件
            StreamWriter sw = new StreamWriter("TransFormInput.dat");
            sw.WriteLine("#百分号是有效数据行的起始标志，出现在有效数据行行首，其他任何地方不得出现百分号");
            if (fq.Rows[fq.RowCount - 1].Cells["分区名称"].Value==null)
              sw.WriteLine("%"+(fq.RowCount-1).ToString());
            else
                sw.WriteLine("%" + (fq.RowCount).ToString());
            //sw.Close();
            for (int i = 0; i < fq.RowCount-1; i++)
            {
                if (fq.Rows[i].Cells["分区名称"].Value!=null)
                    sw.WriteLine("%"+fq.Rows[i].Cells["分区名称"].Value.ToString());
            }
            sw.Close();
            this.Close();
        }
    }
}
