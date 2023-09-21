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
    /// Логика взаимодействия для CancelWindow.xaml
    /// </summary>
    public partial class CancelWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;
        public int countCompleteProgress { get; set; }
        private readonly object countLocker = new();


        public CancelWindow()
        {
            InitializeComponent();
            cancellationTokenSource = null!;
        }

        private async void StartBtn1_Click(object sender, RoutedEventArgs e)
        {
            countCompleteProgress = 0;
            cancellationTokenSource = new CancellationTokenSource();
            RunProgressCancelable(ProgressBar10,cancellationTokenSource.Token);
            RunProgressCancelable(ProgressBar11,cancellationTokenSource.Token,4);
            RunProgressCancelable(ProgressBar12,cancellationTokenSource.Token,2);
        }

        private void StopBtn1_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
        private void StartBtn2_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            RunProgress(ProgressBar20, cancellationTokenSource.Token);
            RunProgress(ProgressBar21, cancellationTokenSource.Token, 4);
            RunProgress(ProgressBar22, cancellationTokenSource.Token, 2);
        }

        private void StopBtn2_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private async void StartBtn3_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await RunProgressWaitable(ProgressBar30, cancellationTokenSource.Token);
            await RunProgressWaitable(ProgressBar31, cancellationTokenSource.Token, 4);
            await RunProgressWaitable(ProgressBar32, cancellationTokenSource.Token, 2);
        }

        private void StopBtn3_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private async void RunProgress(ProgressBar progressBar, CancellationToken cancellationToken, int time = 3)
        {
            progressBar.Value = 0;
            for (int i = 0; i < 10; i++)
            {
                progressBar.Value += 10;
                await Task.Delay(1000 * time / 10);
                if ( cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        private async Task RunProgressWaitable(ProgressBar progressBar, CancellationToken cancellationToken, int time = 3) // писать с await в клике, и клик сделать async void
        {
            progressBar.Value = 0;
            for (int i = 0; i < 10; i++)
            {
                progressBar.Value += 10;
                await Task.Delay(1000 * time / 10);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        private async void RunProgressCancelable(ProgressBar progressBar, CancellationToken cancellationToken, int time = 3)
        {
            try
            {
                progressBar.Value = 0;
                lock(countLocker) countCompleteProgress += 1;
                for (int i = 0; i < 10; i++)
                {
                    progressBar.Value += 10;
                    await Task.Delay(1000 * time / 10);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException) 
            {
                if (progressBar.Value != 100)
                {
                    countCompleteProgress -= 1;
                    for (int i = Convert.ToInt32(progressBar.Value);i > 0;i--)
                    {
                        progressBar.Value -= 10;
                        await Task.Delay(1000 * time / 10);
                    }
                }
            }
            finally 
            {
                bool isLast;
                lock(countLocker) 
                {
                    countCompleteProgress -= 1;
                    isLast = countCompleteProgress == 0;
                }
                if (isLast) MessageBox.Show("Done");
            }
        }

    }
}
