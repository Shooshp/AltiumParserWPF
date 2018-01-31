using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.Analysis.Ett
{
    public class Chanel
    {
        public string Name;
        public string Connection;
        public Directions Direction;

        public Chanel(string name, string connection)
        {
            Name = name;
            Connection = connection;
            Direction = Directions.Unknown;
        }

        public enum Directions
        {
            Input, Output, Biderectional, Unknown
        }
    }
}
