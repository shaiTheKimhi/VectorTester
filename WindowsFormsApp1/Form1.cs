using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int vertical = 0;
        const int CIRCLE_TIME = 1 * 60 ;
        const int FIELD_SIZE = 75;
        Bitmap bitmap;
        DateTime time;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            time = DateTime.Now;
            this.bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = this.bitmap;
            Thread t = new Thread(() =>
            {
            try
            {
                while (true)
                {
                    
                    Thread.Sleep(10);
                    double angle = 360 * get_ratio();
                    angle = degrees_to_radians(angle);
                    int[] arr = circular_field(angle);
                    Invoke((MethodInvoker)delegate () { label1.Text = arr[0] + ":" + arr[1]; });
                    arr[0] += bitmap.Width / 2;
                    arr[1] += bitmap.Height / 2;
                    int[] zero = { bitmap.Width / 2, bitmap.Height / 2 };
                    int[] circ = { (int)(Math.Sin(angle) * 100)+bitmap.Width/2, (int)(Math.Cos(angle) * 100)+bitmap.Height/2};
                    Invoke((MethodInvoker)delegate () { draw_rect(circ, 2); });
                    //this.bitmap.SetPixel(arr[0], arr[1], Color.Black);
                    Invoke((MethodInvoker)delegate () { Line(zero,arr, 5); });
                    Invoke((MethodInvoker)delegate () { pictureBox1.Image = this.bitmap; });
                    }
                }
                catch(Exception exception) {
                    MessageBox.Show(exception.Message);
                }
            });
            t.Start();
        }
        public double get_ratio()
        {
            DateTime time = DateTime.Now;
            TimeSpan t = time - this.time;
            return (t.TotalSeconds%CIRCLE_TIME) / CIRCLE_TIME;

        }
        /// <summary>
        /// Returns 2D vector of Circular Magnetic Field
        /// </summary>
        /// <param name="angle">The angle of the object relative to the path(degrees)</param>
        /// <returns>integer array that represents the vector of the field for the specified angle</returns>
        public int[] circular_field(double angle)
        {
            //angle = degrees_to_radians(angle);
            /*int l = 1;
            double a = Math.Sqrt(1 / (Math.Pow(Math.Cos(angle), 2)) -1);
            double x = Math.Sqrt(Math.Pow(a, 2) / (1 + Math.Pow(a, 2)))*10;
            double y = Math.Sqrt(1 / (1 + Math.Pow(a, 2)))*10;
            int[] arr = { (int)x, (int)y };*/
            //int x = (int)Math.Round(Math.Sin(angle) * 140);
            //int y = (int)Math.Round(Math.Cos(angle) * 140);
            double a = Math.Sin(angle);
            double b = Math.Cos(angle);
            double x, y;
            /*a *= 100;
            b *= 100;*/
            if(b!=0)
            {
                x = 1 * (b / Math.Abs(b));
                y = a / b;
                double length = Math.Sqrt(x * x + y * y);
                x = x / length;
                y = y / length;
                y = y * 100;
                x = x * 100;
            }
            else
            {
                x = 0;
                y = -a;
            }
            int[] arr = { (int)Math.Round(x), (int)Math.Round(y) };
            return arr;
           /* arr = { (int)Math.Round(Math.Sin(angle)*140) , (int)Math.Round(Math.Cos(angle)*140) };
            return arr;*/

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public int[] DePolarField(double angle)
        {
            //TODO : fix function to work
            return circular_field(angle * 2);
        }
        public double degrees_to_radians(double degrees)
        {
            return degrees*Math.PI/180;
        }
        public void draw_circle(int thickness)
        {

        }
        public void draw_rect(int[] location,int size)
        {
            for(int i=location[0]-size;i<location[0]+size;i++)
            {
                for(int j=location[1]-size;j<location[1]+size;j++)
                {
                    this.bitmap.SetPixel(i, j,Color.Black);
                }
            }

        }
        public void Line(int[] start,int[] end,int size)
        {
            
            if (end[0] == start[0])
            {
                for(int i=start[1];i<=end[1];i++)
                {
                    int[] location = { start[0], i };
                    draw_rect(location, 2);
                }
            }
            else
            {
                int slowpe = (end[1] - start[1]) / (end[0] - start[0]);
                for(int i=start[0];i<=end[0];i++)
                {
                    int[] location = { i, start[1] + slowpe * (i-start[0]) };
                    draw_rect(location, 2);
                }
            }
        }
    }
}
