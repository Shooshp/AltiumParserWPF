﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Wire : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int LineWidth;
        public string UniqueId;
        public int LocationCount;

        public List<Line> WireLineList;

        public Wire(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            WireLineList = new List<Line>();
            AddLines();
        }

        private void AddLines()
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
                    var tempxfrac = -1;
                    var tempy = -1;
                    var tempyfrac = -1;

                    foreach (var parameter in Parameters)
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

                    if (tempx != -1 && tempxfrac != -1 && tempy != -1 && tempyfrac != -1) 
                    {
                        WireLineList.Add(new Line(tempx, tempxfrac, tempy, tempyfrac));
                    }
                }
            }
        }
    }
}