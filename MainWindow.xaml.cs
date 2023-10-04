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

namespace SPNP_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThreadingBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            try { new ThreadingWindow().ShowDialog(); } catch { }
            this.Show();
        }

        private void SynchroBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            try { new SynchroWindow().ShowDialog(); } catch { }
            this.Show();
        }

        private void ManyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new TaskWindow().ShowDialog();
            this.Show();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new CancelWindow().ShowDialog();
            this.Show();
        }

        private void ProcessBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            try { new ProcessWindow().ShowDialog(); } catch { }
            this.Show();
        }

        private void ChainingBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            try { new ChainingWindow().ShowDialog(); } catch { }
            this.Show();
        }

        private void DLLBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
           new DllWindow().ShowDialog();
            this.Show();
        }
    }
}
