using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace _2B2T_Queue_Notifier
{
    internal static class DataGet
    {
        public static List<int> GetIndex(string path, string chat)
        {
            try
            {
                var text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var data = new UTF8Encoding(true);
                var rg = new Regex(@"\[CHAT\] " + chat);
                var allInt = new List<int>();
                var b = new byte[640000];
                var allTimeList = new List<int>();

                while (text.Read(b, 0, b.Length) > 0)
                {
                    var allText = data.GetString(b);
                    var textArray = allText.Split("\n");
                    foreach (var textIndex in textArray)
                    {
                        var match = rg.Match(textIndex);
                        if (!match.Success)
                            continue;
                        allInt.Add(int.Parse(textIndex.Split(chat)[1]));
                        var totalTime = 0;
                        var textSub = textIndex.Substring(1, 8).Split(':');
                        totalTime += int.Parse(textSub[0]) * 60 * 60;
                        totalTime += int.Parse(textSub[1]) * 60;
                        totalTime += int.Parse(textSub[2]);
                        allTimeList.Add(totalTime);
                    }
                }
                var output = new List<int>();
                try
                {
                    output.Add(allInt[^1]);
                    output.Add(allTimeList[^1]);
                } catch
                {
                    // ignored
                }
                return output;
            } catch
            {
                var output = new List<int> {0, 0};
                return output;
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
                var totalTime = 0;

                while (text.Read(b, 0, b.Length) > 0)
                {
                    var allText = data.GetString(b);
                    var textArray = allText.Split("\n");
                    foreach (var textIndex in textArray)
                    {
                        var match = rg.Match(textIndex);
                        if (!match.Success)
                            continue;
                        totalTime = 0;
                        var textSub = textIndex.Substring(1, 8).Split(':');
                        totalTime += int.Parse(textSub[0]) * 60 * 60;
                        totalTime += int.Parse(textSub[1]) * 60;
                        totalTime += int.Parse(textSub[2]);
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
            var s = DateTime.Now.ToString("ss");
            var m = DateTime.Now.ToString("mm");
            var h = DateTime.Now.ToString("HH");
            var totalTime = int.Parse(h) * 60 * 60;
            totalTime += int.Parse(m) * 60;
            totalTime += int.Parse(s);

            return totalTime;
        }

        public static bool DiscordWebHook(string WebHook, string Queue, int Cach, string Color, bool enabled, bool doMention, string whomnt)
        {
            if (int.Parse(Queue) == Cach || !enabled)
                return true;
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
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    streamReader.ReadToEnd();
                }
                return true;
            } catch { return false; }
        }

        public static bool DiscordMessage(string WebHook, string text, string Color, bool enabled, bool doMention, string whomnt)
        {
            if (!enabled)
                return true;
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
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    streamReader.ReadToEnd();
                }
                return true;
            } catch { return false; }
        }
    }


    internal class IniFile
    {
        private readonly string exe = Assembly.GetExecutingAssembly().GetName().Name;
        private readonly string path;

        public IniFile(string IniPath = null)
        {
            path = new FileInfo(IniPath ?? exe + ".ini").FullName;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public string Read(string Key, string Section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? exe, Key, "", retVal, 255, path);
            return retVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? exe, Key, Value, path);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}