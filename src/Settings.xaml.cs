using System;
using System.Diagnostics;
using System.Windows;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private readonly IniFile Config =
            new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");

        private int TickDelayValue;
        private int TimeOutValue;

        public Settings()
        {
            bool MntPosition;
            bool MntLogout;
            bool MntLogin;
            bool HookPosition;
            bool HookLogout;
            bool HookLogin;
            bool DoWebHooks;
            InitializeComponent();
            TimeOutValue = int.Parse(Config.Read("timeout"));
            TickDelayValue = int.Parse(Config.Read("tickdelay"));
            var LogPath = Config.Read("logpath");
            var Chat = Config.Read("chat");

            LogPathFeild.Text = LogPath;
            TimeoutLabel.Content = TimeOutValue;
            TickLabel.Content = TickDelayValue;
            ChatRegex.Text = Chat;
            //DISCORD
            var WebHook = Config.Read("hookuri");
            var WhoMnt = Config.Read("whomnt");
            try { DoWebHooks = bool.Parse(Config.Read("dowebhook")); } catch { DoWebHooks = false; }
            try { HookPosition = bool.Parse(Config.Read("hookpoz")); } catch { HookPosition = false; }
            try { HookLogin = bool.Parse(Config.Read("hooklogin")); } catch { HookLogin = false; }
            try { HookLogout = bool.Parse(Config.Read("hooklogout")); } catch { HookLogout = false; }
            try { MntLogin = bool.Parse(Config.Read("mntlogin")); } catch { MntLogin = false; }
            try { MntLogout = bool.Parse(Config.Read("mntlogout")); } catch { MntLogout = false; }
            try { MntPosition = bool.Parse(Config.Read("mntpoz")); } catch { MntPosition = false; }

            WebHooksCheck.IsChecked = DoWebHooks;
            WebHookUri.Text = WebHook;
            LoginBox.IsChecked = HookLogin;
            LogoutBox.IsChecked = HookLogout;
            PozBox.IsChecked = HookPosition;
            LoginBoxCopy.IsChecked = MntLogin;
            LoginBoxCopy1.IsChecked = MntLogout;
            LoginBoxCopy2.IsChecked = MntPosition;
            MntFeild.Text = WhoMnt;
        }

        private void PlusTimeout(object sender, RoutedEventArgs e)
        {
            if (TimeOutValue < 100)
                TimeOutValue++;
            TimeoutLabel.Content = TimeOutValue;
        }

        private void LessTimeout(object sender, RoutedEventArgs e)
        {
            if (TimeOutValue > 0)
                TimeOutValue--;
            TimeoutLabel.Content = TimeOutValue;
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
            var Psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Basicprogrammer10/2B2T-Queue-Notifier/issues",
                UseShellExecute = true
            };
            Process.Start(Psi);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Config.Write("timeout", TimeOutValue.ToString());
            Config.Write("tickdelay", TickDelayValue.ToString());
            Config.Write("logpath", LogPathFeild.Text);
            Config.Write("chat", ChatRegex.Text);
            Config.Write("dowebhook", WebHooksCheck.IsChecked.ToString());
            Config.Write("hookuri", WebHookUri.Text);
            Config.Write("hooklogin", LoginBox.IsChecked.ToString());
            Config.Write("hooklogout", LogoutBox.IsChecked.ToString());
            Config.Write("hookpoz", PozBox.IsChecked.ToString());
            Config.Write("mntlogin", LoginBoxCopy.IsChecked.ToString());
            Config.Write("mntlogout", LoginBoxCopy1.IsChecked.ToString());
            Config.Write("mntpoz", LoginBoxCopy2.IsChecked.ToString());
            Config.Write("whomnt", MntFeild.Text);
            Close();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            IniFile.SetDefaultConfig(Config);
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var Psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Basicprogrammer10/2B2T-Queue-Notifier/blob/master/Settings.md",
                UseShellExecute = true
            };
            Process.Start(Psi);
        }
    }
}