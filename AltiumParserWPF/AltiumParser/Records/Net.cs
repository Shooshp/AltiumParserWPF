﻿using System.Diagnostics;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Net Name:{Text}, X {Connection.X} Y {Connection.Y}")]
    public class Net : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int Color;
        public int FontId;
        public string Text;
        public string UniqueId;

        public Net(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            Connection = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
        }
    }
}
