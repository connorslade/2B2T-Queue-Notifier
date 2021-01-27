using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
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
            byte[] b = new byte[640000];
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
            try
            {
                Output.Add(allInt[allInt.Count - 1]);
                Output.Add(allTimeList[allTimeList.Count - 1]);
            } catch { }
            return Output;
        }

        static public int ChatTime(string path)
        {
            var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            UTF8Encoding data = new UTF8Encoding(true);
            Regex rg = new Regex(@"\[CHAT\] ");
            byte[] b = new byte[640000];
            string allText = "";
            int totalTime = 0;

            while (text.Read(b, 0, b.Length) > 0)
            {
                allText = data.GetString(b);
                string[] textArray = allText.Split("\n");
                foreach (string textIndex in textArray)
                {
                    Match match = rg.Match(textIndex);
                    if (match.Success)
                    {
                        totalTime = 0;
                        string[] textSub = textIndex.Substring(1, 8).Split(':');
                        totalTime += int.Parse(textSub[0]) * 60 * 60;
                        totalTime += int.Parse(textSub[1]) * 60;
                        totalTime += int.Parse(textSub[2]);
                    }
                }
            }
            return totalTime;
        }

        static public int NowTime()
        {
            string S = DateTime.Now.ToString("ss");
            string M = DateTime.Now.ToString("mm");
            string H = DateTime.Now.ToString("HH");
            int totalTime = 0;
            totalTime += int.Parse(H) * 60 * 60;
            totalTime += int.Parse(M) * 60;
            totalTime += int.Parse(S);
          
            return totalTime;
        }

        static public void discordWebHook(String WebHook, String Queue, int Cach, String Color, bool enabled)
        {
            if (int.Parse(Queue) != Cach && enabled)
            {
                string time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebHook);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"**Queue Posision:** `" + Queue.ToString()
                        + "`\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":"+Color
                        +",\"footer\": {\"text\": \""+time+"\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }

        static public void DiscordMessage(String WebHook, String text, String Color, bool enabled)
        {
            if (enabled)
            {
                string time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebHook);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"" + text.ToString()
                        + "\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":" + Color
                        + ",\"footer\": {\"text\": \"" + time + "\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
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



internal class IniFile
    {
        private string Path;
        private string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}