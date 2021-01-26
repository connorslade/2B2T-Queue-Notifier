using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private DispatcherTimer dispatcherTimer;
        public Color BkHv = Color.FromRgb(67, 76, 94);
        public Color BkLv = Color.FromRgb(45, 51, 63);
        public Color TCL = Color.FromRgb(163, 190, 140);
        public Color TCM = Color.FromRgb(235, 203, 139);
        public Color TCF = Color.FromRgb(191, 97, 106);
        private string webHook = "";
        private string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
        private string chat = "Position in queue: ";
        private int indexCach;

        public MainWindow()
        {
            InitializeComponent();
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
            // foreach (int working in dataGet.DataGet.getGameTime(path))
            // {
            //    MessageBox.Show(working.ToString());
            //}
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            List<int> FULL = dataGet.DataGet.getIndex(path, chat);
            int index = FULL[0];
            if (FULL[1] != indexCach)
            {
                MainTime.Text = index.ToString();
                if (index > 500)
                {
                    MainTime.Foreground = new SolidColorBrush(TCF);
                    dataGet.DataGet.discordWebHook(webHook, index.ToString(), indexCach, "12542314");
                    indexCach = index;
                }
                else if (index > 250 && index < 500)
                {
                    MainTime.Foreground = new SolidColorBrush(TCM);
                    dataGet.DataGet.discordWebHook(webHook, index.ToString(), indexCach, "15453067");
                    indexCach = index;
                }
                else if (index > 0 && index < 250)
                {
                    MainTime.Foreground = new SolidColorBrush(TCL);
                    dataGet.DataGet.discordWebHook(webHook, index.ToString(), indexCach, "10731148");
                    indexCach = index;
                }
                indexCach = FULL[1];
            }
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            MainTime.Text = dataGet.DataGet.getIndex(path, chat).ToString();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
    }
}