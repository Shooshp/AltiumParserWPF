using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis
{
    public abstract class PCB
    {
        public AltiumParser.AltiumParser Board;
        public List<Chanel> Chanels;
        public List<ConnectionUnion> Connections;
    }
}
