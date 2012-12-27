using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.IO;
using System.Diagnostics;

namespace fox
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();
            wgdw.SelectedIndex = 0;
            dqms.SelectedIndex = 0;
            qrj.SelectedIndex = 0;
            jy.SelectedIndex = 0;
            hjhf.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        //公共参数设置
        #region
        private Double[,] Data = new double[35000, 128];//前三个是坐标 后面的是数据
        private Int32[,] E = new int[35000, 5];//构成一个面的点有哪些
        private int NumZone = 0;//一共有多少个Zone
        private String[] Zone = new string[128];//每个Zone的名字是神马
        private String[] DataList = new string[128];//每一个具体的变量是什么
        int node = 0, element = 0, DataSize = 0;//节点 面 每个面有多少个类数据
        string ZoneType, DataType;
        int D123;//是几维图像
        Int32[] CentList = new int[128];//都是哪些数据在中心
        SharpGL.OpenGL gl = new OpenGL();//opengl空间
        int anX = 0;//X方向旋转角度
        int anY = 0;//Y方向旋转角度
        int anZ = 0;//Z方向旋转角度
        string fName;//打开的那个要读取的文件
        float SF = 1;//控制缩放
        double MoveX = 0, MoveY = 0;//控制移动
        Double[] ColorR = new double[256];//用三种颜色渐变来做colorbar
        Double[] ColorB = new double[256];
        Double[] ColorG = new double[256];
        Double[] ColorLim = new double[256];//每一个颜色对应的值
        bool face = false;//是不是可以画具体的云图了
        bool needfresh = true;//是不是需要重新计算
        Int32[] EleColor = new int[35000];//用来存储每一个elements的颜色
        short fxxs = 1;//是不是要给所有的Vector*-1
        double sightx = 5, sighty = 5, sightz = 5, upy = 1, upz = 0;//视线所在位置
        Prepare preare = null;
        bool started = false;//这里是判断那个Trans程序有没有被运行过
        string path;
        string folderName;
        #endregion
        //这里是具体画每一个小三角形和四边形的 
        #region
        //构建三角形的函数
        private void triangle(OpenGL gl, double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3)
        {
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0f, 0f, 0f);
            gl.Vertex(x1, y1, z1);
            gl.Vertex(x2, y2, z2);
            gl.Vertex(x3, y3, z3);
            gl.End();
        }
        //构建四边形的函数
        private void quadrangle(OpenGL gl, double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3, double x4, double y4, double z4)
        {
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0f, 0f, 0f);
            gl.Vertex(x1, y1, z1);
            gl.Vertex(x2, y2, z2);
            gl.Vertex(x3, y3, z3);
            gl.Vertex(x4, y4, z4);
            gl.End();
        }
        //上色三角形
        private void triangle(OpenGL gl, double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3, int n)
        {
            gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(ColorR[n], ColorG[n], ColorB[n]);
            gl.Vertex(x1, y1, z1);
            gl.Vertex(x2, y2, z2);
            gl.Vertex(x3, y3, z3);
            gl.End();


        }
        //上色四边形
        private void quadrangle(OpenGL gl, double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3, double x4, double y4, double z4, int n)
        {
            gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(ColorR[n], ColorG[n], ColorB[n]);
            gl.Vertex(x1, y1, z1);
            gl.Vertex(x2, y2, z2);
            gl.Vertex(x3, y3, z3);
            gl.Vertex(x4, y4, z4);
            gl.End();
            //  info.AppendText(n.ToString() + "\r\n");

        }
        //以下是二维平面的处理
        private void triangle(OpenGL gl, double x1, double y1, double x2, double y2, double x3, double y3)
        {
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0f, 0f, 0f);
            gl.Vertex(x1, y1, 0);
            gl.Vertex(x2, y2, 0);
            gl.Vertex(x3, y3, 0);
            gl.End();
        }
        private void quadrangle(OpenGL gl, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0f, 0f, 0f);
            gl.Vertex(x1, y1, 0);
            gl.Vertex(x2, y2, 0);
            gl.Vertex(x3, y3, 0);
            gl.Vertex(x4, y4, 0);
            gl.End();
        }
        private void triangle(OpenGL gl, double x1, double y1, double x2, double y2, double x3, double y3, int n)
        {
            gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(ColorR[n], ColorG[n], ColorB[n]);
            gl.Vertex(x1, y1, 0);
            gl.Vertex(x2, y2, 0);
            gl.Vertex(x3, y3, 0);
            gl.End();
        }
        private void quadrangle(OpenGL gl, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, int n)
        {
            gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(ColorR[n], ColorG[n], ColorB[n]);
            gl.Vertex(x1, y1, 0);
            gl.Vertex(x2, y2, 0);
            gl.Vertex(x3, y3, 0);
            gl.Vertex(x4, y4, 0);
            gl.End();
        }
        #endregion

        //主画图程序
        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);
            //清除深度缓存 
            gl.LoadIdentity();
            //重置模型观察矩阵，我认为其实就是重置了三维坐标轴的位置，将其初始化为原点 

            //gl.Rotate(anY, 0.0f, 0.0f, 1.0f);

            gl.LookAt(sightx, sighty, sightz, 0.0, 0.0, 0.0, 0.0, upy, upz);
            gl.Scale(SF, SF, SF);

            gl.Translate(MoveX / SF, 0f, MoveY / SF);
            //triangle(gl, 0, 1, 0, -1, -1, 0, 1, -1, 0);

            gl.Rotate(anX, 1f, 0f, 0f);
            gl.Rotate(anY, 0f, 0f, 1f);
            gl.Rotate(anZ, 0f, 1f, 0f);

            gl.Begin(OpenGL.GL_LINES);
            gl.Color(255, 0, 0);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(50 / SF, 0, 0.0);
            gl.End();

            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0, 255, 0);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(0, 50 / SF, 0.0);
            gl.End();


            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0, 0, 255);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(0, 0.0, 50 / SF);
            gl.End();

            //如果需要显示网格 处理每一个网格 

            //在这里具体画图
            #region
            if (D123 == 3)
            {
                if (face)
                {
                    //不需要区分数据在哪里
                    if ("T" == ZoneType)
                    {
                        for (int i = 1; i <= element; i++)
                            triangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 1], 3], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 2], 3], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 3], 3], EleColor[i]);
                    }
                    if ("Q" == ZoneType)
                    {
                        for (int i = 1; i <= element; i++)
                            quadrangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 1], 3], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 2], 3], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 3], 3], Data[E[i, 4], 1], Data[E[i, 4], 2], Data[E[i, 4], 3], EleColor[i]);
                    }
                }

                #region
                if (网格.Checked)
                {
                    if ("T" == ZoneType)
                        for (int i = 1; i <= element; i++)
                            triangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 1], 3], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 2], 3], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 3], 3]);

                    if ("Q" == ZoneType)
                        for (int i = 1; i <= element; i++)
                            quadrangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 1], 3], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 2], 3], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 3], 3], Data[E[i, 4], 1], Data[E[i, 4], 2], Data[E[i, 4], 3]);
                }
                #endregion
            }
            if (D123 == 2)
            {
                if (face)
                {
                    //不需要区分数据在哪里

                    if ("T" == ZoneType)
                    {
                        for (int i = 1; i <= element; i++)
                            triangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 3], 1], Data[E[i, 3], 2], EleColor[i]);
                    }
                    if ("Q" == ZoneType)
                    {
                        for (int i = 1; i <= element; i++)
                            quadrangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 4], 1], Data[E[i, 4], 2], EleColor[i]);
                    }
                }

                #region
                if (网格.Checked)
                {
                    if ("T" == ZoneType)
                        for (int i = 1; i <= element; i++)
                            triangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 3], 1], Data[E[i, 3], 2]);
                    if ("Q" == ZoneType)
                        for (int i = 1; i <= element; i++)
                            quadrangle(gl, Data[E[i, 1], 1], Data[E[i, 1], 2], Data[E[i, 2], 1], Data[E[i, 2], 2], Data[E[i, 3], 1], Data[E[i, 3], 2], Data[E[i, 4], 1], Data[E[i, 4], 2]);
                }
                #endregion
            }
            #endregion
            gl.Flush();
        }

        //以下是根据一个颜色值，来确定应该在处于那个Color Bar 如果数据溢出 用最大值 如果数据特别小 用最小值
        private int GetColorIndex(double value)
        {
            //如果不止一个值
            if ((ColorLim[ColorNum.Value] - ColorLim[1]) > 0)
            {
                int num = (int)((double)((value - ColorLim[1]) / (ColorLim[ColorNum.Value] - ColorLim[1])) * ColorNum.Value);
                if (num <= ColorNum.Value && num > 0)
                    return num;
                else if (num <= 0)
                    return 1;
                else
                    return ColorNum.Value;
            }
            else
                return 1;
        }

        //以下region是处理拖动的代码
        #region
        private Point mouseOffset;
        private bool isMouseDown = false;

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                // detaX.Text = (mousePos.Y - mouseOffset.Y).ToString();
                // translate(this.gl,mousePos.X - mouseOffset.X, mousePos.Y - mouseOffset.Y);
                anX += mousePos.X - mouseOffset.X;
                anY += mousePos.Y - mouseOffset.Y;
                mouseOffset = mousePos;

            }
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mouseOffset = mousePos;
                isMouseDown = true;
            }

        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }
        #endregion

        //处理打开一个数据文件
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "cgns文件|*.cgns|文本文件|*.txt|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = folderName;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fName = openFileDialog1.FileName;
                path = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                //拷贝到计算目录
                File.Copy(fName, System.IO.Path.GetFileName(fName), true);
                //拷贝到工程目录
                if (fName!=folderName + @"\" + System.IO.Path.GetFileName(fName))
                File.Copy(fName, folderName + @"\" + System.IO.Path.GetFileName(fName), true);
                started = false;
                preare = new Prepare();
                preare.Show();
                info.AppendText("Loading input files: " + fName + "\r\n");
                timer3.Start();
            }
        }

        //这里是判断数据读入时的转换是否完成的Timer
        private void timer3_Tick(object sender, EventArgs e)
        {
            bool has = false;
            if ((preare == null || preare.IsDisposed) && (started == false))
            {
                StreamWriter sw = new StreamWriter("TransFormInput.dat", true);
                sw.WriteLine('%' + System.IO.Path.GetFileName(fName));
                sw.WriteLine('%' + wgdw.SelectedItem.ToString());
                sw.Close();
                Process.Start("Transform.exe");
                started = true;
            }
            else
            {
                foreach (Process thisproc in System.Diagnostics.Process.GetProcesses())
                {
                    if (thisproc.ProcessName.Contains("Transform"))
                    {
                        has = true;
                    }
                }
                if ((has == false) && (started))
                {
                    info.AppendText("Finish Transform The Data \r\n");
                    timer3.Stop();
                    loadreal();
                    分区.SelectedIndex = 0;
                    数据信息.SelectedIndex = 0;
                    File.Copy("Vector-Tec360.dat", folderName + @"\Vector-Tec360.dat", true);
                }
            }
        }

        public void loadreal()
        {
            fName = "Vector-Tec360.dat";
            loadAll();
            发射率.Enabled = true;
            温度.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button7.Enabled = true;
            wdsz.Enabled = true;
            button9.Enabled = true;
        }

        //这是第一次读取数据 计算zone的数目
        private void loadAll()
        {
            D123 = 1;
            DataSize = 0;
            NumZone = 0;
            //第一次遍历文件 需要load所有的Zone的名称 Data的数据量
            info.AppendText("Analysis Zone info \r\n");
            StreamReader sr = new StreamReader(fName);
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
                        if (str.ToUpper().Contains("COORDINATE"))
                        {
                            if (str.ToUpper().Contains("COORDINATEZ"))
                                D123 = 3;
                            if (3 != D123 && str.ToUpper().Contains("COORDINATEY"))
                                D123 = 2;
                        }
                        else
                        {
                            DataSize++;
                            int be = str.IndexOf('"');
                            int en = str.IndexOf('"', be + 1);
                            DataList[DataSize] = str.Substring(be + 1, en - be - 1);
                        }
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
            数据信息.Items.Clear();
            for (int i = 1; i <= DataSize; i++)
            {
                数据信息.Items.Add(DataList[i]);
            }

            分区.Items.Clear();
            for (int i = 1; i <= NumZone; i++)
            {
                分区.Items.Add(Zone[i]);
            }
            info.AppendText("Data is: " + D123 + " graph \r\n");
            info.AppendText(DataSize.ToString() + " DataSize total \r\n");
            info.AppendText(NumZone.ToString() + " Zones total \r\n");
        }

        //选定一个Zone后 重新读取数据
        private void 分区_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i <= 127; i++)
                CentList[i] = 0;
            bool flag = false;
            bool readline = false;
            bool readface = false;//这个是true的时候 就可以开始读取节点构成面的信息了
            long count = 1;//统计读了每一个属性多少个值了
            int point = 0;//统计读了多少个属性了
            StreamReader sr = new StreamReader(fName);
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
                if ((flag) && str.ToUpper().Contains("NODES"))
                {
                    node = int.Parse(str.Substring(str.ToUpper().IndexOf('=') + 1));
                    info.AppendText("Number of Nodes is: " + node.ToString() + "\r\n");
                }
                if ((flag) && str.ToUpper().Contains("ELEMENTS"))
                {
                    element = int.Parse(str.Substring(str.ToUpper().IndexOf('=') + 1));
                    info.AppendText("Number of Elements is: " + element.ToString() + "\r\n");
                }
                if ((flag) && str.ToUpper().Contains("FETRIANGLE") && str.ToUpper().Contains("ZONETYPE"))
                {
                    ZoneType = "T";
                    info.AppendText("Style of ZoneType is: FETriangle \r\n");
                }
                if ((flag) && str.ToUpper().Contains("FEQUADRILATERAL") && str.ToUpper().Contains("ZONETYPE"))
                {
                    ZoneType = "Q";
                    info.AppendText("Style of ZoneType is: FEQuadrilateral \r\n");
                }
                if ((flag) && str.ToUpper().Contains("DATAPACKING") && str.ToUpper().Contains("BLOCK"))
                {
                    DataType = "B";
                    info.AppendText("Style of DataType is: Block \r\n");
                }
                if ((flag) && str.ToUpper().Contains("DATAPACKING") && str.ToUpper().Contains("POINT"))
                {
                    DataType = "P";
                    info.AppendText("Style of ZoneType is: Point \r\n");
                }
                //在这里处理那些中心点！
                if ((flag) && str.ToUpper().Contains("VARLOCATION") && str.ToUpper().Contains("CELLCENTERED"))
                {
                    int be = str.IndexOf("[");
                    int en = str.IndexOf("]");
                    string[] arraystr = str.Substring(be + 1, en - be - 1).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in arraystr)
                    {
                        CentList[int.Parse(s)] = 1;
                    }
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
                    //区分多维数据 Block与point数据
                    if (DataType == "B")
                    {
                        //3D的Block数据
                        string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in arraystr)
                        {
                            point++;
                            if (0 == CentList[count])
                                if (point > node)
                                {
                                    point = 1;
                                    count++;
                                    //如果已经不是数据了就不要读取了 开始创建面
                                    if (count > D123 + DataSize)
                                    {
                                        readline = false;
                                        readface = true;

                                    }
                                }
                            if (1 == CentList[count])
                                if (point > element)
                                {
                                    point = 1;
                                    count++;
                                    //如果已经不是数据了就不要读取了 开始创建面
                                    if (count > D123 + DataSize)
                                    {
                                        readline = false;
                                        readface = true;

                                    }
                                }
                            Data[point, count] = double.Parse(s);
                        }
                    }
                    if (DataType == "P")
                    {
                        //处理点数据
                        for (int i = 1; i <= node; i++)
                        {
                            string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 1; j <= DataSize; j++)
                            {
                                Data[i, j] = double.Parse(arraystr[j - 1]);
                            }
                            str = sr.ReadLine();
                        }
                    }
                }

                if (readface)
                {
                    int en = 0;
                    if ("T" == ZoneType)
                        en = 3;
                    if ("Q" == ZoneType)
                        en = 4;

                    for (int i = 1; i <= element; i++)
                    {
                        if (i > 1)
                            str = sr.ReadLine();
                        string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 1; j <= en; j++)
                            E[i, j] = int.Parse(arraystr[j - 1]);
                    }
                    info.AppendText("Load Data success \r\n");

                    //如果数据信息里面依旧有东西 那么还是需要更新的
                    if (数据信息.SelectedItem != null)
                    {
                        needfresh = true;
                        face = false;
                        info.AppendText("Analysing the Data...\r\n");
                        //如果是面数据
                        if (CentList[D123 + 数据信息.SelectedIndex + 1] == 0)
                        {
                            for (int i = 1; i <= node; i++)
                                arrlist[i] = i;
                            quickSort(arrlist, 1, node, D123 + 数据信息.SelectedIndex + 1);
                            info.AppendText("Min Data: " + Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1].ToString() + " Max Data: " + Data[arrlist[node], D123 + 数据信息.SelectedIndex + 1].ToString() + "\r\n");
                            if (needfresh) MixColor(ColorNum.Value, node);
                        }
                        //如果是点数据
                        if (CentList[D123 + 数据信息.SelectedIndex + 1] == 1)
                        {
                            for (int i = 1; i <= element; i++)
                                arrlist[i] = i;
                            quickSort(arrlist, 1, element, D123 + 数据信息.SelectedIndex + 1);
                            info.AppendText("Min Data: " + Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1].ToString() + " Max Data: " + Data[arrlist[element], D123 + 数据信息.SelectedIndex + 1].ToString() + "\r\n");
                            if (needfresh) MixColor(ColorNum.Value, element);
                        }
                        CalcEleCol();
                        face = true;
                    }
                    break;
                }
                str = sr.ReadLine();
            }
            sr.Close();
            数据信息.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((数据信息.SelectedItem.ToString().ToUpper().Contains("EMITTANCE")) && (isNumberic(发射率.Text)))
            {
                try
                {
                    //处理数据如果在中心的情况
                    if (CentList[数据信息.SelectedIndex + D123 + 1] == 1)
                    {
                        for (int i = 1; i <= element; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] = Double.Parse(发射率.Text);
                    }
                    //处理在节点上面的数据
                    else
                    {
                        for (int i = 1; i <= node; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] = Double.Parse(发射率.Text);
                    }
                    //然后去重新根据这个来做数据
                    CalcEleCol();
                    info.AppendText("Emittance change successful!\r\n");
                }
                catch
                {

                }
            }
        }

        private void openGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if ("-" == e.KeyCode.ToString())
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.D)
                MoveX += 1;
            if (e.KeyCode == Keys.A)
                MoveX -= 1;
            if (e.KeyCode == Keys.W)
                MoveY += 1;
            if (e.KeyCode == Keys.S)
                MoveY -= 1;
            //缩小
            if (e.KeyCode == Keys.Space)
                SF *= 0.75f;
            //放大
            if (e.KeyCode == Keys.Z)
                SF /= 0.75f;
            //X轴旋转
            if (e.KeyCode == Keys.I)
                anY += 5;
            if (e.KeyCode == Keys.K)
                anY -= 5;
            if (e.KeyCode == Keys.J)
                anX += 5;
            if (e.KeyCode == Keys.L)
                anX -= 5;
            if (e.KeyCode == Keys.U)
                anZ += 5;
            if (e.KeyCode == Keys.P)
                anZ -= 5;

            if (e.KeyCode == Keys.D1)
            {
                sightx = 5;
                sighty = 0;
                sightz = 0;
                upy = 1;
                upz = 0;
            }
            if (e.KeyCode == Keys.D2)
            {
                sightx = -5;
                sighty = 0;
                sightz = 0;
                upy = 1;
                upz = 0;
            }
            if (e.KeyCode == Keys.D3)
            {
                sightx = 0;
                sighty = 5;
                sightz = 0;
                upy = 0;
                upz = 1;
            }
            if (e.KeyCode == Keys.D4)
            {
                sightx = 0;
                sighty = -5;
                sightz = 0;
                upy = 0;
                upz = 1;
            }
            if (e.KeyCode == Keys.D5)
            {
                sightx = 0;
                sighty = 0;
                sightz = 5;
                upy = 1;
                upz = 0;
            }
            if (e.KeyCode == Keys.D6)
            {
                sightx = 0;
                sighty = 0;
                sightz = -5;
                upy = 1;
                upz = 0;
            }
            if (e.KeyCode == Keys.D7)
            {
                sightx = 5;
                sighty = 5;
                sightz = 5;
                upy = 1;
                upz = 0;
            }



        }

        Int32[] arrlist = new int[35000];

        //这里的完成着色工作 首先生成色带，统计数据的频数
        private void 数据信息_SelectedIndexChanged(object sender, EventArgs e)
        {
            needfresh = true;
            face = false;
            info.AppendText("Analysing the Data...\r\n");
            //如果是面数据
            if (CentList[D123 + 数据信息.SelectedIndex + 1] == 0)
            {
                for (int i = 1; i <= node; i++)
                    arrlist[i] = i;
                quickSort(arrlist, 1, node, D123 + 数据信息.SelectedIndex + 1);
                info.AppendText("Min Data: " + Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1].ToString() + " Max Data: " + Data[arrlist[node], D123 + 数据信息.SelectedIndex + 1].ToString() + "\r\n");
                if (needfresh) MixColor(ColorNum.Value, node);

            }
            //如果是点数据
            if (CentList[D123 + 数据信息.SelectedIndex + 1] == 1)
            {
                for (int i = 1; i <= element; i++)
                    arrlist[i] = i;
                quickSort(arrlist, 1, element, D123 + 数据信息.SelectedIndex + 1);
                info.AppendText("Min Data: " + Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1].ToString() + " Max Data: " + Data[arrlist[element], D123 + 数据信息.SelectedIndex + 1].ToString() + "\r\n");
                if (needfresh) MixColor(ColorNum.Value, element);
            }
            CalcEleCol();
            face = true;
        }

        //分别是排序好的数组，开始位置，结束位置，对Data的那一列进行排序。
        #region
        public void quickSort(int[] arrays, int low, int high, int DataCou)
        {
            // 枢纽元，一般以第一个元素为基准进行划分 
            int i = low;
            int j = high;
            if (low < high)
            {
                // 从数组两端交替地向中间扫描 
                double key = Data[arrays[low], DataCou];
                // 进行扫描的指针i,j;i从左边开始，j从右边开始 
                while (i < j)
                {
                    while (i < j && Data[arrays[j], DataCou] > key)
                    {
                        j--;
                    }// end while 
                    if (i < j)
                    {
                        // 比枢纽元素小的移动到左边 
                        arrays[i] = arrays[j];
                        i++;
                    }// end if 
                    while (i < j && Data[arrays[i], DataCou] < key)
                    {
                        i++;
                    }// end while 
                    if (i < j)
                    {
                        // 比枢纽元素大的移动到右边 
                        arrays[j] = arrays[i];
                        j--;
                    }// end if 
                }// end while 
                // 枢纽元素移动到正确位置 
                arrays[i] = arrays[low];
                // 前半个子表递归排序 
                quickSort(arrays, low, i - 1, DataCou);
                // 后半个子表递归排序 
                quickSort(arrays, i + 1, high, DataCou);
            }// end if  
        }

        #endregion

        //这里是创建了一个Color Bar n个颜色 num个数据
        private void MixColor(int n, int num)
        {
            for (int i = 2; i <= n - 1; i++)
            {
                if (i < n / 4)
                {
                    ColorR[i] = 0;
                    ColorG[i] = 4 * i / (double)n;
                    ColorB[i] = 1;
                }
                else if (i < 2 * n / 4)
                {
                    ColorR[i] = 0;
                    ColorG[i] = 1;
                    ColorB[i] = 1 - 4 * (i - n / 4) / (double)n;
                }
                else if (i < 3 * n / 4)
                {
                    ColorR[i] = (i - n / 2) / (double)(n / 2);
                    ColorG[i] = 1;
                    ColorB[i] = 0;
                }
                else
                {
                    ColorR[i] = 1;
                    ColorG[i] = 1 - (4 * (i - 3 * n / 4)) / (double)n;
                    ColorB[i] = 0;
                }
                ColorLim[i] = Data[arrlist[(int)(num / (n - 1) * (i - 1))], D123 + 数据信息.SelectedIndex + 1];
            }

            ColorLim[1] = Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1];
            ColorLim[n] = Data[arrlist[num], D123 + 数据信息.SelectedIndex + 1];
            min.Text = Data[arrlist[1], D123 + 数据信息.SelectedIndex + 1].ToString();
            max.Text = Data[arrlist[num], D123 + 数据信息.SelectedIndex + 1].ToString();

            ColorR[1] = 0;
            ColorG[1] = 0;
            ColorB[1] = 1;

            ColorR[n] = 1;
            ColorG[n] = 0;
            ColorB[n] = 0;
            needfresh = false;
            //构建完成Colorbar
        }

        //在这里计算每一个elements的颜色
        private void CalcEleCol()
        {
            //对于节点上面的数据
            if (0 == CentList[D123 + 数据信息.SelectedIndex + 1])
            {
                for (int i = 1; i <= element; i++)
                {
                    double value = (Data[E[i, 1], D123 + 数据信息.SelectedIndex + 1] + Data[E[i, 2], D123 + 数据信息.SelectedIndex + 1] + Data[E[i, 3], D123 + 数据信息.SelectedIndex + 1]) / 3;
                    int valuecount = GetColorIndex(value);
                    EleColor[i] = valuecount;
                }
            }
            else
            {
                for (int i = 1; i <= element; i++)
                {
                    double value = Data[i, D123 + 数据信息.SelectedIndex + 1];
                    int valuecount = GetColorIndex(value);
                    EleColor[i] = valuecount;
                }
            }
        }

        //那个拖动的变化的时候 重新更新
        private void ColorNum_Scroll(object sender, EventArgs e)
        {
            js.Text = ColorNum.Value.ToString();
            needfresh = true;
            face = false;
            int n = ColorNum.Value;
            if (CentList[D123 + 数据信息.SelectedIndex + 1] == 0)
            {
                //MixColor(ColorNum.Value, node);
             
                //int num;
                //if (CentList[数据信息.SelectedIndex + D123 + 1] == 1)
                //    num = element;
                //else
                //    num = node;
                ColorLim[1] = Double.Parse(min.Text);
                ColorLim[n] = Double.Parse(max.Text);

                makecolorbar();
                needfresh = false;
                CalcEleCol();
               
            }
            if (CentList[D123 + 数据信息.SelectedIndex + 1] == 1)
            {
                //MixColor(ColorNum.Value, element);
                ColorLim[1] = Double.Parse(min.Text);
                ColorLim[n] = Double.Parse(max.Text);

                makecolorbar();
                needfresh = false;
                CalcEleCol();
            }
            face = true;
        }

        //这里是那个小小的框框
        private void d3d_OpenGLDraw(object sender, PaintEventArgs e)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);
            //清除深度缓存 
            gl.LoadIdentity();
            //重置模型观察矩阵，我认为其实就是重置了三维坐标轴的位置，将其初始化为原点 

            //gl.Translate(0.2*MoveX / SF, 0.2*MoveY / SF, 0f);

            gl.LookAt(sightx, sighty, sightz, 0.0, 0.0, 0.0, 0.0, upy, upz);

            gl.Rotate(anX, 1f, 0f, 0f);
            gl.Rotate(anY, 0f, 0f, 1f);
            gl.Rotate(anZ, 0f, 1f, 0f);

            gl.Color(1, 0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(50 , 0, 0.0);
            gl.End();
            gl.Color(0, 1, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(0, 50, 0.0);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(0, 0.0, 50 );
            gl.End();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((数据信息.SelectedItem.ToString().ToUpper().Contains("TEMPERATURE")) && (isNumberic(温度.Text)))
            {
                try
                {
                    //处理数据如果在中心的情况
                    if (CentList[数据信息.SelectedIndex + D123 + 1] == 1)
                    {
                        for (int i = 1; i <= element; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] += Double.Parse(温度.Text);
                    }
                    //处理在节点上面的数据
                    else
                    {
                        for (int i = 1; i <= node; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] += Double.Parse(温度.Text);
                    }
                    //然后去重新根据这个来做数据
                    CalcEleCol();
                    info.AppendText("Temperature change successful!\r\n");
                }
                catch
                {

                }
            }
        }

        protected bool isNumberic(string message)
        {
            double result = -1;   //result 定义为out 用来输出值
            try
            {

                result = Convert.ToDouble(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //自定义最大最小值
        private void button4_Click(object sender, EventArgs e)
        {
            face = false;
            int n = ColorNum.Value;
            //int num;
            //if (CentList[数据信息.SelectedIndex + D123 + 1] == 1)
            //    num = element;
            //else
            //    num = node;
            ColorLim[1] = Double.Parse(min.Text);
            ColorLim[n] = Double.Parse(max.Text);

            makecolorbar();
            needfresh = false;
            CalcEleCol();
            face = true;
        }

        public void makecolorbar()
        {
            int n = ColorNum.Value;
            for (int i = 2; i <= n - 1; i++)
            {
                if (i < n / 4)
                {
                    ColorR[i] = 0;
                    ColorG[i] = 4 * i / (double)n;
                    ColorB[i] = 1;
                }
                else if (i < 2 * n / 4)
                {
                    ColorR[i] = 0;
                    ColorG[i] = 1;
                    ColorB[i] = 1 - 4 * (i - n / 4) / (double)n;
                }
                else if (i < 3 * n / 4)
                {
                    ColorR[i] = (i - n / 2) / (double)(n / 2);
                    ColorG[i] = 1;
                    ColorB[i] = 0;
                }
                else
                {
                    ColorR[i] = 1;
                    ColorG[i] = 1 - (4 * (i - 3 * n / 4)) / (double)n;
                    ColorB[i] = 0;
                }
                ColorLim[i] = (ColorLim[n] - ColorLim[1]) / n * i;
            }

            ColorR[1] = 0;
            ColorG[1] = 0;
            ColorB[1] = 1;

            ColorR[n] = 1;
            ColorG[n] = 0;
            ColorB[n] = 0;
        }

        //将改动写回数据文件
        private void button5_Click(object sender, EventArgs e)
        {
            if (分区.SelectedItem != null)
            {
                //找到对应的zone 然后把这个zone里面所有
                StreamWriter sw = new StreamWriter("temp.txt");
                bool flag = false;
                bool readline = false;
                bool readface = false;//这个是true的时候 就可以开始读取节点构成面的信息了
                long count = 1;//统计读了每一个属性多少个值了
                int point = 0;//统计读了多少个属性了
                StreamReader sr = new StreamReader(fName);
                string str = sr.ReadLine();
                sw.WriteLine(str);
                while (true)
                {
                    str = sr.ReadLine();
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
                    if ((flag) && str.ToUpper().Contains("DT="))
                    {
                        readline = true;
                        readface = false;
                        sw.WriteLine(str);
                        str = sr.ReadLine();
                    }
                    //这里才开始正式的读取数据
                    if ((flag) && (readline))
                    {
                        //如果是Block数据的话 那么重新构建str
                        if (DataType == "B")
                        {
                            //3D的Block数据
                            string temp = str;
                            string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            str = "";
                            foreach (string s in arraystr)
                            {
                                point++;
                                if (0 == CentList[count])
                                    if (point > node)
                                    {
                                        point = 1;
                                        count++;
                                        //如果已经不是数据了就不要读取了 开始创建面
                                        if (count > D123 + DataSize)
                                        {
                                            readline = false;
                                            readface = true;
                                            sw.Write(temp);
                                        }
                                    }
                                if (1 == CentList[count])
                                    if (point > element)
                                    {
                                        point = 1;
                                        count++;
                                        //如果已经不是数据了就不要读取了 开始创建面
                                        if (count > D123 + DataSize)
                                        {
                                            readline = false;
                                            readface = true;
                                            sw.Write(temp);
                                        }
                                    }
                                //这里是原来用来读取数据的东西 现在就用于输出喽
                                //if ((count < DataSize + D123) && ((DataList[count-3].ToUpper() == "VECTORX") || (DataList[count-3].ToUpper() == "VECTORY") || (DataList[count-3].ToUpper() == "VECTORZ")))
                                if (!readface)
                                str += Data[point, count].ToString("0.0000000e+000") + ' ';
                            }
                        }
                        if (DataType == "P")
                        {
                            //处理点数据
                            for (int i = 1; i <= node; i++)
                            {
                                string[] arraystr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                for (int j = 1; j <= D123 + element; j++)
                                {
                                    //这里也是读取数据的东西
                                    //Data[i, j] = double.Parse(arraystr[j]);
                                }
                                sw.WriteLine(str);
                                str = sr.ReadLine();
                            }
                            readline = false;
                            flag = false;
                        }
                    }
                    //这个代码是处理每一个area包含的节点数 不需要动
                    #region
                    if (readface)
                    {
                        for (int i = 2; i <= element; i++)
                        {
                            sw.WriteLine(str);
                            str = sr.ReadLine();
                        }
                    }
                    #endregion
                    //这里是把数据写会
                    sw.WriteLine(str);
                }
                sr.Close();
                sw.Close();
                File.Copy("temp.txt", "Vector-Tec360.dat", true);
                info.AppendText("Writing Data success \r\n");
            }
            else
            {
                info.AppendText("Please Select the write back region \r\n");
            }
        }

        //生成dataset文件
        private void createdataset()
        {
            StreamWriter sw = new StreamWriter("dataset.dat");
            sw.WriteLine("#百分号是有效数据行的起始标志，出现在有效数据行行首，其他任何地方不得出现百分号");
            sw.WriteLine("%" + ' ' + tc1.Text + ' ' + tc2.Text + ' ' + tc3.Text + " #探测器探测圆心绝对坐标（系统坐标系下）");
            sw.WriteLine("%" + ' ' + tcbj.Text + " #探测半角（角度制）");
            sw.WriteLine("%" + ' ' + bdxx.Text + ' ' + bdsx.Text + " #计算波长范围，单位微米");
            sw.WriteLine("% 2 #当前计算模式下探测方式数目");
            sw.WriteLine("%	 " + ((int.Parse(czzz.Text) - int.Parse(czqs.Text)) / int.Parse(czjg.Text) + 1).ToString() + "                      #第1探测方式探测角数目,探测点数目范围[1,500]  #沿着地面水平方向一周探测");
            sw.WriteLine("%	 " + ((int.Parse(spzz.Text) - int.Parse(spqs.Text)) / int.Parse(spjg.Text) + 1).ToString() + "                     #第2探测方式探测角数目,探测点数目范围[1,500]  #沿着地面水平方向一周探测");
            sw.WriteLine();
            //这里开始输出
            if (isNumberic(czfw.Text) && isNumberic(czqs.Text) && isNumberic(czzz.Text) && isNumberic(czjg.Text))
            {
                for (int i = 0; i <= (Double.Parse(czzz.Text) - Double.Parse(czqs.Text)) / Double.Parse(czjg.Text); i++)
                {
                    if (i > 0)
                        sw.WriteLine("%" + ' ' + tcjl.Text + ' ' + (Double.Parse(czqs.Text) + i * Double.Parse(czjg.Text)).ToString() + ' ' + czfw.Text);
                    else
                        sw.WriteLine("%" + ' ' + tcjl.Text + ' ' + (Double.Parse(czqs.Text) + i * Double.Parse(czjg.Text)).ToString() + ' ' + czfw.Text + ' ' + "#探测距离(0,+8)、天顶角[0,180]、方位角[0,2pi)");
                }
            }
            sw.WriteLine();
            if (isNumberic(sptd.Text) && isNumberic(spqs.Text) && isNumberic(spzz.Text) && isNumberic(spjg.Text))
            {
                for (int i = 0; i <= (Double.Parse(spzz.Text) - Double.Parse(spqs.Text)) / Double.Parse(spjg.Text); i++)
                    sw.WriteLine("%" + ' ' + tcjl.Text + ' ' + sptd.Text + ' ' + (Double.Parse(spqs.Text) + i * Double.Parse(spjg.Text)).ToString());
            }
            //开始计算太阳高度角和方位角
            if (isNumberic(month.Text) && isNumberic(date.Text) && isNumberic(time.Text) && isNumberic(lat.Text))
                sw.WriteLine("%" + ' ' + Azimuth1(Int32.Parse(month.Text), Int32.Parse(date.Text), Int32.Parse(time.Text), double.Parse(lat.Text)).ToString() + ' ' + Zenith1(Int32.Parse(month.Text), Int32.Parse(date.Text), Int32.Parse(time.Text), double.Parse(lat.Text)).ToString() + ' ' + " #太阳方位角，太阳高度角，角度制");
            //太阳波数数目
            sw.WriteLine("%" + ' ' + Math.Round(Math.Ceiling(10000 / Double.Parse(bdxx.Text)) - Math.Floor(10000 / Double.Parse(bdsx.Text)) + 1).ToString() + ' ' + "#太阳波数数目");
            sw.WriteLine("%" + ' ' + (Double.Parse(hjhf.Text) + 1).ToString() + ' ' + "#环境辐射角度数目");
            sw.WriteLine("%" + ' ' + Math.Round(Math.Ceiling(10000 / Double.Parse(bdxx.Text)) - Math.Floor(10000 / Double.Parse(bdsx.Text)) + 1).ToString() + ' ' + "#环境辐射波数数目，必须和太阳波数数目相同");
            sw.Close();
            info.AppendText("Create Dataset.dat Success \r\n");
        }

        //这里是运行Environ的代码
        public void runEnv()
        {
            Process.Start("Environ.exe");
            button10.Enabled = false;
            info.AppendText("Running Environ.exe \r\n");
            timer1.Start();
            info.AppendText("Waiting Enciron finish \r\n");

        }

        //提供的两个函数
        const double pi = 3.1415926535898;
        double Zenith1(int mon_in, int day_in, double time_in, double latitude_in)
        {
            double day, SL, w, deta, beta;

            day = 30 * (mon_in - 1) + day_in;
            SL = 4.87 + 0.0175 * day + 0.033 * Math.Sin(0.0175 * day);
            deta = Math.Asin(0.398 * Math.Sin(SL));
            w = (time_in - 12) * pi / 12;
            beta = Math.Asin(Math.Sin(latitude_in * pi / 180) * Math.Sin(deta) + Math.Cos(latitude_in * pi / 180) * Math.Cos(deta) * Math.Cos(w));
            beta = beta / pi * 180.0;

            return beta;
        }
        double Azimuth1(int mon_in, int day_in, double time_in, double latitude_in)
        {
            double day, SL, w, deta, beta, alpha;

            day = 30 * (mon_in - 1) + day_in;
            SL = 4.87 + 0.0175 * day + 0.033 * Math.Sin(0.0175 * day);
            deta = Math.Asin(0.398 * Math.Sin(SL));
            w = (time_in - 12) * pi / 12;
            beta = Math.Asin(Math.Sin(latitude_in * pi / 180) * Math.Sin(deta) + Math.Cos(latitude_in * pi / 180) * Math.Cos(deta) * Math.Cos(w));

            alpha = (Math.Sin(beta) * Math.Sin(latitude_in * pi / 180) - Math.Sin(deta)) / (Math.Cos(beta) * Math.Cos(latitude_in * pi / 180));
            alpha = Math.Acos(alpha);
            alpha = alpha / pi * 180.0;
            return alpha;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if ((null != qrj.SelectedItem) && (null != dqms.SelectedItem) && (null != jy.SelectedItem))
            {
                StreamWriter sw = new StreamWriter("Environset.dat");
                sw.WriteLine("#百分号是有效数据行的起始标志，出现在有效数据行行首，其他任何地方不得出现百分号");
                sw.WriteLine("%" + ' ' + bdxx.Text + ' ' + bdsx.Text + ' ' + "#计算波长范围，单位微米");
                sw.WriteLine("%" + ' ' + mbgd.Text + ' ' + lat.Text + ' ' + "#目标高度，纬度");
                sw.WriteLine("%" + ' ' + month.Text + ' ' + date.Text + ' ' + time.Text + ' ' + "#月份，日期，时间");
                if (dm.Checked)
                    sw.WriteLine("%" + ' ' + hjhf.Text + ' ' + '0' + " #环境划分份数m_Number，背景类型默认地面为0，海面为1");
                if (hm.Checked)
                    sw.WriteLine("%" + ' ' + hjhf.Text + ' ' + '1' + " #环境划分份数m_Number，背景类型默认地面为0，海面为1");
                sw.WriteLine("%" + ' ' + bjwd.Text + ' ' + fsl.Text + ' ' + "#地面温度，发射率");
                sw.WriteLine("%" + ' ' + czfw.Text + ' ' + czqs.Text + ' ' + czzz.Text + ' ' + czjg.Text + ' ' + "#垂直探测设置");
                sw.WriteLine("%" + ' ' + sptd.Text + ' ' + spqs.Text + ' ' + spzz.Text + ' ' + spjg.Text + ' ' + "#水平探测设置");
                sw.Write("%" + ' ' + (dqms.SelectedIndex + 1).ToString());
                if (qrj.SelectedIndex < 7)
                    sw.Write(' ' + (qrj.SelectedIndex).ToString());
                else
                    sw.Write(' ' + (qrj.SelectedIndex + 1).ToString());
                if (jy.SelectedIndex < 12)
                    sw.WriteLine(' ' + jy.SelectedIndex.ToString() + ' ' + "#大气模式，气溶胶模式，云模式和雨强");
                else
                    sw.WriteLine(' ' + (jy.SelectedIndex + 7).ToString() + ' ' + "#大气模式，气溶胶模式，云模式和雨强");
                sw.WriteLine("%" + ' ' + hbgd.Text + ' ' + yq.Text + ' ' + qxsj.Text + ' ' + "#海拔高度，雨强，气象视距");
                sw.WriteLine("%" + ' ' + dsfs.Text + ' ' + pjfs.Text + ' ' + tcjl.Text + ' ' + "#当时风速，平均风速，探测距离");
                sw.Close();
                info.AppendText("Create Environset Success \r\n");
                runEnv();
            }
            else
            {
                info.AppendText("Please Fill the info! \r\n");
            }
        }

        private void createin()
        {
            if (null != wgdw.SelectedItem)
            {
                StreamWriter sw = new StreamWriter("in.dat");
                sw.WriteLine("#百分号是有效数据行的起始标志，出现在有效数据行行首，其他任何地方不得出现百分号");
                sw.WriteLine("%" + NumZone.ToString() + " #分区面元的总数目");
                for (int i = 1; i <= NumZone; i++)
                {
                    sw.WriteLine("%" + Zone[i]);
                }
                sw.WriteLine("%" + gxsm.Text + ' ' + "#发射光线数目[1-21000000000]");
                sw.WriteLine("%dataset.dat #设置文件名称");
                sw.WriteLine("%solar.dat #太阳辐射文件名称");
                sw.WriteLine("%environ.dat #环境辐射文件");
                sw.WriteLine("%trans.dat #大气衰减文件名称");
                sw.WriteLine("%" + System.IO.Path.GetFileName(openFileDialog1.FileName) + " #CGNS数据文件名称（打开文件的名称）");
                sw.WriteLine("%" + wgdw.SelectedItem.ToString() + ' ' + " #CGNS数据文件单位[m,cm,mm]");
                sw.WriteLine("%" + xskjx.Text + ' ' + xskjy.Text + ' ' + "#像素空间大小，如100 X 100");
                sw.Close();
            }
            else
            {
                info.AppendText("Please Fill the info! \r\n");
            }
        }


        //在这里检测Environ有木有运行完
        private void timer1_Tick(object sender, EventArgs e)
        {
            bool has = false;
            foreach (Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.Contains("Environ"))
                {
                    has = true;
                    info.AppendText("Waitting Environ.exe finished. Check next 5s \r\n");
                }
            }
            if (has == false)
            {
                info.AppendText("Finish Environ.exe \r\n");
                timer1.Stop();
                button10.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (zdy.Checked)
            {
                qxsj.ReadOnly = false;
                dsfs.ReadOnly = false;
                pjfs.ReadOnly = false;
            }
        }

        private void mr_CheckedChanged(object sender, EventArgs e)
        {
            if (mr.Checked)
            {
                qxsj.ReadOnly = true;
                dsfs.ReadOnly = true;
                pjfs.ReadOnly = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            createdataset();
            createin();
            //把输入文件拷贝到程序工作目录里面来
            //   File.Copy(fName,System.IO.Path.GetFileName(fName),true);
            Process.Start("AirCraft.exe");
            info.AppendText("Running AirCraft.exe \r\n");
            timer2.Start();
            info.AppendText("Waiting AirCraft finish \r\n");
        }

        private void 新建工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "请选择任务保存的路径";
            folderBrowserDialog1.ShowNewFolderButton = true;
            //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                if (folderName != "")
                {
                    File.Copy("config.sln", folderName + @"\config.sln", true);
                    // MessageBox.Show(folderName);
                    slnName = folderName + @"\config.sln";
                    StreamReader sr = new StreamReader(slnName);
                    //sr.ReadLine();
                    gxsm.Text = sr.ReadLine();
                    tc1.Text = sr.ReadLine(); tc2.Text = sr.ReadLine(); tc3.Text = sr.ReadLine();
                    tcbj.Text = sr.ReadLine();
                    tcjl.Text = sr.ReadLine();
                    bdxx.Text = sr.ReadLine();
                    bdsx.Text = sr.ReadLine();
                    xskjx.Text = sr.ReadLine();
                    xskjy.Text = sr.ReadLine();
                    czfw.Text = sr.ReadLine();
                    czqs.Text = sr.ReadLine();
                    czzz.Text = sr.ReadLine();
                    czjg.Text = sr.ReadLine();
                    sptd.Text = sr.ReadLine();
                    spqs.Text = sr.ReadLine();
                    spzz.Text = sr.ReadLine();
                    spjg.Text = sr.ReadLine();
                    dm.Checked = sr.ReadLine().Contains("True");
                    hm.Checked = sr.ReadLine().Contains("True");
                    dqms.SelectedIndex = Int32.Parse(sr.ReadLine());
                    qrj.SelectedIndex = Int32.Parse(sr.ReadLine());
                    jy.SelectedIndex = Int32.Parse(sr.ReadLine());
                    month.Text = sr.ReadLine();
                    date.Text = sr.ReadLine();
                    time.Text = sr.ReadLine();
                    lat.Text = sr.ReadLine();
                    mbgd.Text = sr.ReadLine();
                    hbgd.Text = sr.ReadLine();
                    bjwd.Text = sr.ReadLine();
                    fsl.Text = sr.ReadLine();
                    yq.Text = sr.ReadLine();
                    mr.Checked = sr.ReadLine().Contains("True");
                    zdy.Checked = sr.ReadLine().Contains("True");
                    qxsj.Text = sr.ReadLine();
                    dsfs.Text = sr.ReadLine();
                    pjfs.Text = sr.ReadLine();
                    hjhf.SelectedIndex = Int32.Parse(sr.ReadLine());
                    dm.Checked = (sr.ReadLine().Contains("True"));
                    hm.Checked = (sr.ReadLine().Contains("True"));
                    mr.Checked = (sr.ReadLine().Contains("True"));
                    zdy.Checked = (sr.ReadLine().Contains("True"));
                    string ca = sr.ReadLine();
                    button1.Enabled = true;
                    info.AppendText("Create porject success \r\n");
                    //   textBox1.Text = folderName;
                }
            }

        }
        string slnName;
        private void 打开工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "解决方案|*.sln|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                slnName = openFileDialog1.FileName;
                folderName = System.IO.Path.GetDirectoryName(slnName);
                info.AppendText("Open Exist Project: " + slnName + "\r\n");
                StreamReader sr = new StreamReader(slnName);
                //sr.ReadLine();
                gxsm.Text = sr.ReadLine();
                tc1.Text = sr.ReadLine(); tc2.Text = sr.ReadLine(); tc3.Text = sr.ReadLine();
                tcbj.Text = sr.ReadLine();
                tcjl.Text = sr.ReadLine();
                bdxx.Text = sr.ReadLine();
                bdsx.Text = sr.ReadLine();
                xskjx.Text = sr.ReadLine();
                xskjy.Text = sr.ReadLine();
                czfw.Text = sr.ReadLine();
                czqs.Text = sr.ReadLine();
                czzz.Text = sr.ReadLine();
                czjg.Text = sr.ReadLine();
                sptd.Text = sr.ReadLine();
                spqs.Text = sr.ReadLine();
                spzz.Text = sr.ReadLine();
                spjg.Text = sr.ReadLine();
                dm.Checked = sr.ReadLine().Contains("True");
                hm.Checked = sr.ReadLine().Contains("True");
                dqms.SelectedIndex = Int32.Parse(sr.ReadLine());
                qrj.SelectedIndex = Int32.Parse(sr.ReadLine());
                jy.SelectedIndex = Int32.Parse(sr.ReadLine());
                month.Text = sr.ReadLine();
                date.Text = sr.ReadLine();
                time.Text = sr.ReadLine();
                lat.Text = sr.ReadLine();
                mbgd.Text = sr.ReadLine();
                hbgd.Text = sr.ReadLine();
                bjwd.Text = sr.ReadLine();
                fsl.Text = sr.ReadLine();
                yq.Text = sr.ReadLine();
                mr.Checked = sr.ReadLine().Contains("True");
                zdy.Checked = sr.ReadLine().Contains("True");
                qxsj.Text = sr.ReadLine();
                dsfs.Text = sr.ReadLine();
                pjfs.Text = sr.ReadLine();
                hjhf.SelectedIndex = Int32.Parse(sr.ReadLine());
                dm.Checked = (sr.ReadLine().Contains("True"));
                hm.Checked = (sr.ReadLine().Contains("True"));
                mr.Checked = (sr.ReadLine().Contains("True"));
                zdy.Checked = (sr.ReadLine().Contains("True"));
                string ca = sr.ReadLine();
                button1.Enabled = true;
            }
        }

        private void 保存工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == fName)
            {
                info.AppendText("Lack of input files! \r\n");
            }
            else if (null == slnName)
            {
                folderBrowserDialog1.Description = "请选择任务保存的路径";
                folderBrowserDialog1.ShowNewFolderButton = true;
                //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string folderName = folderBrowserDialog1.SelectedPath;
                    if (folderName != "")
                    {
                        if (!File.Exists(folderName + @"\" + System.IO.Path.GetFileName(fName)))
                            File.Copy(fName, folderName + @"\" + System.IO.Path.GetFileName(fName));
                        StreamWriter sw = new StreamWriter(folderName + @"\config.sln");
                        //sw.WriteLine(folderName);
                        sw.WriteLine(gxsm.Text);
                        sw.WriteLine(tc1.Text); sw.WriteLine(tc2.Text); sw.WriteLine(tc3.Text);
                        sw.WriteLine(tcbj.Text);
                        sw.WriteLine(tcjl.Text);
                        sw.WriteLine(bdxx.Text);
                        sw.WriteLine(bdsx.Text);
                        sw.WriteLine(xskjx.Text);
                        sw.WriteLine(xskjy.Text);
                        sw.WriteLine(czfw.Text);
                        sw.WriteLine(czqs.Text);
                        sw.WriteLine(czzz.Text);
                        sw.WriteLine(czjg.Text);
                        sw.WriteLine(sptd.Text);
                        sw.WriteLine(spqs.Text);
                        sw.WriteLine(spzz.Text);
                        sw.WriteLine(spjg.Text);
                        sw.WriteLine(dm.Checked);
                        sw.WriteLine(hm.Checked);
                        sw.WriteLine(dqms.SelectedIndex);
                        sw.WriteLine(qrj.SelectedIndex);
                        sw.WriteLine(jy.SelectedIndex);
                        sw.WriteLine(month.Text);
                        sw.WriteLine(date.Text);
                        sw.WriteLine(time.Text);
                        sw.WriteLine(lat.Text);
                        sw.WriteLine(mbgd.Text);
                        sw.WriteLine(hbgd.Text);
                        sw.WriteLine(bjwd.Text);
                        sw.WriteLine(fsl.Text);
                        sw.WriteLine(yq.Text);
                        sw.WriteLine(mr.Checked);
                        sw.WriteLine(zdy.Checked);
                        sw.WriteLine(qxsj.Text);
                        sw.WriteLine(dsfs.Text);
                        sw.WriteLine(pjfs.Text);
                        sw.WriteLine(hjhf.SelectedIndex);
                        sw.WriteLine(dm.Checked);
                        sw.WriteLine(hm.Checked);
                        sw.WriteLine(mr.Checked);
                        sw.WriteLine(zdy.Checked);
                        sw.Close();
                        info.AppendText("Save project success \r\n");
                    }
                }
            }
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Help.chm");
        }

        //在这里load一维的数据并且显示出来
        private void button11_Click(object sender, EventArgs e)
        {
            D1D D1D = new D1D();
            D1D.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            D3D D3D = new D3D();
            D3D.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            D2D D2D = new D2D();
            D2D.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //    IntPtr a=new IntPtr();
            //    gl.ReadPixels(0, 0, 719, 364,
            //OpenGL.GL_BGR_EXT, OpenGL.GL_UNSIGNED_BYTE, a);
            //    Graphics g1 = Graphics.FromHdc(a);
            //    Bitmap image = new Bitmap(719, 364, g1);
            //    image.Save("a.bmp");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            bool has = false;
            foreach (Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.Contains("aircraft"))
                {
                    has = true;
                    info.AppendText("Waitting Aircraft.exe finished. Check next 5s \r\n");
                }
            }
            if (has == false)
            {
                info.AppendText("Finish AirCraft.exe \r\n");
                timer2.Stop();
                File.Copy("DetIntensity-1D", path + "DetIntensity-1D", true);
                File.Copy("InfraredImage-2D", path + "InfraredImage-2D", true);
                File.Copy("PhysicalFields-3D", path + "DetIntensity-1D", true);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if ((数据信息.SelectedItem.ToString().ToUpper().Contains("TEMPERATURE")) && (isNumberic(wdsz.Text)))
            {
                try
                {
                    //处理数据如果在中心的情况
                    if (CentList[数据信息.SelectedIndex + D123 + 1] == 1)
                    {
                        for (int i = 1; i <= element; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] = Double.Parse(wdsz.Text);
                    }
                    //处理在节点上面的数据
                    else
                    {
                        for (int i = 1; i <= node; i++)
                            Data[i, 数据信息.SelectedIndex + D123 + 1] = Double.Parse(wdsz.Text);
                    }
                    //然后去重新根据这个来做数据
                    CalcEleCol();
                    info.AppendText("Temperature Set Successful!\r\n");
                }
                catch
                {

                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fxxs *= -1;
            for (int i = 1; i <= DataSize; i++)
            {
                if ((DataList[i].ToUpper().Contains("VECTOR")) || (DataList[i].ToUpper().Contains("DIRECTION")))
                    if (CentList[i] == 1)
                    {
                        for (int j = 1; j <= element; j++)
                            Data[j, i] *= -1;
                    }
                    //处理在节点上面的数据
                    else
                    {
                        for (int j = 1; j <= node; j++)
                            Data[j, i] *= -1;
                    }
            }
            CalcEleCol();

        }

        private void 另存工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == fName)
            {
                info.AppendText("Lack of input files! \r\n");
            }
            else if (null == slnName)
            {
                folderBrowserDialog1.Description = "请选择另存路径";
                folderBrowserDialog1.ShowNewFolderButton = true;
                //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string folderName = folderBrowserDialog1.SelectedPath;
                    if (folderName != "")
                    {
                        if (!File.Exists(folderName + @"\" + System.IO.Path.GetFileName(fName)))
                            File.Copy(fName, folderName + @"\" + System.IO.Path.GetFileName(fName));
                        StreamWriter sw = new StreamWriter(folderName + @"\config.sln");
                        //sw.WriteLine(folderName);
                        sw.WriteLine(gxsm.Text);
                        sw.WriteLine(tc1.Text); sw.WriteLine(tc2.Text); sw.WriteLine(tc3.Text);
                        sw.WriteLine(tcbj.Text);
                        sw.WriteLine(tcjl.Text);
                        sw.WriteLine(bdxx.Text);
                        sw.WriteLine(bdsx.Text);
                        sw.WriteLine(xskjx.Text);
                        sw.WriteLine(xskjy.Text);
                        sw.WriteLine(czfw.Text);
                        sw.WriteLine(czqs.Text);
                        sw.WriteLine(czzz.Text);
                        sw.WriteLine(czjg.Text);
                        sw.WriteLine(sptd.Text);
                        sw.WriteLine(spqs.Text);
                        sw.WriteLine(spzz.Text);
                        sw.WriteLine(spjg.Text);
                        sw.WriteLine(dm.Checked);
                        sw.WriteLine(hm.Checked);
                        sw.WriteLine(dqms.SelectedIndex);
                        sw.WriteLine(qrj.SelectedIndex);
                        sw.WriteLine(jy.SelectedIndex);
                        sw.WriteLine(month.Text);
                        sw.WriteLine(date.Text);
                        sw.WriteLine(time.Text);
                        sw.WriteLine(lat.Text);
                        sw.WriteLine(mbgd.Text);
                        sw.WriteLine(hbgd.Text);
                        sw.WriteLine(bjwd.Text);
                        sw.WriteLine(fsl.Text);
                        sw.WriteLine(yq.Text);
                        sw.WriteLine(mr.Checked);
                        sw.WriteLine(zdy.Checked);
                        sw.WriteLine(qxsj.Text);
                        sw.WriteLine(dsfs.Text);
                        sw.WriteLine(pjfs.Text);
                        sw.WriteLine(hjhf.SelectedIndex);
                        sw.WriteLine(dm.Checked);
                        sw.WriteLine(hm.Checked);
                        sw.WriteLine(mr.Checked);
                        sw.WriteLine(zdy.Checked);
                        sw.Close();
                        info.AppendText("Save project success \r\n");
                    }
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



    }
}
