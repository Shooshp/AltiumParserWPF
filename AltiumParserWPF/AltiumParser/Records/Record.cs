using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AltiumParserWPF.AltiumParser.Records
{
    public abstract class Record
    {
        public bool IsConnectable;
        public int Id;
        public string RecordString;
        public List<string> Parameters;

        protected void AllocateValues(object child)
        {
            var fields = this.GetType().GetFields(System.Reflection.BindingFlags.Public
                                                  | System.Reflection.BindingFlags.Instance
                                                  | System.Reflection.BindingFlags.DeclaredOnly);

            foreach (var parameter in Parameters)
            {
                var temp = parameter.Split(new string[] { "=" }, StringSplitOptions.None);
                var name = temp[0].Replace('.', '_');
                var value = temp[1];

                foreach (var field in fields)
                {
                    var fieldname = field.Name.ToUpper();

                    if (fieldname == name)
                    {
                        var converted = Convert.ChangeType(value, field.FieldType);
                        field.SetValue(child, converted);
                    }
                }
            }

            foreach (var field in fields)
            {
              //  Console.WriteLine(field.Name.ToUpper() + " : " + field.GetValue(child));
            }
        }

        protected void ExtractParameters()
        {
            Parameters = new List<string>();

            var strings = RecordString.Split(new string[] { "|" }, StringSplitOptions.None);
            foreach (var line in strings)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Parameters.Add(line);
                }
            }
        }

        protected void TrimRecord(string record)
        {
            var charArray = record.ToCharArray();
            var counter = 0;
            foreach (var symbol in charArray)
            {
                if (symbol == '\0' ) 
                {
                    RecordString = new string(charArray.Take(counter).ToArray());
                    break;
                }

                counter++;
            }
        }
    }
}
