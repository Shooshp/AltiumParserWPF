using System;
using System.Globalization;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Dot
    {
        public float X;
        public float Y;

        public Dot(int x, int xfrac, int y, int yfrac)
        {
            var tempx = x + "." + xfrac;
            var tempy = y + "." + yfrac;

            X = float.Parse(tempx, CultureInfo.InvariantCulture.NumberFormat);
            Y = float.Parse(tempy, CultureInfo.InvariantCulture.NumberFormat);
        }

        public Dot(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool IsMatch(Dot dot)
        {
            var xabsdiff = Math.Abs(dot.X - X);
            var yabsdiff = Math.Abs(dot.Y - Y);
            if (xabsdiff < 0.2 && yabsdiff < 0.2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
