﻿using System;
using System.Diagnostics;
using System.Windows;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private readonly IniFile config =
            new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");

        private int tickDelayValue;
        private int timeOutValue;

        public Settings()
        {
            bool mntpoz;
            bool mntlogout;
            bool mntlogin;
            bool hookpoz;
            bool hooklogout;
            bool hooklogin;
            bool doWebHooks;
            InitializeComponent();
            timeOutValue = int.Parse(config.Read("timeout"));
            tickDelayValue = int.Parse(config.Read("tickdelay"));
            var logPath = config.Read("logpath");
            var chat = config.Read("chat");

            LogPathFeild.Text = logPath;
            TimeoutLabel.Content = timeOutValue;
            TickLabel.Content = tickDelayValue;
            ChatRegex.Text = chat;
            //DISCORD
            var webHook = config.Read("hookuri");
            var whomnt = config.Read("whomnt");
            try { doWebHooks = bool.Parse(config.Read("dowebhook")); } catch { doWebHooks = false; }
            try { hookpoz = bool.Parse(config.Read("hookpoz")); } catch { hookpoz = false; }
            try { hooklogin = bool.Parse(config.Read("hooklogin")); } catch { hooklogin = false; }
            try { hooklogout = bool.Parse(config.Read("hooklogout")); } catch { hooklogout = false; }
            try { mntlogin = bool.Parse(config.Read("mntlogin")); } catch { mntlogin = false; }
            try { mntlogout = bool.Parse(config.Read("mntlogout")); } catch { mntlogout = false; }
            try { mntpoz = bool.Parse(config.Read("mntpoz")); } catch { mntpoz = false; }

            WebHooksCheck.IsChecked = doWebHooks;
            WebHookUri.Text = webHook;
            LoginBox.IsChecked = hooklogin;
            LogoutBox.IsChecked = hooklogout;
            PozBox.IsChecked = hookpoz;
            LoginBoxCopy.IsChecked = mntlogin;
            LoginBoxCopy1.IsChecked = mntlogout;
            LoginBoxCopy2.IsChecked = mntpoz;
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
            if (tickDelayValue < 10)
                tickDelayValue++;
            TickLabel.Content = tickDelayValue;
        }

        private void LessTick(object sender, RoutedEventArgs e)
        {
            if (tickDelayValue > 1)
                tickDelayValue--;
            TickLabel.Content = tickDelayValue;
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
            config.Write("tickdelay", tickDelayValue.ToString());
            config.Write("logpath", LogPathFeild.Text);
            config.Write("chat", ChatRegex.Text);
            config.Write("dowebhook", WebHooksCheck.IsChecked.ToString());
            config.Write("hookuri", WebHookUri.Text);
            config.Write("hooklogin", LoginBox.IsChecked.ToString());
            config.Write("hooklogout", LogoutBox.IsChecked.ToString());
            config.Write("hookpoz", PozBox.IsChecked.ToString());
            config.Write("mntlogin", LoginBoxCopy.IsChecked.ToString());
            config.Write("mntlogout", LoginBoxCopy1.IsChecked.ToString());
            config.Write("mntpoz", LoginBoxCopy2.IsChecked.ToString());
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