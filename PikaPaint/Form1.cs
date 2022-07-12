using System;
using System.Drawing;
using System.Windows.Forms;

namespace PikaPaint
{
    public partial class Form1 : Form
    {
        // 当前鼠标是否被按下
        private bool isMousePressed = false;
        // 画笔
        private readonly Pen brush;
        // 画布
        private Graphics canvas;
        // 上次鼠标的位置
        int x, y;
        // 上次选择的颜色
        Panel lastPanel;

        public Form1()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            LoadColors();

            // 初始化画笔
            this.brush = new Pen(Color.Black, 1);
            // 从界面的图片控件中创建一个画布
            this.canvas = pictureBox1.CreateGraphics();
            // 打开抗锯齿
            this.canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void LoadColors()
        {
            // 枚举所有可用的颜色，显示到屏幕上
            foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
            {
                Color now = Color.FromKnownColor(color);
                // 过滤掉太白的颜色
                if (now.R > 200 && now.G > 200 && now.B > 200) continue;
                // 创建每一个颜色小方格
                Panel colorPanel = new Panel()
                {
                    Size = new Size(25, 25),
                    BackColor = now,
                    ForeColor = now,
                };
                // 为这个颜色小方格加上点击事件
                colorPanel.Click += new EventHandler(ChangeColor_Click);
                // 将这个颜色小方格添加到控件中
                this.flowLayoutPanel1.Controls.Add(colorPanel);
            }
        }

        private void ChangeColor_Click(object sender, EventArgs e)
        {
            // 根据用户选择的颜色来设置画笔的颜色，设置为选中
            if (lastPanel != null) lastPanel.BorderStyle = BorderStyle.None;
            // 然后获取新的颜色，设置到画笔中
            lastPanel = ((Panel)sender);
            lastPanel.BorderStyle = BorderStyle.Fixed3D;
            this.brush.Color = lastPanel.BackColor;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 鼠标点击时获取一个初始位置
            this.isMousePressed = true;
            this.x = e.X;
            this.y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMousePressed)
            {
                // 在鼠标拖动时在鼠标此时的位置和上一个位置画一条线条
                this.canvas.DrawLine(brush, new Point(x, y), e.Location);
                // 更新鼠标现在的位置
                this.x = e.X;
                this.y = e.Y;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMousePressed = false;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            // 按下清除按钮时清空现在画布上的所有痕迹
            this.canvas.Clear(Color.White);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // 改变当前画笔的粗细
            this.brush.Width = (float)((TrackBar)sender).Value;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // 如果窗口大小改变了，就需要重新创建画布
            this.canvas = pictureBox1.CreateGraphics();
            this.groupBox1.Refresh();
        }
    }
}
