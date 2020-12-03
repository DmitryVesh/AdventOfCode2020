using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    public static class ReadInputFile
    {
        public static List<string> GetInputAsLines(string path)
        {
            List<string> entries = new List<string>();
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line != null)
                    entries.Add(line);
                else
                    break;
            }
            return entries;
        }
    }
}
