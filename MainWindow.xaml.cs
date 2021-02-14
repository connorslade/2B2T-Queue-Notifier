//TODO: Do all tests for Release (New System, Run with Minecraft, Web hooks)
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Color bkHv = Color.FromRgb(67, 76, 94);
        private readonly Color bkLv = Color.FromRgb(45, 51, 63);
        private string chat = "Position in queue: ";

        private readonly IniFile config =
            new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");

        private DispatcherTimer dispatcherTimer;
        private bool doWebHook;
        private int eqFr;
        private bool hooklogin;
        private bool hooklogout;
        private bool hookpoz;
        private int indexCach;
        private bool isIn = true;
        private bool isLogin = true;
        private bool mntlogin;
        private bool mntlogout;
        private bool mntpoz;
        private string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
        private readonly Color tcf = Color.FromRgb(191, 97, 106);
        private readonly Color tcl = Color.FromRgb(163, 190, 140);
        private readonly Color tcm = Color.FromRgb(235, 203, 139);
        private int tickdelay = 10;
        private int timeout = 30;
        private string webHook;
        private string whomnt;

        public MainWindow()
        {
            InitializeComponent();
            if (!config.KeyExists("setup"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\");
                config.Write("setup", "true");
                config.Write("timeout", "30");
                config.Write("tickdelay", "10");
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
            }
            else
            {
                UpdateVars();
            }
        }

        private void UpdateVars()
        {
            try
            {
                chat = config.Read("chat");
                timeout = int.Parse(config.Read("timeout"));
                try { doWebHook = bool.Parse(config.Read("tickdelay")); } catch { doWebHook = false; }
                tickdelay = int.Parse(config.Read("tickdelay"));
                path = Environment.ExpandEnvironmentVariables(config.Read("logpath"));
                try { doWebHook = bool.Parse(config.Read("dowebhook")); } catch { doWebHook = false; }
                try { hookpoz = bool.Parse(config.Read("hookpoz")); } catch { hookpoz = false; }
                try { hooklogin = bool.Parse(config.Read("hooklogin")); } catch { hooklogin = false; }
                try { hooklogout = bool.Parse(config.Read("hooklogout")); } catch { hooklogout = false; }
                try { mntlogin = bool.Parse(config.Read("mntlogin")); } catch { mntlogin = false; }
                try { mntlogout = bool.Parse(config.Read("mntlogout")); } catch { mntlogout = false; }
                try { mntpoz = bool.Parse(config.Read("mntpoz")); } catch { mntpoz = false; }
                whomnt = config.Read("whomnt");
                webHook = config.Read("hookuri");
            } catch
            {
                MessageBox.Show("ERR reading Config File...");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\");
                config.Write("setup", "true");
                config.Write("timeout", "30");
                config.Write("tickdelay", "10");
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
            }
        }

        private void start_MouseEnter(object sender, MouseEventArgs e)
        {
            Start.Fill = new SolidColorBrush(bkHv);
        }

        private void start_MouseLeave(object sender, MouseEventArgs e)
        {
            Start.Fill = new SolidColorBrush(bkLv);
        }

        private void start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var win2 = new Settings();
            win2.Show();
        }

        private void WebhookError(bool sucess)
        {
            HookErr.Visibility = !sucess ? Visibility.Visible : Visibility.Hidden;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateVars();

                var full = DataGet.GetIndex(path, chat);
                var lastChatEvent = DataGet.ChatTime(path);
                var index = full[0];
                if (index != indexCach && full[1] > DataGet.NowTime() - timeout)
                {
                    eqFr = 0;
                    MainTime.Text = index.ToString();
                    if (index > 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(tcf);
                        if (hookpoz)
                            WebhookError(DataGet.DiscordWebHook(webHook, index.ToString(), indexCach, "12542314", doWebHook, mntpoz, whomnt));
                        indexCach = index;
                    }
                    else if (index > 250 && index < 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(tcm);
                        if (hookpoz)
                            WebhookError(DataGet.DiscordWebHook(webHook, index.ToString(), indexCach, "15453067", doWebHook, mntpoz, whomnt));
                        indexCach = index;
                    }
                    else if (index > 0 && index < 250)
                    {
                        MainTime.Foreground = new SolidColorBrush(tcl);
                        if (hookpoz)
                            WebhookError(DataGet.DiscordWebHook(webHook, index.ToString(), indexCach, "10731148", doWebHook, mntpoz, whomnt));
                        indexCach = index;
                    }
                    indexCach = full[0];
                    isIn = true;
                    isLogin = true;
                }
                else if (full[1] != lastChatEvent && lastChatEvent > DataGet.NowTime() - timeout)
                {
                    MainTime.Text = "Online!";
                    MainTime.Foreground = new SolidColorBrush(tcl);
                    if (hooklogin && isLogin)
                        WebhookError(DataGet.DiscordMessage(webHook, "**Logged In!** :grin:", "9419928", doWebHook, mntlogin, whomnt));
                    isIn = true;
                    isLogin = false;
                }
                else if (index == indexCach && full[1] > DataGet.NowTime() - timeout) { }
                else
                {
                    eqFr += tickdelay;
                    if (eqFr <= timeout)
                        return;
                    MainTime.Text = "…";
                    MainTime.Foreground = new SolidColorBrush(tcf);
                    if (hooklogout && isIn)
                        WebhookError(DataGet.DiscordMessage(webHook, "**Logged Out **", "12150125", doWebHook, mntlogout, whomnt));
                    isIn = false;
                    isLogin = false;
                    eqFr = 0;
                }
            } catch
            {
                MainTime.Foreground = new SolidColorBrush(tcf);
                MainTime.Text = "…";
            }
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            UpdateVars();
            MainTime.Text = "…";
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, tickdelay);
            dispatcherTimer.Start();
        }

        #region TopBar

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Close();
        }

        private void Mini_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                WindowState = WindowState.Minimized;
        }

        private void Pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (FullPin.Visibility == Visibility.Hidden)
            {
                FullPin.Visibility = Visibility.Visible;
                Topmost = true;
            }
            else
            {
                FullPin.Visibility = Visibility.Hidden;
                Topmost = false;
            }
        }

        #endregion TopBar
    }
}