using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltiumParserWPF.Analysis.Ett.CodeMaker;

namespace AltiumParserWPF.Analysis.Ett
{
    public class EttPostProcessor
    {
        public List<ConnectionUnion> Unions;
        public List<EttFunctions> Functions;

        public EttPostProcessor(List<ConnectionUnion> unions)
        {
            Unions = new List<ConnectionUnion>();
            Functions = new List<EttFunctions>();
            Unions = unions;

            foreach (var union in Unions)
            {
                Functions.Add(new EttFunctions(union));
            }

            foreach (var function in Functions)
            {
                foreach (var line in function.WorkingRutines)
                {
                    Console.WriteLine(line);
                }
            }
        }

    }
}
