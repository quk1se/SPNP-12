using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
                proctime += process.TotalProcessorTime.TotalSeconds;
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
        Process? notepadProcess;
        private void StartNotepad_Click(object sender, RoutedEventArgs e)
        {
           notepadProcess ??= Process.Start("notepad.exe");

        }

        private void StopNotepad_Click(object sender, RoutedEventArgs e)
        {
            notepadProcess?.Kill();
            notepadProcess?.WaitForExit();
            notepadProcess?.Dispose();
            notepadProcess = null;
        }

        private void StartEdit_Click(object sender, RoutedEventArgs e)
        {
            String dir = AppContext.BaseDirectory;
            int binPosition = dir.LastIndexOf("bin");
            String projectRoot = dir[..binPosition];
            notepadProcess ??= Process.Start(
                "notepad.exe",
                $"{projectRoot}ProcessWindow.xaml.cs");
        }
        Process? browserProcess;
        private void StartBrowser_Click(object sender, RoutedEventArgs e)
        {
            browserProcess ??= Process.Start(
                "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",
                "");
        }
        Process? calculator;
        private void StartCalc_Click(object sender, RoutedEventArgs e)
        {
            calculator ??= Process.Start("calc.exe");
        }

        private void StopCalc_Click(object sender, RoutedEventArgs e)
        {
            calculator?.CloseMainWindow();
            calculator?.Kill(true);
            calculator?.Dispose();
            calculator = null;
        }
        Process? settings;
        private void StartSett_Click(object sender, RoutedEventArgs e)
        {
            settings ??= Process.Start("control.exe");
        }
        Process? disp;
        private void StartDisp_Click(object sender, RoutedEventArgs e)
        {
            disp ??= Process.Start("taskmgr.exe");
        }
    }
}
