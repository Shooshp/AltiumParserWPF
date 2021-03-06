﻿namespace AltiumParserWPF.AltiumParser.Records
{
    public class SheetFile : Record
    {
        public int OwnerIndex;
        public int IndexInSheet;
        public int OwnerpartId;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int FontId;
        public string Text;

        public SheetFile(string record)
        {
            IsConnectable = false;

            IsConnectable = false;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            Connection = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
        }
    }
}
