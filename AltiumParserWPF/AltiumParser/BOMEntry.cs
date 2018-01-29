using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser
{
    public class BOMEntry
    {
        public string DeviceType;
        public List<string> Designators;

        public BOMEntry(string deviceType, string designator)
        {
            Designators = new List<string>();
            DeviceType = deviceType;
            Designators.Add(designator);
        }

        public override string ToString()
        {
            var line = DeviceType + " [" + Designators.Count + "]: ";

            var counter = 1;
            foreach (var designator in Designators)
            {
                line += designator;
                if (counter != Designators.Count)
                {
                    line += ", ";
                }

                counter++;
            }
            return line;
        }
    }
}
