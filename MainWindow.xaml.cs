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
        private string webHook = "";
        private string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
        private string chat = "Position in queue: ";
        private int timeout = 30;
        private int tickdelay;
        private int indexCach = 0;
        private int EqFr;

        public MainWindow()
        {
            InitializeComponent();
            if (!config.KeyExists("setup"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\connorcode\2B2T-Queue-Notifier\");
                config.Write("setup","true");
                config.Write("timeout","30");
                config.Write("tickdelay","1");
                config.Write("chat", "Position in queue: ");
                config.Write("logpath", @"%AppData%\.minecraft\logs\latest.log");
            }
            else
            {
                chat = config.Read("chat");
                timeout = int.Parse(config.Read("timeout"));
                tickdelay = int.Parse(config.Read("tickdelay"));
                path = Environment.ExpandEnvironmentVariables(config.Read("logpath"));
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
                List<int> FULL = dataGet.DataGet.getIndex(path, chat);
                int lastChatEvent = dataGet.DataGet.ChatTime(path);
                int index = FULL[0];
                //MessageBox.Show(index.ToString());
                if (index != indexCach && FULL[1] > dataGet.DataGet.NowTime() - timeout)
                {
                    EqFr = 0;
                    MainTime.Text = index.ToString();
                    if (index > 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCF);
                        //discordWebHook(webHook, index.ToString(), indexCach, "12542314");
                        indexCach = index;
                    }
                    else if (index > 250 && index < 500)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCM);
                        //discordWebHook(webHook, index.ToString(), indexCach, "15453067");
                        indexCach = index;
                    }
                    else if (index > 0 && index < 250)
                    {
                        MainTime.Foreground = new SolidColorBrush(TCL);
                        //discordWebHook(webHook, index.ToString(), indexCach, "10731148");
                        indexCach = index;
                    }
                    indexCach = FULL[1];
                }
                else if (FULL[1] != lastChatEvent && lastChatEvent > dataGet.DataGet.NowTime() - timeout)
                {
                    MainTime.Text = "Online!";
                    MainTime.Foreground = new SolidColorBrush(TCL);
                }
                else
                {
                    EqFr++;
                    if (EqFr > timeout)
                    {
                        EqFr = 0;
                        MainTime.Text = "…";
                        MainTime.Foreground = new SolidColorBrush(TCF);
                    }
                }

            }
            catch { MainTime.Text = "…"; }
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            MainTime.Text = "…";
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, tickdelay);
            dispatcherTimer.Start();
        }
    }
}