using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SPNP_12
{
    public partial class SynchroWindow : Window
    {
        private double sum;
        private int threadCount;
        private static Mutex? mutex;
        private const String mutexName = "SPNP_SW_MUTEX";
        public SynchroWindow()
        {
            WaitOtherInstance();
            InitializeComponent();
        }
        private void WaitOtherInstance()
        {
            try { mutex = Mutex.OpenExisting(mutexName); } catch { }
            if (mutex == null)
            {
                mutex = new Mutex(true, mutexName);
            }
            else
            {
                if (!mutex.WaitOne(1))
                {
                    if (new CountDownWindow(mutex).ShowDialog() == false)
                    {
                        throw new ApplicationException();
                    }
                    mutex.WaitOne();
                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            mutex?.ReleaseMutex();
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            LogTextBlock.Text = String.Empty;
            threadCount = 12;
            for (int i = 0; i < threadCount; i++)
            {
                new Thread(AddPercent).Start(new MonthData { Month = i + 1});
            }
        }
        private object sumLocker = new();
        private void AddPercent(object? data)
        { 
            var monthData = data as MonthData;
            Thread.Sleep(200);
            double localSum;
            lock (sumLocker)
            {
                localSum = 
                    sum *= 1.1;
            }
            Dispatcher.Invoke(() =>
            {
                LogTextBlock.Text += $"{monthData?.Month} {localSum}\n";
            });
            threadCount--;
            Thread.Sleep(200);
            if (threadCount == 0)
            {
                Dispatcher.Invoke(() =>
                    LogTextBlock.Text += $"---------\nresult = {sum}");
            }
            
        }
        class MonthData
        {
            public int Month { get; set; }

        }
        private void AddPercent3()
        {
            Thread.Sleep(200);
            double localSum;
            lock (sumLocker)
            {
                localSum =
                    sum *= 1.1;
            }
            Dispatcher.Invoke(() =>
            {
                LogTextBlock.Text += $"{sum}\n";
            });

        }
        private void AddPercent2()
        {
            lock(sumLocker)
            {
                Thread.Sleep(200);
                double localSum = sum;
                localSum *= 1.1;
                sum = localSum;
                Dispatcher.Invoke(() =>
                {
                    LogTextBlock.Text += $"{sum}\n";
                });
            }
        }
        private void AddPercent1()
        {
            double localSum = sum;
            Thread.Sleep(200);
            localSum *= 1.1;
            sum = localSum;
            Dispatcher.Invoke(() =>
            {
                LogTextBlock.Text += $"{sum}\n";
            });
        }
    }
}
