using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace _2B2T_Queue_Notifier
{
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