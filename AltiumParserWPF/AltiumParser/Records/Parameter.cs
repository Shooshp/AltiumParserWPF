using System.Diagnostics;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Name:{Name}, Text:{Text}")]
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
