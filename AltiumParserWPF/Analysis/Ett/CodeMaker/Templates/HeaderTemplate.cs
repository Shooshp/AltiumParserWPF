using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class HeaderTemplate
    {
        public List<string> Code;
        public string Name;

        public HeaderTemplate(string name)
        {
            Code = new List<string>();
            Name = name;
            AddIfNDef(name.ToUpper()+"_H_");
            AddDefine(name.ToUpper()+"_H_");
        }

        public void AddNameSpace(string name)
        {
            Code.Add("using namespace " + name + ";");
        }

        public void AddDefine(string name, string value)
        {
            Code.Add("#define " + name + " " + value);
        }

        public void AddDefine(string name)
        {
            Code.Add("#define " + name);
        }

        public void AddIfNDef(string name)
        {
            Code.Add("#ifndef " + name);
        }

        public void Endif()
        {
            Code.Add("#endif");
        }

        public void AddInclude(string include)
        {
            var line = "#include ";
            if (!include.Contains("<"))
            {
                line += "\"" + include + "\"";
            }
            else
            {
                line += include;
            }
            Code.Add(include);
        }

        public void AddBlankLine()
        {
            Code.Add("");
        }
    }
}
