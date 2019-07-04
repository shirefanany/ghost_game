using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace oldfinal_4_curve_and_transformation
{
    public class LineSeg
    {
        public PointF ps;
        public PointF pe;

        public void DrawYourSelf(Graphics g)
        {
            Pen p = new Pen(Color.Green,5);
            g.DrawLine(p, ps.X, ps.Y, pe.X, pe.Y);
            g.FillEllipse(Brushes.Blue, ps.X - 5, ps.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Red, pe.X - 5, pe.Y - 5, 10, 10);

        }

        public void Translation(float tx, float ty)
        {
            ps.X += tx;
            ps.Y += ty;

            pe.X += tx;
            pe.Y += ty;
        }

        public void Rotate(float th_degg)
        {
            double xn, yn;
            double th_red = th_degg * Math.PI / 180;

            xn = ps.X * Math.Cos(th_red) - ps.Y * Math.Sin(th_red);
            yn = ps.X * Math.Sin(th_red) + ps.Y * Math.Cos(th_red);
            ps.X = (float)xn;
            ps.Y = (float)yn;

            xn = pe.X * Math.Cos(th_red) - pe.Y * Math.Sin(th_red);
            yn = pe.X * Math.Sin(th_red) + pe.Y * Math.Cos(th_red);
            pe.X = (float)xn;
            pe.Y = (float)yn;

        }

        public void RotateAround(float th_degg, PointF refPt)
        {
            Translation(-refPt.X, -refPt.Y);
            Rotate(th_degg);
            Translation(refPt.X, refPt.Y);
        }
    }

    public partial class Form1 : Form
    {
        Bitmap off;

        Timer t = new Timer();

        BizzearCurve my_crv = new BizzearCurve();

        List<LineSeg> L_segmant = new List<LineSeg>(); LineSeg ptrav;
        List<LineSeg> L_segmant2 = new List<LineSeg>(); LineSeg ptrav2;

        int counttick = 0;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);

            t.Tick += new EventHandler(t_Tick);
            t.Interval = 50;
            t.Start();
        }

        int variable = 0;
        int index = 0;

        int done = 0;
        void t_Tick(object sender, EventArgs e)
        {
            if (done == 0)
            {
                if (variable <= L_segmant.Count - 1)
                {
                    PointF refPt = new PointF(L_segmant[variable].ps.X, L_segmant[variable].ps.Y);

                    if (counttick < 35)
                    {
                        for (int i = index; i < L_segmant.Count; i++)
                        {

                            L_segmant[i].RotateAround(-1, refPt);

                        }
                    }


                    if (counttick > 35 && counttick < 37)
                    {
                        index++;
                        variable++;
                        //t.Stop();
                    }

                    if (counttick >= 35)
                    {

                        if (counttick < 70)
                        {
                            for (int i = index; i < L_segmant.Count; i++)
                            {
                                L_segmant[i].RotateAround(1, refPt);
                            }
                        }

                    }

                    if (counttick > 70 && counttick < 72)
                    {

                        counttick = -1;
                        variable++;
                        index++;

                    }


                }
                else
                {
                    if (counttick < 15)
                    {
                        PointF refPt = new PointF(L_segmant[3].ps.X, L_segmant[3].ps.Y);
                        for (int i = 0; i < L_segmant.Count; i++)
                        {
                            L_segmant[i].RotateAround(1, refPt);
                        }
                    }
                    else
                    {
                        if (counttick < 35)
                        {
                            for (int i = 0; i < L_segmant.Count; i++)
                            {
                                L_segmant[i].Translation(0, 5);
                            }
                        }
                        else if (counttick > 35 && counttick == 37)
                        {
                            ptrav2 = new LineSeg();
                            ptrav2.ps.X = my_crv.ControlPoints[3].X;
                            ptrav2.ps.Y = my_crv.ControlPoints[3].Y;
                            ptrav2.pe.X = my_crv.ControlPoints[2].X;
                            ptrav2.pe.Y = my_crv.ControlPoints[2].Y;
                            L_segmant2.Add(ptrav2);


                            ptrav2 = new LineSeg();
                            ptrav2.ps.X = my_crv.ControlPoints[4].X;
                            ptrav2.ps.Y = my_crv.ControlPoints[4].Y;
                            ptrav2.pe.X = my_crv.ControlPoints[5].X;
                            ptrav2.pe.Y = my_crv.ControlPoints[5].Y;
                            L_segmant2.Add(ptrav2);
                            counttick = -1;
                            done = 1;

                        }
                    }
                }
            }

            if (done == 1)
            {
                if (counttick < 25)
                {
                    for (int i = 0; i < L_segmant2.Count; i++)
                    {
                        PointF refPt = new PointF(L_segmant2[i].ps.X,L_segmant2[i].ps.Y);
                        L_segmant2[i].RotateAround(1, refPt);
                    }
                    my_crv.ModifyCtrlPoint(2, (int)L_segmant2[0].pe.X, (int)L_segmant2[0].pe.Y);
                    my_crv.ModifyCtrlPoint(5, (int)L_segmant2[1].pe.X, (int)L_segmant2[1].pe.Y);
                }

                if (counttick > 25 && counttick < 50)
                {
                    for (int i = 0; i < L_segmant2.Count; i++)
                    {
                        PointF refPt = new PointF(L_segmant2[i].ps.X, L_segmant2[i].ps.Y);
                        L_segmant2[i].RotateAround(-1, refPt);


                        
                    }
                    my_crv.ModifyCtrlPoint(2, (int)L_segmant2[0].pe.X, (int)L_segmant2[0].pe.Y);
                    my_crv.ModifyCtrlPoint(5, (int)L_segmant2[1].pe.X, (int)L_segmant2[1].pe.Y);
                }

                if (counttick > 50 && counttick == 51)
                {
                    counttick = 0;
                }
            }
            
            counttick++;
            Graphics g = this.CreateGraphics();
            drawdubb(g);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            my_crv.SetControlPoint(new Point(35, 450));
            my_crv.SetControlPoint(new Point(300, 250));
            my_crv.SetControlPoint(new Point(30, 100));
            my_crv.SetControlPoint(new Point(260, 25));
            my_crv.SetControlPoint(new Point(450, 25));
            my_crv.SetControlPoint(new Point(630, 100));
            my_crv.SetControlPoint(new Point(440, 250));
            my_crv.SetControlPoint(new Point(650, 450));

            int x = my_crv.ControlPoints[0].X;
            for (int i = 0; i < 8; i++)
            {
                ptrav = new LineSeg();
                ptrav.ps.X = x;
                ptrav.ps.Y = my_crv.ControlPoints[0].Y;
                ptrav.pe.X = x + 100;
                ptrav.pe.Y = my_crv.ControlPoints[0].Y; ;
                L_segmant.Add(ptrav);

                x = x + 100;
            }
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);
        }

        void drawscene(Graphics g2)
        {
            g2.Clear(Color.White);

            my_crv.DrawCurve(g2);

            g2.FillEllipse(Brushes.Black, 250, 250,50,50);
            g2.FillEllipse(Brushes.Black, 400, 250, 50, 50);

            for (int i = 0; i < L_segmant.Count; i++)
            {
                L_segmant[i].DrawYourSelf(g2);
            }

            for (int i = 0; i < L_segmant2.Count; i++)
            {
                L_segmant2[i].DrawYourSelf(g2);
            }
        }

        void drawdubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);

        }
    }
}