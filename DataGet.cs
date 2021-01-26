using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace dataGet
{
    internal class DataGet
    {
        static public List<int> getIndex(string path, string chat)
        {
            var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            UTF8Encoding data = new UTF8Encoding(true);
            Regex rg = new Regex(@"\[CHAT\] " + chat);
            List<int> allInt = new List<int>();
            byte[] b = new byte[64000];
            string allText = "";
            List<int> allTimeList = new List<int>();
            int totalTime;

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
                        totalTime = 0;
                        string[] textSub = textIndex.Substring(1, 8).Split(':');
                        totalTime += int.Parse(textSub[0]) * 60 * 60;
                        totalTime += int.Parse(textSub[1]) * 60;
                        totalTime += int.Parse(textSub[2]);
                        allTimeList.Add(totalTime);
                    }
                }
            }
            List<int> Output = new List<int>();
            Output.Add(allInt[allInt.Count - 1]);
            Output.Add(allTimeList[allTimeList.Count - 1]);;
            return Output;
        }

        static public void discordWebHook(String WebHook, String Queue, int Cach, String Color)
        {
            if (int.Parse(Queue) != Cach)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebHook);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"**Queue Posision:** `" + Queue.ToString()
                        + "`\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":"+Color
                        +",\"footer\": {\"text\": \"10:49 • 01/25/2021\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                    MessageBox.Show(json);

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }
    }
}