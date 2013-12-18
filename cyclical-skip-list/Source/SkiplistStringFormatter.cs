using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistStringFormatter
    {
        public static string ConvertToString<T>(INode<T> start)
        {
            var levelFormatString = CreateFormatString(start);
            var headsOfLevels = SkiplistUtilities.EnumerateLevels(start);

            string outputString = "";
            foreach (var head in headsOfLevels)
            {
                outputString = outputString + ConvertLevelToString(head, levelFormatString);
            }

            return outputString.Replace(" ", "-");
        }

        private static string CreateFormatString<T>(INode<T> head)
        {
            var headOfLowestLevel = head.Bottom();
            var keys = SkiplistUtilities.EnumerateKeysInLevel(headOfLowestLevel).ToList();

            var numberOfColumns = keys.Count();
            var columnWidth = keys.Max(key => key.ToString().Length) + 1;

            var formatStrings = Enumerable.Range(0, numberOfColumns).Select(i => "{" + i + ",-" + (columnWidth) + "}").ToArray();
            var levelFormatString = string.Concat(formatStrings);

            return levelFormatString;
        }

        private static string ConvertLevelToString<T>(INode<T> head, string formatString)
        {
            var nodes = SkiplistUtilities.EnumerateNodesInLevel(head).ToList();
            var keys = nodes.ToDictionary(node => node, node => node.Key);
            var spacings = nodes.ToDictionary(node => node, node => node.SizeOfBaseGapTo(node.Right) - 1);

            var valuesForThisLevel = new List<string>();
            foreach (var node in nodes)
            {
                valuesForThisLevel.Add(keys[node].ToString());
                valuesForThisLevel.AddRange(Enumerable.Repeat("", spacings[node]));
            }

            var stringForThisLevel = String.Format(formatString, valuesForThisLevel.ToArray());

            return stringForThisLevel + "\n";
        }
    }
}
