using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SPNP_12
{
    /// <summary>
    /// Логика взаимодействия для ThreadingWindow.xaml
    /// </summary>
    public partial class ThreadingWindow : Window
    {
        public ThreadingWindow()
        {
            InitializeComponent();
        }
        #region 1-2
        private void StopBtn1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartBtn1_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                ProgressBar1.Value = i * 10;
                Thread.Sleep(300);
            }
            ProgressBar1.Value = 100;
        }

        private void StartBtn2_Click(object sender, RoutedEventArgs e)
        {
            new Thread(IncrementProgress).Start();
        }

        private void StopBtn2_Click(object sender, RoutedEventArgs e)
        {

        }
        private void IncrementProgress()
        {
            for (int i = 0; i < 11; i++)
            {
                ProgressBar2.Value = i * 10;
                Thread.Sleep(300);
            }
            ProgressBar2.Value = 100;
        }
#endregion
        #region 3
        private bool isStopped3 { get; set; }
        private void StopBtn3_Click(object sender, RoutedEventArgs e)
        {
            isStopped3 = true;
        }
        private void StartBtn3_Click(object sender, RoutedEventArgs e)
        {
            new Thread(IncrementProgress3).Start();
            isStopped3 = false;
        }
        private void IncrementProgress3()
        {
            for (int i = 0; i <= 10 && !isStopped3; i++)
            {
                this.Dispatcher.Invoke(
                    () => ProgressBar3.Value = i * 10
                    );
                Thread.Sleep(300);
            }


        }
#endregion
        #region 4
        private bool isStopped4 { get; set; }
        private Thread thread4;
        private void StartBtn4_Click(object sender, RoutedEventArgs e)
        {
            StartBtn4.IsEnabled = false;
            StopBtn4.IsEnabled = true;
            if (thread4 == null)
            {
                isStopped4 = false;
                thread4 = new Thread(IncrementProgress4);
                thread4.Start();
            }
        }

        private void StopBtn4_Click(object sender, RoutedEventArgs e)
        {
            StopBtn4.IsEnabled = false;
            StartBtn4.IsEnabled = true;
            isStopped4 = true;
            thread4 = null;
        }

        private void IncrementProgress4()
        {
            for (int i = 0; i <= 10 && !isStopped4; i++)
            {
                this.Dispatcher.Invoke(
                    () => ProgressBar4.Value = i * 10
                    );
                Thread.Sleep(300);
            }
            thread4 = null;
            Dispatcher.Invoke( () =>StartBtn4.IsEnabled = true);
        }
#endregion
        #region 5
        private bool isStopped5 { get; set; }
        public Thread? thread5;
        CancellationTokenSource cts;
        private void StartBtn5_Click(object sender, RoutedEventArgs e)
        {
            int workTime = Convert.ToInt32(WorkTimeTxtBox.Text);
            thread5 = new Thread(IncrementProgress5);
            cts = new();
            thread5.Start(new ThreadData5
            {
                WorkTime = workTime,
                CancelToken = cts.Token
            });
        }

        private void StopBtn5_Click(object sender, RoutedEventArgs e)
        {
            cts?.Cancel();
        }
        private void IncrementProgress5(object?parameter)
        {   
            if (parameter is ThreadData5 data)
            {
                for (int i = 0; i <= 10; i++)
                {
                    this.Dispatcher.Invoke(
                        () => ProgressBar5.Value = i * 10
                        );
                    Thread.Sleep(100 * data.WorkTime);
                    if (data.CancelToken.IsCancellationRequested) 
                        break;

                }
            }
            else
            {
                MessageBox.Show("Thread 5 started with invalid argument");
            }

        }
        
        class ThreadData5
        {
            public int WorkTime { get; set; }
            public CancellationToken CancelToken { get; set; }
        }
        #endregion
        #region 6
        private void StopBtn6_Click(object sender, RoutedEventArgs e)
        {
            cts6?.Cancel();
        }

        private void StartBtn6_Click(object sender, RoutedEventArgs e)
        {
            int workTime1 = Convert.ToInt32(WorkTimeTxtBox1.Text);
            int workTime2 = Convert.ToInt32(WorkTimeTxtBox1.Text);
            int workTime3 = Convert.ToInt32(WorkTimeTxtBox1.Text);
            thread6 = new Thread(IncrementProgress6);
            cts6 = new();
            thread6.Start(new ThreadData6
            {
                WorkTime1 = workTime1,
                WorkTime2 = workTime2,
                WorkTime3 = workTime3,
                CancelToken = cts6.Token
            });
        }
        public Thread? thread6;
        public Thread? thread61;
        public Thread? thread62;
        CancellationTokenSource cts6;
        class ThreadData6
        {
            public int WorkTime1 { get; set; }
            public int WorkTime2 { get; set; }
            public int WorkTime3 { get; set; }
            public CancellationToken CancelToken { get; set; }
        }
        private void IncrementProgress6(object? parameter)
        {
            if (parameter is ThreadData6 data)
            {
                for (int i = 0; i <= 10; i++)
                {
                    this.Dispatcher.Invoke(
                        () => ProgressBar6.Value = i * 10
                        );
                    this.Dispatcher.Invoke(
                        () => ProgressBar61.Value = i * 10
                        );
                    this.Dispatcher.Invoke(
                        () => ProgressBar62.Value = i * 10
                        );
                    Thread.Sleep(100 * data.WorkTime1);
                    if (data.CancelToken.IsCancellationRequested)
                        break;

                }
            }
            else
            {
                MessageBox.Show("Thread 5 started with invalid argument");
            }
        }
        #endregion

    }
}
