using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser
{
    public class SubPart
    {
        public List<string> Names;
        public AltiumParser SubParser;

        public SubPart(string filepath, string name)
        {
            Names = new List<string>();
            Names.Add(name);
            SubParser = new AltiumParser(filepath);
        }

    }
}
