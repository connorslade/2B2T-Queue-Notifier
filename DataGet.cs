using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

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

        static public List<int> getGameTime(string path)
        {
            var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            List<List<int>> allInt = new List<List<int>>();
            UTF8Encoding data = new UTF8Encoding(true);
            List<int> workingList = new List<int>();
            List<int> allWorking = new List<int>();
            List<int> Output = new List<int>();
            byte[] b = new byte[64000];
            string allText = "";
            int index = 0;

            while (text.Read(b, 0, b.Length) > 0)
            {
                allText = data.GetString(b);
                string[] textArray = allText.Split("\n");
                foreach (string textIndex in textArray)
                {
                    string time = textIndex.Substring(1, 8);
                    foreach (string working in time.Split(":"))
                    {
                        try { allWorking.Add(int.Parse(working)); }
                        catch { }
                    }
                    allInt.Add(allWorking);
                }
            }

            foreach (int num in allInt[allInt.Count - 1])
            {
                if (index >= 3)
                {
                    workingList = new List<int>();
                    index = 0;
                }
                workingList.Add(num);
                index++;
            }

            foreach (int print in workingList)
            {
                Output.Add(print);
            }

            return Output;
        }
    }
}