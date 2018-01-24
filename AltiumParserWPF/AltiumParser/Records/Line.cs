using System;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Line
    {
        public double X;
        public double Y;

        public Line(int x, int xfrac, int y, int yfrac)
        {
            var tempx = x + "." + xfrac;
            var tempy = y + "." + yfrac;

            X = Double.Parse(tempx);
            Y = Double.Parse(tempy);
        }
    }
}
