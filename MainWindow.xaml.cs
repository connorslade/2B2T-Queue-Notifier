using System;
using System.Collections.Generic;
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
        private string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
        private string chat = "Position in queue: ";

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
            foreach (int working in dataGet.DataGet.getGameTime(path))
            {
                MessageBox.Show(working.ToString());
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //MainTime.Text = dataGet.DataGet.getIndex(path, chat).ToString();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            //MainTime.Text = dataGet.DataGet.getIndex(path, chat).ToString();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
    }
}