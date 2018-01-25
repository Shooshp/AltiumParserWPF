using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Parameter : Record
    {
        public int IndexInSheet;
        public int OwnerIndex;
        public int OwnerpartId;
        public string Name;
        public string Text;

        public Parameter(string record)
        {
            IsConnectable = false;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);
        }
    }
}
