using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.Analysis
{
    public static class TextAnalysis
    {
        public static string GetCommonInStrings(string a, string b)
        {
            var results = new List<string>();

            string shortest;
            string longest;

            if (a.Length > b.Length)
            {
                longest = a;
                shortest = b;
            }
            else
            {
                longest = b;
                shortest = a;
            }

            for (int startindex = 0; startindex < shortest.Length; startindex++)
            {
                for (int length = 0; length < (shortest.Length-startindex); length++)
                {
                    var tempword = shortest.Substring(startindex, length);

                    if (longest.Contains(tempword))
                    {
                        results.Add(tempword);
                    }
                }
            }

            results = results.OrderByDescending(s => s.Length).ToList();

            return results.ElementAt(0);
        }

        public static string GetCommonInListOfStrings(List<string> list)
        {
            var commondeterminators = new List<string>();

            foreach (var entry in list)
            {
                foreach (var otherentry in list)
                {
                    if (entry != otherentry)
                    {
                        commondeterminators.Add(GetCommonInStrings(entry, otherentry));
                    }
                }
            }

            var result = from item in commondeterminators
                group item by item
                into groupe
                let count = groupe.Count()
                orderby count descending
                select new {Value = groupe.Key, Count = count};

            var common = result.ElementAt(0).Value;

            if (common.StartsWith("_"))
            {
                common = common.TrimStart('_');
            }

            if (common.EndsWith("_"))
            {
                common = common.TrimEnd('_');
            }

            if (common.Length >= 2)
            {
                return common + "S";
            }
            else
            {
                return "NEW";
            }
        }
    }
}
