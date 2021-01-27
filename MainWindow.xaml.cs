using dataGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IniFile config = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\settings.ini");
        private DispatcherTimer dispatcherTimer;
        public Color BkHv = Color.FromRgb(67, 76, 94);
        public Color BkLv = Color.FromRgb(45, 51, 63);
        public Color TCL = Color.FromRgb(163, 190, 140);
        public Color TCM = Color.FromRgb(235, 203, 139);
        public Color TCF = Color.FromRgb(191, 97, 106);
        public Color TCN = Color.FromRgb(94, 129, 172);
        private string webHook;
        private bool doWebHook;
        private bool hooklogin;
        private bool hooklogout;
        private bool hookpoz;
        private string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
        private string chat = "Position in queue: ";
        private int timeout = 30;
        private int tickdelay = 10;
        private int indexCach = 0;
        private bool isIn = true;
        private bool isLogin = true;
        private int EqFr;

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
                ///// ----- D I S C O R D ----- \\\\\
                config.Write("dowebhook", "false");
                config.Write("hooklogin", "true");
                config.Write("hooklogout", "true");
                config.Write("hookpoz", "true");
                config.Write("hookuri", "");
            }
            else
            {
                updateVars();
            }
        }

        #region TopBar

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.Close();
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
                this.Topmost = true;
            }
            else
            {
                FullPin.Visibility = Visibility.Hidden;
                this.Topmost = false;
            }
        }

        #endregion TopBar

        private void updateVars()
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
            webHook = config.Read("hookuri");
        }

        private void start_MouseEnter(object sender, MouseEventArgs e)
        {
            start.Fill = new SolidColorBrush(BkHv);
        }

        private void start_MouseLeave(object sender, MouseEventArgs e)
        {
            start.Fill = new SolidColorBrush(BkLv);
        }

        private void start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings win2 = new Settings();
            win2.Show();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateVars();

                List<int> FULL = dataGet.DataGet.getIndex(path, chat);
                int lastChatEvent = dataGet.DataGet.ChatTime(path);
                int index = FULL[0];
                if (index != indexCach && FULL[1] > dataGet.DataGet.NowTime() - timeout)
                {
                    EqFr = 0;
                    MainTime.Text = index.ToString();
                    if (index > 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCF);
                        if (hookpoz)
                            DataGet.discordWebHook(webHook, index.ToString(), indexCach, "12542314", doWebHook);
                        indexCach = index;
                    }
                    else if (index > 250 && index < 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCM);
                        if (hookpoz)
                            DataGet.discordWebHook(webHook, index.ToString(), indexCach, "15453067", doWebHook);
                        indexCach = index;
                    }
                    else if (index > 0 && index < 250)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCL);
                        if (hookpoz)
                            DataGet.discordWebHook(webHook, index.ToString(), indexCach, "10731148", doWebHook);
                        indexCach = index;
                    }
                    indexCach = FULL[0];
                    isIn = true;
                    isLogin = true;
                }
                else if (FULL[1] != lastChatEvent && lastChatEvent > dataGet.DataGet.NowTime() - timeout)
                {
                    MainTime.Text = "Online!";
                    MainTime.Foreground = new SolidColorBrush(TCL);
                    if (hooklogin && isLogin)
                        DataGet.DiscordMessage(webHook, "**Logged In!** :grin:", "9419928", doWebHook);
                    isIn = true;
                    isLogin = false;
                }
                else
                {
                    EqFr += tickdelay;
                    if (EqFr > timeout)
                    {
                        EqFr = 0;
                        MainTime.Text = "…";
                        MainTime.Foreground = new SolidColorBrush(TCF);
                        if (hooklogout && isIn)
                            DataGet.DiscordMessage(webHook, "**Logged Out **", "12150125", doWebHook);
                        isIn = false;
                        isLogin = false;
                    }
                }
            }
            catch { MainTime.Text = "…"; }
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            updateVars();
            MainTime.Text = "…";
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, tickdelay);
            dispatcherTimer.Start();
        }
    }
}