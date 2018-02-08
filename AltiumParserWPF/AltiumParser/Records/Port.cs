using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Port: {Name} Srtyle: {Style}")]
    public class Port : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int Style;
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

        public float Offset;
        public Dot FirstDot;
        public Dot SecondDot;


        public Port(string record)
        {
            ConnectionsDots = new List<Dot>();

            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            var temp = Width + "." + Width_Frac;

            Offset = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);

            FirstDot = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);

            if (Style == 0 || Style == 1 || Style == 2 || Style == 3 )
            {
                SecondDot = new Dot(FirstDot.X + Offset, FirstDot.Y);
            }

            if (Style == 4 || Style == 5 || Style == 6 || Style == 7)
            {
                SecondDot = new Dot(FirstDot.X, FirstDot.Y + Offset);
            }
            
            ConnectionsDots.Add(FirstDot);
            ConnectionsDots.Add(SecondDot);
        } 
    }
}
