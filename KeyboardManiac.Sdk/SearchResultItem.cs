using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardManiac.Sdk
{
    public class SearchResultItem
    {
        public SearchResultItem(string name, string resultType, string path)
        {
            Name = name;
            ResultType = resultType;
            Path = path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string ResultType { get; set; }
        public int Score { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2})", ResultType, Name, Score);
        }
    }
}
