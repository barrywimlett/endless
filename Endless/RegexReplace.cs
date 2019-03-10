using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Endless
{
    public class RegexReplace 
    {
        public string FindPattern { get; set; }
        public string ReplacePattern { get; set; }
        private Regex regex;

        public RegexReplace(string find, string replace)
        {
            FindPattern = find;
            ReplacePattern = replace;
            regex = new Regex(FindPattern, RegexOptions.IgnoreCase);
                        
        }

        public string Replace(string str)
        {
            if (regex.IsMatch(str))
            {
                var newsql = regex.Replace(str, ReplacePattern);

                if (string.Compare(newsql, str) != 0)
                {
                    str = newsql;
                }

            }
            return str;
        }

        public static string Replace(string str,IEnumerable<RegexReplace> regexes )
        {
            if (regexes != null)
            {
                foreach (var r in regexes)
                {
                    str = r.Replace(str);
                }

            }
            return str;
        }

        public static string Replace(string str, string find,string replace)
        {
            RegexReplace repl=new RegexReplace(find,replace);
            str = repl.Replace(str);
              
            return str;
        }
    }
}