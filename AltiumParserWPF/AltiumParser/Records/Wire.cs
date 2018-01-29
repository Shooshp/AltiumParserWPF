using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Wire: {UniqueId}, Dots: {WireDotList.Count}")]
    public class Wire : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int LineWidth;
        public string UniqueId;
        public int LocationCount;

        public List<Dot> WireDotList;

        public Wire(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            WireDotList = new List<Dot>();
            AddDots();
        }

        private void AddDots()
        {
            if (LocationCount != 0)
            {
                for (var i = 1; i < LocationCount + 1; i++)
                {
                    var searchx = new StringBuilder("X" + i).ToString();
                    var searchxfrac = new StringBuilder("X" + i + "_FRAC").ToString();
                    var searchy = new StringBuilder("Y" + i).ToString();
                    var searchyfrac = new StringBuilder("Y" + i + "_FRAC").ToString();

                    var tempx = -1;
                    var tempxfrac = 0;
                    var tempy = -1;
                    var tempyfrac = 0;

                    foreach (var parameter in Parameters)
                    {
                        if (parameter.Contains("="))
                        {
                            var temp = parameter.Split(new string[] { "=" }, StringSplitOptions.None);
                            var name = temp[0];
                            var value = temp[1];

                            if (name == searchx)
                            {
                                tempx = Convert.ToInt32(value);
                            }
                            if (name == searchxfrac)
                            {
                                tempxfrac = Convert.ToInt32(value);
                            }
                            if (name == searchy)
                            {
                                tempy = Convert.ToInt32(value);
                            }
                            if (name == searchyfrac)
                            {
                                tempyfrac = Convert.ToInt32(value);
                            }
                        }
                    }

                    if (tempx != -1 && tempy != -1) 
                    {
                        WireDotList.Add(new Dot(tempx, tempxfrac, tempy, tempyfrac));
                    }
                }
            }
        }

        public bool DotIsOnLine(Dot dot)
        {
            var ChekDot = new Point(dot.X, dot.Y);

            for (int i = 0; i < WireDotList.Count - 1; i++)
            {
                var A = WireDotList.ElementAt(i);
                var B = WireDotList.ElementAt(i + 1);

                
                var start = new Point(A.X, A.Y);
                var end = new Point(B.X, B.Y);

                var line = new LineGeometry(start, end);

                if (line.FillContains(ChekDot, 2, ToleranceType.Absolute))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsDot(Dot dot)
        {
            foreach (var wiredot in WireDotList)
            {
                if (wiredot.IsMatch(dot))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
