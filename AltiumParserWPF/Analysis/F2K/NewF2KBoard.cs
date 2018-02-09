using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltiumParserWPF.AltiumParser.Records;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis.F2K
{
    class NewF2KBoard : PCB
    {
        public List<Component> Sockets;

        public NewF2KBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            ActiveChanels = new List<Chanel>();

            Sockets = new List<Component>();

            GetSockets();
        }

        private void GetSockets()
        {
            var templist = new List<Component>();

            foreach (var component in Board.Components)
            {
                if (component.DesignItemId.Contains("FORMULA-256") && component.CurrentPartId < 17)
                {
                    templist.Add(component);
                }
            }

            templist = templist.OrderBy(x => x.CurrentPartId).ToList();
        }

        private void GetActiveChanels()
        {
            
        }
    }
}
