using System;
using System.Diagnostics;
using System.Windows;
using dataGet;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly string chat = "Position in queue: ";

        private readonly IniFile config =
            new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");

        private readonly bool doWebHooks;
        private readonly bool hooklogin;
        private readonly bool hooklogout;
        private readonly bool hookpoz;
        private readonly string LogPath = @"%AppData%\.minecraft\logs\latest.log";
        private readonly bool mntlogin;
        private readonly bool mntlogout;
        private readonly bool mntpoz;
        private int TickDelayValue = 1;
        private int timeOutValue = 30;
        private readonly string webHook = "";
        private readonly string whomnt;

        public Settings()
        {
            InitializeComponent();
            timeOutValue = int.Parse(config.Read("timeout"));
            TickDelayValue = int.Parse(config.Read("tickdelay"));
            LogPath = config.Read("logpath");
            chat = config.Read("chat");

            LogPathFeild.Text = LogPath;
            TimeoutLabel.Content = timeOutValue;
            TickLabel.Content = TickDelayValue;
            ChatRegex.Text = chat;
            //DISCORD
            webHook = config.Read("hookuri");
            whomnt = config.Read("whomnt");
            try { doWebHooks = bool.Parse(config.Read("dowebhook")); } catch { doWebHooks = false; }
            try { hookpoz = bool.Parse(config.Read("hookpoz")); } catch { hookpoz = false; }
            try { hooklogin = bool.Parse(config.Read("hooklogin")); } catch { hooklogin = false; }
            try { hooklogout = bool.Parse(config.Read("hooklogout")); } catch { hooklogout = false; }
            try { mntlogin = bool.Parse(config.Read("mntlogin")); } catch { mntlogin = false; }
            try { mntlogout = bool.Parse(config.Read("mntlogout")); } catch { mntlogout = false; }
            try { mntpoz = bool.Parse(config.Read("mntpoz")); } catch { mntpoz = false; }

            webHooksCheck.IsChecked = doWebHooks;
            WebHookUri.Text = webHook;
            LoginBox.IsChecked = hooklogin;
            LogoutBox.IsChecked = hooklogout;
            PozBox.IsChecked = hookpoz;
            LoginBox_Copy.IsChecked = mntlogin;
            LoginBox_Copy1.IsChecked = mntlogout;
            LoginBox_Copy2.IsChecked = mntpoz;
            MntFeild.Text = whomnt;
        }

        private void PlusTimeout(object sender, RoutedEventArgs e)
        {
            if (timeOutValue < 100)
                timeOutValue++;
            TimeoutLabel.Content = timeOutValue;
        }

        private void LessTimeout(object sender, RoutedEventArgs e)
        {
            if (timeOutValue > 0)
                timeOutValue--;
            TimeoutLabel.Content = timeOutValue;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlusTick(object sender, RoutedEventArgs e)
        {
            if (TickDelayValue < 10)
                TickDelayValue++;
            TickLabel.Content = TickDelayValue;
        }

        private void LessTick(object sender, RoutedEventArgs e)
        {
            if (TickDelayValue > 1)
                TickDelayValue--;
            TickLabel.Content = TickDelayValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Basicprogrammer10/2B2T-Queue-Notifier/issues",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            config.Write("timeout", timeOutValue.ToString());
            config.Write("tickdelay", TickDelayValue.ToString());
            config.Write("logpath", LogPathFeild.Text);
            config.Write("chat", ChatRegex.Text);
            config.Write("dowebhook", webHooksCheck.IsChecked.ToString());
            config.Write("hookuri", WebHookUri.Text);
            config.Write("hooklogin", LoginBox.IsChecked.ToString());
            config.Write("hooklogout", LogoutBox.IsChecked.ToString());
            config.Write("hookpoz", PozBox.IsChecked.ToString());
            config.Write("mntlogin", LoginBox_Copy.IsChecked.ToString());
            config.Write("mntlogout", LoginBox_Copy1.IsChecked.ToString());
            config.Write("mntpoz", LoginBox_Copy2.IsChecked.ToString());
            config.Write("whomnt", MntFeild.Text);
            Close();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            config.Write("setup", "true");
            config.Write("timeout", "30");
            config.Write("tickdelay", "1");
            config.Write("chat", "Position in queue: ");
            config.Write("logpath", @"%AppData%\.minecraft\logs\latest.log");
            config.Write("dowebhook", "false");
            config.Write("hooklogin", "true");
            config.Write("hooklogout", "true");
            config.Write("hookpoz", "true");
            config.Write("hookuri", "");
            config.Write("mntlogin", "false");
            config.Write("mntlogout", "false");
            config.Write("mntpoz", "false");
            config.Write("whomnt", "@everyone");
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Basicprogrammer10/2B2T-Queue-Notifier/blob/master/Settings.md",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}