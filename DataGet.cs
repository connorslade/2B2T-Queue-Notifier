using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace dataGet
{
    internal class DataGet
    {
        public static List<int> getIndex(string path, string chat)
        {
            try
            {
                var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var data = new UTF8Encoding(true);
                var rg = new Regex(@"\[CHAT\] " + chat);
                var allInt = new List<int>();
                var b = new byte[640000];
                var allText = "";
                var allTimeList = new List<int>();
                int totalTime;

                while (text.Read(b, 0, b.Length) > 0)
                {
                    allText = data.GetString(b);
                    var textArray = allText.Split("\n");
                    foreach (var textIndex in textArray)
                    {
                        var match = rg.Match(textIndex);
                        if (match.Success)
                        {
                            allInt.Add(int.Parse(textIndex.Split(chat)[1]));
                            totalTime = 0;
                            var textSub = textIndex.Substring(1, 8).Split(':');
                            totalTime += int.Parse(textSub[0]) * 60 * 60;
                            totalTime += int.Parse(textSub[1]) * 60;
                            totalTime += int.Parse(textSub[2]);
                            allTimeList.Add(totalTime);
                        }
                    }
                }
                var Output = new List<int>();
                try
                {
                    Output.Add(allInt[allInt.Count - 1]);
                    Output.Add(allTimeList[allTimeList.Count - 1]);
                } catch { }
                return Output;
            } catch
            {
                var Output = new List<int>();
                Output.Add(0);
                Output.Add(0);
                return Output;
            }
        }

        public static int ChatTime(string path)
        {
            try
            {
                var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var data = new UTF8Encoding(true);
                var rg = new Regex(@"\[CHAT\] ");
                var b = new byte[640000];
                var allText = "";
                var totalTime = 0;

                while (text.Read(b, 0, b.Length) > 0)
                {
                    allText = data.GetString(b);
                    var textArray = allText.Split("\n");
                    foreach (var textIndex in textArray)
                    {
                        var match = rg.Match(textIndex);
                        if (match.Success)
                        {
                            totalTime = 0;
                            var textSub = textIndex.Substring(1, 8).Split(':');
                            totalTime += int.Parse(textSub[0]) * 60 * 60;
                            totalTime += int.Parse(textSub[1]) * 60;
                            totalTime += int.Parse(textSub[2]);
                        }
                    }
                }
                return totalTime;
            } catch
            {
                return 0;
            }
        }

        public static int NowTime()
        {
            var S = DateTime.Now.ToString("ss");
            var M = DateTime.Now.ToString("mm");
            var H = DateTime.Now.ToString("HH");
            var totalTime = 0;
            totalTime += int.Parse(H) * 60 * 60;
            totalTime += int.Parse(M) * 60;
            totalTime += int.Parse(S);

            return totalTime;
        }

        public static bool discordWebHook(string WebHook, string Queue, int Cach, string Color, bool enabled, bool doMention, string whomnt)
        {
            if (int.Parse(Queue) != Cach && enabled)
                try
                {
                    var mnt = "";
                    if (doMention)
                        mnt = whomnt;
                    var time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(WebHook);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"**Queue Posision:** `" + Queue
                                                                                                                                                  + "`\\n" + mnt +
                                                                                                                                                  "\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":" +
                                                                                                                                                  Color
                                                                                                                                                  + ",\"footer\": {\"text\": \"" +
                                                                                                                                                  time +
                                                                                                                                                  "\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                        streamWriter.Write(json);
                    }

                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                    return true;
                } catch { return false; }
            return true;
        }

        public static bool DiscordMessage(string WebHook, string text, string Color, bool enabled, bool doMention, string whomnt)
        {
            if (enabled)
                try
                {
                    var mnt = "";
                    if (doMention)
                        mnt = whomnt;
                    var time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(WebHook);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"" + text
                                                                                                                             + "\\n" + mnt +
                                                                                                                             "\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":" +
                                                                                                                             Color
                                                                                                                             + ",\"footer\": {\"text\": \"" + time +
                                                                                                                             "\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                        streamWriter.Write(json);
                    }

                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                    return true;
                } catch { return false; }
            return true;
        }
    }


    internal class IniFile
    {
        private readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name;
        private readonly string Path;

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

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