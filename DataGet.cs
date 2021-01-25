using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace dataGet
{
    internal class DataGet
    {
        static public int getIndex(string path, string chat)
        {
            var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            UTF8Encoding data = new UTF8Encoding(true);
            Regex rg = new Regex(@"\[CHAT\] " + chat);
            List<int> allInt = new List<int>();
            byte[] b = new byte[64000];
            string allText = "";

            while (text.Read(b, 0, b.Length) > 0)
            {
                allText = data.GetString(b);
                string[] textArray = allText.Split("\n");
                foreach (string textIndex in textArray)
                {
                    Match match = rg.Match(textIndex);
                    if (match.Success)
                    {
                        allInt.Add(int.Parse(textIndex.Split(chat)[1]));
                    }
                }
            }
            return allInt[allInt.Count - 1];
        }
    }
}