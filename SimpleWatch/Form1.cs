using Microsoft.VisualBasic.Devices;
using System.Drawing.Drawing2D;

namespace SimpleWatch
{
    public partial class Form1 : Form
    {
        Graphics g;
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            g = e.Graphics;
            DrawClock(g);
        }
        private void DrawClock(Graphics g)
        {
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            int clockRadius = Math.Min(centerX, centerY) - 20;

            // Draw clock circle
            g.DrawEllipse(Pens.Black, centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);

            // Create Font Family and String Format and Graphic Path
            GraphicsPath graphicsPath = new GraphicsPath();
            FontFamily font = new FontFamily("Arial");
            StringFormat stringFormat = new StringFormat();

            // Draw Clock Numbers
            graphicsPath.AddString("12", font, 0, 30, new Point(centerX - 16, centerY - 16 - (clockRadius - 16)) , stringFormat);
            for (int i = 0; i < 12; i++)
            {
                if(i + 3 == 12)
                {
                    continue;
                }
                double Angel = (i % 12 * Math.PI / 6);
                graphicsPath.AddString(((i + 3) % 12).ToString(), font, 0, 30, new Point((int)(centerX - 16 + ((clockRadius - 16) * Math.Cos(Angel))), (int)(centerY - 16 + ((clockRadius - 16) * Math.Sin(Angel)))), stringFormat);
            }

            g.DrawPath(new Pen(Color.Black), graphicsPath);

            // Get the current time
            DateTime now = DateTime.Now;
            float hour = now.Hour % 12 + now.Minute / 60f;
            float minute = now.Minute + now.Second / 60f;
            float second = now.Second;

            // Calculate angles for each hand
            float secondAngle = (second / 60) * 360;
            float minuteAngle = (minute / 60) * 360;
            float hourAngle = (hour / 12) * 360;

            // Draw the hands
            DrawHand(g, centerX, centerY, hourAngle, clockRadius * 0.5f, Pens.Black, 6);
            DrawHand(g, centerX, centerY, minuteAngle, clockRadius * 0.7f, Pens.Blue, 4);
            DrawHand(g, centerX, centerY, secondAngle, clockRadius * 0.9f, Pens.Red, 2);
        }

        private void DrawHand(Graphics g, int cx, int cy, float angle, float length, Pen pen, int thickness)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TranslateTransform(cx, cy);
            g.RotateTransform(angle - 90); // Rotate the coordinate system

            // Draw the hand line
            g.DrawLine(new Pen(pen.Color, thickness), 0, 0, length, 0);

            g.ResetTransform(); // Reset transformation
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
