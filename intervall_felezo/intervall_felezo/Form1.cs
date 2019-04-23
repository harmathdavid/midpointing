using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace intervall_felezo
{
    public partial class Form1 : Form
    {
        Graphics g;
        Brush bp = new SolidBrush(Color.Red);
        Pen pl = new Pen(Color.Blue, 2);
        Pen line = new Pen(Color.Red);
        List<PointF> points;
        Point origo;
        int iterate;
        float sx1, sx2;
        float epsilon = 0.5f;

        public Form1()
        {
            InitializeComponent();
            points = new List<PointF>();
            origo = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            iterate = 0;
            this.KeyDown += Form1_KeyDown;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(points.Count==2)
                {
                    iterate++;
                    IntervallBisect(iterate);
                }
                else
                {
                    MessageBox.Show("Kérlek előbb helyezz el a pontokat az egér segítségével");
                }
                
            }
            else if (e.KeyCode == Keys.Space)
            {
                iterate = 0;
                Clear();
                line.Color = Color.Red;
            }
        }

        int found = -1;
        int size = 4;
        int scale = 20;



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (points.Count == 2)
            {
                if (checkPointsAreValid(points))
                {
                    sx1 = points[0].X;
                    sx2 = points[1].X;

                    pictureBox1.Refresh();
                    
                }
                else
                {
                    MessageBox.Show("Nincs metszet az X tengellyel");
                    Clear();
                }

            }

            found = -1;

        }     


        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (points.Count < 2)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (Math.Abs(points[i].X - e.X) < size && Math.Abs(points[i].Y - e.Y) < size)
                        found = i;
                }

                if (found == -1)
                {
                    points.Add(e.Location);
                    pictureBox1.Refresh();

                }
            }


        }

        void DrawCoordSys()
        {
            g.DrawLine(new Pen(Color.Black), new PointF(0, pictureBox1.Height / 2), new PointF(pictureBox1.Width, pictureBox1.Height / 2));
            g.DrawLine(new Pen(Color.Black), new PointF(pictureBox1.Width / 2, 0), new PointF(pictureBox1.Width / 2, pictureBox1.Height));

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                if (i % scale == 0)
                    g.DrawRectangle(new Pen(Color.Black), i, (pictureBox1.Height / 2) - 2, 1, 4);
            }
            for (int i = 0; i < pictureBox1.Height; i++)
            {
                if (i % scale == 0)
                    g.DrawRectangle(new Pen(Color.Black), (pictureBox1.Width / 2) - 2, i, 4, 1);
            }

        }

        bool checkPointsAreValid(List<PointF> l)
        {
            PointF SPoint1 = position(points[0]);
            PointF SPoint2 = position(points[1]);

            if (Math.Sign(SPoint1.X) != Math.Sign(SPoint2.X))
                return true;
            else return false;

        }

        void Clear()
        {
            points.Clear();
            pictureBox1.Refresh();
        }

        //2 pontra illeszkedő egyenes egyenlete
        float basicFunction(PointF A, PointF B, float x)
        {
            PointF V = new PointF(B.X - A.X, B.Y - A.Y);

            return ((V.Y * A.X - V.X * A.Y) - V.Y * x) / -(V.X);
        }

        float fx(float x)
        {
            return basicFunction(points[0], points[1], x);
        }


        void IntervallBisect(int iterations)
        {
            float c;
            
            c = (sx1+sx2) / 2;

            if (position(fx(sx1)) * position(fx(c))< 0)
                sx2 = c;
            else
                sx1 = c;


            if (Math.Abs(sx1 - sx2) < epsilon)
            {
                line.Color = Color.Green;
            }

            pictureBox1.Refresh();
        }
        float position(float A)
        {
            if (A < origo.Y)
            {
                return A * (-1);
            }
            else
            {
                return A;
            }

        }
        
        PointF position(PointF A)
        {
            if (A.X < origo.X && A.Y < origo.Y)
                return new PointF(A.X * (-1), A.Y);
            else if (A.X > origo.X && A.Y < origo.Y)
                return new PointF(A.X * (-1), A.Y * (-1));
            else if (A.X > origo.X && A.Y > origo.Y)
                return new PointF(A.X, A.Y * (-1));
            else
                return A;
        }

        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

          
            DrawCoordSys();

            if (points.Count == 2)
            {
               
                g.DrawLine(line, new PointF(Math.Abs(sx1), 0), new PointF(Math.Abs(sx1), pictureBox1.Height));
                g.DrawLine(line, new PointF(Math.Abs(sx2), 0), new PointF(Math.Abs(sx2), pictureBox1.Height));

                g.DrawLine(new Pen(Color.Blue), points[0], points[1]);
            }

            foreach (var p in points)
            {
                g.FillEllipse(bp, p.X - size, p.Y - size, size * 2, size * 2);
            }
        }


    }
}

