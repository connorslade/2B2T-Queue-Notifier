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
                var Text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var Data = new UTF8Encoding(true);
                var Rg = new Regex(@"\[CHAT\] " + chat);
                var AllInt = new List<int>();
                var B = new byte[640000];
                var AllTimeList = new List<int>();

                while (Text.Read(B, 0, B.Length) > 0)
                {
                    var AllText = Data.GetString(B);
                    var TextArray = AllText.Split("\n");
                    foreach (var TextIndex in TextArray)
                    {
                        var Match = Rg.Match(TextIndex);
                        if (!Match.Success)
                            continue;
                        AllInt.Add(int.Parse(TextIndex.Split(chat)[1]));
                        var TotalTime = 0;
                        var TextSub = TextIndex.Substring(1, 8).Split(':');
                        TotalTime += int.Parse(TextSub[0]) * 60 * 60;
                        TotalTime += int.Parse(TextSub[1]) * 60;
                        TotalTime += int.Parse(TextSub[2]);
                        AllTimeList.Add(TotalTime);
                    }
                }
                var Output = new List<int>();
                try
                {
                    Output.Add(AllInt[^1]);
                    Output.Add(AllTimeList[^1]);
                } catch
                {
                    // ignored
                }
                return Output;
            } catch
            {
                var Output = new List<int> {0, 0};
                return Output;
            }
        }

        public static int ChatTime(string path)
        {
            try
            {
                var Text = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var Data = new UTF8Encoding(true);
                var Rg = new Regex(@"\[CHAT\] ");
                var B = new byte[640000];
                var TotalTime = 0;

                while (Text.Read(B, 0, B.Length) > 0)
                {
                    var AllText = Data.GetString(B);
                    var TextArray = AllText.Split("\n");
                    foreach (var TextIndex in TextArray)
                    {
                        var Match = Rg.Match(TextIndex);
                        if (!Match.Success)
                            continue;
                        TotalTime = 0;
                        var TextSub = TextIndex.Substring(1, 8).Split(':');
                        TotalTime += int.Parse(TextSub[0]) * 60 * 60;
                        TotalTime += int.Parse(TextSub[1]) * 60;
                        TotalTime += int.Parse(TextSub[2]);
                    }
                }
                return TotalTime;
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
            var TotalTime = int.Parse(H) * 60 * 60;
            TotalTime += int.Parse(M) * 60;
            TotalTime += int.Parse(S);

            return TotalTime;
        }

        public static bool DiscordWebHook(string WebHook, string Queue, int Cache, string Color, bool enabled, bool doMention, string WhoMnt)
        {
            if (int.Parse(Queue) == Cache || !enabled)
                return true;
            try
            {
                var Mnt = "";
                if (doMention)
                    Mnt = WhoMnt;
                var Time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                var HttpWebRequest = (HttpWebRequest) WebRequest.Create(WebHook);
                HttpWebRequest.ContentType = "application/json";
                HttpWebRequest.Method = "POST";

                using (var StreamWriter = new StreamWriter(HttpWebRequest.GetRequestStream()))
                {
                    var Json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"**Queue Position:** `" + Queue
                                                                                                                                              + "`\\n" + Mnt +
                                                                                                                                              "\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":" +
                                                                                                                                              Color
                                                                                                                                              + ",\"footer\": {\"text\": \"" +
                                                                                                                                              Time +
                                                                                                                                              "\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                    StreamWriter.Write(Json);
                }

                var HttpResponse = (HttpWebResponse) HttpWebRequest.GetResponse();
                using (var StreamReader = new StreamReader(HttpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    StreamReader.ReadToEnd();
                }
                return true;
            } catch { return false; }
        }

        public static bool DiscordMessage(string WebHook, string text, string Color, bool enabled, bool doMention, string WhoMnt)
        {
            if (!enabled)
                return true;
            try
            {
                var Mnt = "";
                if (doMention)
                    Mnt = WhoMnt;
                var Time = DateTime.Now.ToString("HH:mm:ss • MM/dd/yy");
                var HttpWebRequest = (HttpWebRequest) WebRequest.Create(WebHook);
                HttpWebRequest.ContentType = "application/json";
                HttpWebRequest.Method = "POST";

                using (var StreamWriter = new StreamWriter(HttpWebRequest.GetRequestStream()))
                {
                    var Json = "{\"content\":null,\"embeds\":[{\"title\":\"Σ's 2B2T Queue Notifier\",\"description\":\"" + text
                                                                                                                         + "\\n" + Mnt +
                                                                                                                         "\",\"url\":\"https://github.com/Basicprogrammer10/2B2T-Queue-Notifier\",\"color\":" +
                                                                                                                         Color
                                                                                                                         + ",\"footer\": {\"text\": \"" + Time +
                                                                                                                         "\"},\"thumbnail\":{\"url\":\"https://i.imgur.com/K1KWFjR.png\"}}]}";
                    StreamWriter.Write(Json);
                }

                var HttpResponse = (HttpWebResponse) HttpWebRequest.GetResponse();
                using (var StreamReader = new StreamReader(HttpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    StreamReader.ReadToEnd();
                }
                return true;
            } catch { return false; }
        }
    }


    internal class IniFile
    {
        private readonly string Exe = Assembly.GetExecutingAssembly().GetName().Name;
        private readonly string Path;

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? Exe + ".ini").FullName;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? Exe, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? Exe, Key, Value, Path);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}