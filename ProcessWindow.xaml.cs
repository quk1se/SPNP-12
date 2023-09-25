using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace SPNP_12
{
    /// <summary>
    /// Логика взаимодействия для ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        public ProcessWindow()
        {
            InitializeComponent();
        }

        private void ShowProcesses_Click(object sender, RoutedEventArgs e)
        {
            long allmemory = 0;
            double proctime = 0;
            Process[] processes = Process.GetProcesses();
            //ProcTextBlock.Text = "";
            ProcTreeView.Items.Clear();
            string prevName = "";
            TreeViewItem item = null;
            processcount.Content = processes.Count().ToString();
            foreach (Process process in processes.OrderBy(p => p.ProcessName))
            {
                allmemory += process.PrivateMemorySize64;
                //proctime += process.TotalProcessorTime.TotalSeconds;
                if (prevName != process.ProcessName)
                {
                    prevName = process.ProcessName + " | " + process.PrivateMemorySize64 + " bytes";
                    item = new TreeViewItem() { Header = prevName };
                    ProcTreeView.Items.Add(item);
                }
                var subItem = new TreeViewItem()
                {
                    Header = process.Id + " | " + process.ProcessName + " | " + process.PrivateMemorySize64 + " bytes",
                    Tag = process
                };
                subItem.MouseDoubleClick += TreeViewItem_MouseDoubleClick;
                item?.Items.Add(subItem);
            }
            processmemory.Content = (allmemory * 0.000001).ToString() + " MB";
            processtime.Content = (proctime / 60).ToString() + " min";
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                String message = "";
                if (item.Tag is Process process)
                {
                    foreach (ProcessThread thread in process.Threads)
                    {
                        message += thread.Id + "\r\n";
                    }
                }
                else
                {
                    message = "No process in tag";
                }
                MessageBox.Show(message);
            }
        }
    }
}
