using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dataGet;

namespace _2B2T_Queue_Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Color BkHv = Color.FromRgb(67, 76, 94);
        public Color BkLv = Color.FromRgb(45, 51, 63);
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
        #endregion
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
            string path = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft\logs\latest.log");
            string chat = "Position in queue: ";
            MainTime.Text = dataGet.DataGet.getIndex(path, chat).ToString();
        }
    }
}
