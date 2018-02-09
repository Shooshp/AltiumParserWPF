using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett
{
    [DebuggerDisplay("Chanel: {ChanelName}, ConnectionName: {ConnectionName}")]
    public class Chanel
    {
        public string ChanelName { get; set; }
        public string ConnectionName { get; set; }

        public List<string> ConnectedObjects { get; set; }

        public string Elements
        {
            get
            {
                if (ConnectedObjects.Count == 1)
                {
                    return ConnectedObjects.ElementAt(0);
                }
                else
                {
                    var counter = 1;
                    var line = "";
                    foreach (var connectedObject in ConnectedObjects)
                    {
                        line += connectedObject;
                        if (counter != ConnectedObjects.Count)
                        {
                            line += ", ";
                        }
                        counter++;
                    }

                    return line;
                }
            }
        }


        public Chanel(string name, string connection)
        {
            ChanelName = name;
            ConnectionName = connection;
            ConnectedObjects = new List<string>();
        }
    }
}
