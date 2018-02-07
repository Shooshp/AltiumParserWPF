using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett
{
    public abstract class PCB
    {
        public AltiumParser.AltiumParser Board;
        public List<Chanel> Chanels;
        public List<ConnectionUnion> Connections;
    }
}
