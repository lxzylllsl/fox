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
    public partial class D1D : Form
    {
        int DataSize = 0;
        String[] DataList = new String[128];
        int NumZone = 0;
        String[] Zone = new String[128];
        bool needload=true;
        int node;
        Double[,] Data=new double[35000,128];
        public D1D()
        {
            InitializeComponent();
            if (needload)
                loaddata();
            分区.SelectedIndex = 0;
        }


        //读取数据的基本信息
        public void loaddata()
        {
            //第一次遍历文件 需要load所有的Zone的名称 Data的数据量
            StreamReader sr = new StreamReader("DetIntensity-1D");
            while (true)
            {
                string str = sr.ReadLine();
                if (null == str)
                    break;
                if (str.ToUpper().Contains("VARIABLES"))
                {
                    str = sr.ReadLine();
                    while (str.Contains('"') && !str.Contains('='))
                    {

                        DataSize++;
                        int be = str.IndexOf('"');
                        int en = str.IndexOf('"', be + 1);
                        DataList[DataSize] = str.Substring(be + 1, en - be - 1);

                        str = sr.ReadLine();
                    }
                }
                if (str.ToUpper().Contains("ZONE "))
                {
                    //遇到Zone的话 就把这个Zone的名称加到list里面去
                    NumZone++;
                    int be = str.IndexOf('"');
                    int en = str.IndexOf('"', be + 1);
                    Zone[NumZone] = str.Substring(be + 1, en - be - 1);
                }

            }
            sr.Close();
            数据信息.Rows.Clear();
            for (int i = 1; i <= DataSize; i++)
            {
                this.数据信息.Rows.Add(true,DataList[i]);
//                数据信息.Items.Add(DataList[i]);
            }

            分区.Items.Clear();
            for (int i = 1; i <= NumZone; i++)
            {
                分区.Items.Add(Zone[i]);
            }
            needload = false;
        }
        //读取一个area的数据
        public void loadarea()
        {
            bool flag = false;
            bool readline = false;
            StreamReader sr = new StreamReader("DetIntensity-1D");
            string str = sr.ReadLine();
            while (true)
            {
                if (null == str)
                    break;

                if (str.ToUpper().Contains("ZONE") && str.ToUpper().Contains("T="))
                {
                    int be = str.IndexOf('"');
                    int en = str.IndexOf('"', be + 1);
                    if (分区.SelectedItem.ToString() == str.Substring(be + 1, en - be - 1))
                        flag = true;
                    else
                        flag = false;
                }

                //这里是每一个块的基本属性 没问题了
                #region
                if ((flag) && str.ToUpper().Contains("I="))
                {
                    node = int.Parse(str.Substring(str.ToUpper().IndexOf('=') + 1));

                }
                #endregion

                if ((flag) && str.ToUpper().Contains("DT="))
                {
                    readline = true;
                    str = sr.ReadLine();
                }
                //这里才开始正式的读取数据
                if ((flag) && (readline))
                {
                    //处理点数据
                    for (int i = 1; i <= node; i++)
                    {
                        string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 1; j <= DataSize; j++)
                        {
                            Data[i, j] = double.Parse(arraystr[j-1]);
                        }
                        str = sr.ReadLine();     
                    }
                    readline = false;
                    flag = false;
                }
                str = sr.ReadLine();
            }
            sr.Close();
        }
        private void 分区_SelectedIndexChanged(object sender, EventArgs e)
        {
            //读取一个分区的数据
            loadarea();
            drawfig();
        }
        public void drawfig()
        {
            if (zjzb.Checked)
            {
                dis.Series.Clear();
                for (int j = 1; j <= DataSize; j++)
                {
                    if (数据信息.Rows[j - 1].Cells[0].Value.ToString() == "True")
                    {
                        var serial1 = dis.Series.Add(DataList[j]);
                        for (int i = 1; i <= node; i++)
                        {
                            serial1.Points.AddXY(i, Data[i, j]);
                        }
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType ct = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        serial1.ChartType = ct;
                    }
                }   
            }
            if (jzb.Checked)
            {
                dis.Series.Clear();
                for (int j = 2; j <= DataSize; j++)
                {
                    if (数据信息.Rows[j - 1].Cells[0].Value.ToString() == "True")
                    {
                        var serial1 = dis.Series.Add(DataList[j]);
                        for (int i = 1; i <= node; i++)
                        {
                            serial1.Points.AddXY(Data[i, 1], Data[i, j]);
                        }
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType ct = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
                        serial1.ChartType = ct;
                    }
                }   
            }
        }

        private void 数据信息_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            drawfig();
        }

        private void jzb_CheckedChanged(object sender, EventArgs e)
        {
            drawfig();
        }

        private void zjzb_CheckedChanged(object sender, EventArgs e)
        {
            drawfig();
        }

    
  

    }
}
