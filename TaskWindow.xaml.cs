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
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
        }

        private void DemoBtn1_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(demo1);
            task.Start();

            Task task2 = Task.Run(demo1);
        }
        private void demo1()
        {
            Dispatcher.Invoke(() => LogTextBlock.Text += "demo1 starts\n");
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => LogTextBlock.Text += "demo1 finishes\n");
        }
        private async void DemoBtn2_Click(object sender, RoutedEventArgs e)
        {
            //Task<String> task = demo2();
            //String str = await task;
            //LogTextBlock.Text += $"demo2 result: {str}\n";a

            Task<String> task1 = demo2();
            Task<String> task2 = demo2();
            string res = $"demo2-1 result: {await task1} \n";
            LogTextBlock.Text += res;
            res = $"demo2-2 result:  {await task2} \n";
            LogTextBlock.Text += res;
            
        }
        private async Task<string> demo2()
        {
            LogTextBlock.Text += "demo2 starts\n";
            await Task.Delay(1000);  
            return "Done";
        }

        private void ParalelBtn_Click(object sender, RoutedEventArgs e)
        {
            Task task2 = new Task(MultitaskProgress2);
            task2.Start();
            Task task3 = new Task(MultitaskProgress3);
            task3.Start();
        }

        private async void SubsequenceBtn_Click(object sender, RoutedEventArgs e)
        {
            Task task1 = new Task(MultitaskProgress1); 
            task1.Start();
        }
        public Thread? threadMulti;
        private async void MultitaskProgress1()
        {
            MultitaskingData data = new MultitaskingData();
            data.WorkTime1 = 5;
            data.WorkTime2 = 2;
            Dispatcher.Invoke( () => SubsequenceBtn.IsEnabled = false);
            for (int i = 0; i <= 10; i++)   
            {
                this.Dispatcher.Invoke(
                    () => FirstProgressBar.Value = i * 10
                    );
                await Task.Delay(100 * data.WorkTime1);
            }
            for (int i = 0; i <= 10; i++)
            {
                this.Dispatcher.Invoke(
                    () => SecondProgressBar.Value = i * 10
                    );
                await Task.Delay(100 * data.WorkTime2);
            }
            Dispatcher.Invoke(() => SubsequenceBtn.IsEnabled = true);
        }
        private async void MultitaskProgress2()
        {
            MultitaskingData data = new MultitaskingData();
            data.WorkTime1 = 5;
            data.WorkTime2 = 2;
            Dispatcher.Invoke(() => ParalelBtn.IsEnabled = false);
            for (int i = 0; i <= 10; i++)
            {
                this.Dispatcher.Invoke(
                    () => FirstProgressBar.Value = i * 10
                    );
                await Task.Delay(100 * data.WorkTime1);
            }
            Dispatcher.Invoke(() => ParalelBtn.IsEnabled = true);
            if (data.WorkTime1 > data.WorkTime2) Dispatcher.Invoke(() => ParalelBtn.IsEnabled = true);
        }
        private async void MultitaskProgress3()
        {
            MultitaskingData data = new MultitaskingData();
            data.WorkTime1 = 5;
            data.WorkTime2 = 2;
            Dispatcher.Invoke(() => ParalelBtn.IsEnabled = false);
            for (int i = 0; i <= 10; i++)
            {
                this.Dispatcher.Invoke(
                    () => SecondProgressBar.Value = i * 10
                    );
                await Task.Delay(100 * data.WorkTime2);
            }
            if (data.WorkTime2 > data.WorkTime1) Dispatcher.Invoke(() => ParalelBtn.IsEnabled = true);
        }
        class MultitaskingData
        {
            public int WorkTime1 { get; set; }
            public int WorkTime2 { get; set; }
        }
    }
}
