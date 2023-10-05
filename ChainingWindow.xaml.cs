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

namespace SPNP_12
{
    /// <summary>
    /// Логика взаимодействия для ChainingWindow.xaml
    /// </summary>
    public partial class ChainingWindow : Window
    {
        public ChainingWindow()
        {
            InitializeComponent();
        }
        private void StartBtn1_Click(object sender, RoutedEventArgs e)
        {
            var task10 = showProgress(ProgressBar1).ContinueWith( task10 => showProgress(ProgressBar2)
            .ContinueWith( task11 => showProgress(ProgressBar3)));
            showProgress(ProgressBar12).ContinueWith(task => showProgress(ProgressBar22)
            .ContinueWith(task => showProgress(ProgressBar32)));
        }

        private void StopBtn1_Click(object sender, RoutedEventArgs e)
        {

        }
        private async Task showProgress(ProgressBar progressBar)
        {
            int delay = 100;
            if (progressBar == ProgressBar1) delay = 100;
            if (progressBar == ProgressBar2) delay = 200;
            if (progressBar == ProgressBar3) delay = 300;
            if (progressBar == ProgressBar12) delay = 300;
            if (progressBar == ProgressBar22) delay = 200;
            if (progressBar == ProgressBar32) delay = 100;

            for (int i = 0; i <= 10; i++)
            {
                await Task.Delay(delay);
                Dispatcher.Invoke(() => progressBar.Value = i * 10);
            }
        }

        private void StopBtn12_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void StartBtn22_Click(object sender, RoutedEventArgs e)
        {
            var task10 = showProgress(ProgressBar1);
            var task21 = showProgress(ProgressBar12);
            await task10; var task11 = showProgress(ProgressBar2);
            await task11; var task12 = showProgress(ProgressBar3);
            await task21; var task22 = showProgress(ProgressBar22);
            await task22; var task23 = showProgress(ProgressBar32);
        }
        CancellationTokenSource cts;
        private void StartBtn3_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            LogTextBlock.Text = "";
            String str = "";
            try
            {
                var text = AddHello(str)
                .ContinueWith(task =>
                {
                    String res = task.Result;
                    Dispatcher.Invoke(() => LogTextBlock.Text = res);
                    return AddWorld(res);
                }, cts.Token)
                .Unwrap()
                .ContinueWith(task2 =>
                {
                    String res = task2.Result;
                    Dispatcher.Invoke(() => LogTextBlock.Text = res);
                    return AddSymbol(res);
                }, cts.Token)
                .Unwrap()
                .ContinueWith(task =>
                Dispatcher.Invoke(() => LogTextBlock.Text = task.Result), cts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }   
        }
        async Task<String> AddHello(String str)
        {
            await Task.Delay(1000);
            if (!cts.Token.IsCancellationRequested) return str + " Hello ";
            else return "";
        }
        async Task<String> AddWorld(String str)
        {
            await Task.Delay(1000);
            if (!cts.Token.IsCancellationRequested) return str + " World ";
            else return "";
        }
        async Task<String> AddSymbol(String str)
        {
            await Task.Delay(1000);
            if (!cts.Token.IsCancellationRequested) return str + " ! ";
            else return "";
        }

        private void StopBtn3_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
    }
}
