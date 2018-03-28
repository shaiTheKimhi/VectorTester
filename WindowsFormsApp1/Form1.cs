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
        Color[] colors = { Color.Black, Color.Green, Color.Red, Color.LightBlue, Color.Purple, Color.Yellow };
        const int CIRCLE_TIME = 1 * 60;
        const int FIELD_SIZE = 75;
        Bitmap bitmap;
        DateTime time;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
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

                        int[] arr = circular_field(angle);
                        arr[0] += bitmap.Width / 2;
                        arr[1] += bitmap.Height / 2;
                        //MessageBox.Show(arr[0].ToString() + ":" + arr[1].ToString());
                        this.bitmap.SetPixel(arr[0], arr[1], Color.Black);
                        Invoke((MethodInvoker)delegate () { draw_rect(arr, 5); });
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
            angle = degrees_to_radians(angle);
            /*int l = 1;
            double a = Math.Sqrt(1 / (Math.Pow(Math.Cos(angle), 2)) -1);
            double x = Math.Sqrt(Math.Pow(a, 2) / (1 + Math.Pow(a, 2)))*10;
            double y = Math.Sqrt(1 / (1 + Math.Pow(a, 2)))*10;
            int[] arr = { (int)x, (int)y };*/
            //int x = (int)Math.Round(Math.Sin(angle) * 140);
            //int y = (int)Math.Round(Math.Cos(angle) * 140);
            double a = Math.Sin(angle);
            double b = Math.Cos(angle);
           /* double x, y;
            if(b!=0)

            {
                x = Math.Sqrt(FIELD_SIZE / ((a * a) / (b * b) + 1));
                y = -1 * a * x / b;

            }
            else
            {
                x = 0;
                y = a * FIELD_SIZE;
            }
            int[] arr = { (int)Math.Round(x), (int)Math.Round(y) };
            return arr;*/
            int[] arr = { (int)Math.Round(Math.Sin(angle)*140) , (int)Math.Round(Math.Cos(angle)*140) };
            return arr;

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
            Color c;
            TimeSpan ti = DateTime.Now - time;
            c = this.colors[(int)(ti.TotalSeconds / CIRCLE_TIME) % this.colors.Length];
            for(int i=location[0]-size;i<location[0]+size;i++)
            {
                for(int j=location[1]-size;j<location[1]+size;j++)
                {
                    this.bitmap.SetPixel(i, j,c);
                }
            }

        }
    }
}
