using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Color_Picker
{
    class LinearGdHelperClass
    {

        //////////////
        //Calculates the color of a point in a rectangle that is filled
        //with a LearGradientBrush.
        public static Color GetColorAtPoint(Rectangle theRec, Point thePoint)
        {
            //Get properties    
            LinearGradientBrush br = (LinearGradientBrush)theRec.Fill;
            double y3 = thePoint.Y;
            double x3 = thePoint.X;
            double x1 = br.StartPoint.X * theRec.ActualWidth;
            double y1 = br.StartPoint.Y * theRec.ActualHeight;

            Point p1 = new Point(x1, y1);
            //Starting point     
            double x2 = br.EndPoint.X * theRec.ActualWidth;
            double y2 = br.EndPoint.Y * theRec.ActualHeight;
            Point p2 = new Point(x2, y2);
            //End point     
            //Calculate intersecting points     
            Point p4 = new Point();
            //with tangent     

            if (y1 == y2)
            //Horizontal case    
            {
                p4 = new Point(x3, y1);
            }
            else if (x1 == x2)
            //Vertical case    
            {
                p4 = new Point(x1, y3);
            }
            else
            //Diagnonal case    
            {
                double m = (y2 - y1) / (x2 - x1);
                double m2 = -1 / m;
                double b = y1 - m * x1;
                double c = y3 - m2 * x3;
                double x4 = (c - b) / (m - m2);
                double y4 = m * x4 + b;
                p4 = new Point(x4, y4);
            }
            //Calculate distances relative to the vector start    
            double d4 = dist(p4, p1, p2);
            double d2 = dist(p2, p1, p2);
            double x = d4 / d2;
            //Clip the input if before or after the max/min offset values    
            double max = br.GradientStops.Max(n => n.Offset);
            if (x > max)
            {

                x = max;

            }
            double min = br.GradientStops.Min(n => n.Offset);
            if (x < min)
            {
                x = min;
            }
            //Find gradient stops that surround the input value    
            GradientStop gs0 = br.GradientStops.Where(n => n.Offset <= x).OrderBy(n => n.Offset).Last();
            GradientStop gs1 = br.GradientStops.Where(n => n.Offset >= x).OrderBy(n => n.Offset).First();
            float y = 0f;
            if (gs0.Offset != gs1.Offset)
            {
                y = (float)((x - gs0.Offset) / (gs1.Offset - gs0.Offset));
            }
            //Interpolate color channels 
            Color cx = new Color();
            if (br.ColorInterpolationMode == ColorInterpolationMode.SRgbLinearInterpolation)
            {
                byte aVal = (byte)((gs1.Color.A - gs0.Color.A) * y + gs0.Color.A);
                byte rVal = (byte)((gs1.Color.R - gs0.Color.R) * y + gs0.Color.R);
                byte gVal = (byte)((gs1.Color.G - gs0.Color.G) * y + gs0.Color.G);
                byte bVal = (byte)((gs1.Color.B - gs0.Color.B) * y + gs0.Color.B);
                cx = Color.FromArgb(aVal, rVal, gVal, bVal);
            }
            return cx;
        }
        //Helper method for GetColorAtPoint//Returns the signed magnitude of a point on a vector with origin po and pointing to pfprivate 
        static double dist(Point px, Point po, Point pf)
        {
            double d = Math.Sqrt((px.Y - po.Y) * (px.Y - po.Y) + (px.X - po.X) * (px.X - po.X));
            if (((px.Y < po.Y) && (pf.Y > po.Y)) ||
                ((px.Y > po.Y) && (pf.Y < po.Y)) ||
                ((px.Y == po.Y) && (px.X < po.X) && (pf.X > po.X)) ||
                ((px.Y == po.Y) && (px.X > po.X) && (pf.X < po.X)))
            { d = -d; }
            return d;
        }

        ////////////////
    }
}
