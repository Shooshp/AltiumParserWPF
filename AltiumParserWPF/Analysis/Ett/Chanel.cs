using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett
{
    public class Chanel
    {
        public string ChanelName;
        public string ConnectionName;
        public List<string> ConnectedObjects;

        public Chanel(string name, string connection)
        {
            ChanelName = name;
            ConnectionName = connection;
            ConnectedObjects = new List<string>();
        }
    }
}
