using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Designator : Record
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
        public string Name;
        public int ReadOnlyState;
        public string UniqueId;

        public Designator(string record)
        {
            IsConnectable = false;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);
        }
    }
}
