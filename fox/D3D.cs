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
    public partial class D3D : Form
    {
        public D3D()
        {
            InitializeComponent();
            loadAll();
        }
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
        Int32[] arrlist = new int[35000];
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
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(50 / SF, 0, 0.0);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0.0);
            gl.Vertex(0, 50 / SF, 0.0);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
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

        private void loadAll()
        {
            fName = "PhysicalFields-3D";
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
           if (数据信息.SelectedItem==null) 数据信息.SelectedIndex = 0;
        }

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

        private void D3D_SizeChanged(object sender, EventArgs e)
        {
            openGLControl.Size = new System.Drawing.Size(this.Size.Width-250 ,this.Size.Height-80);
        }
    }
}
