using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Port: {Name}")]
    public class Port : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int Alignment;
        public int Width;
        public int Width_Frac;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int FontId;
        public int AreaColor;
        public string Name;
        public string UniqueId;
        public int Height;
        public List<Dot> ConnectionsDots;
        public float XOffset;

        public Port(string record)
        {
            ConnectionsDots = new List<Dot>();

            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            var temp = Width + "." + Width_Frac;

            XOffset = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);

            var FirstDot = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
            var SecondDot = new Dot(FirstDot.X + XOffset, FirstDot.Y);
            
            ConnectionsDots.Add(FirstDot);
            ConnectionsDots.Add(SecondDot);
        }
    }
}
