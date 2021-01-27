using dataGet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private IniFile config = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");
        int timeOutValue = 30;
        int TickDelayValue = 1;
        string LogPath = @"%AppData%\.minecraft\logs\latest.log";
        private string chat = "Position in queue: ";
        bool doWebHooks = false;
        string webHook = "";

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
            try { doWebHooks = bool.Parse(config.Read("dowebhook")); } catch { doWebHooks = false; }
            webHook = config.Read("hookuri");


            webHooksCheck.IsChecked = doWebHooks;
            WebHookUri.Text = webHook;
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
            this.Close();
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
            this.Close();
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
            this.Close();
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
