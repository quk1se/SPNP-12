using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для DllWindow.xaml
    /// </summary>
    public partial class DllWindow : Window
    {
        [DllImport("user32.dll")]
        public
            static
            extern
            int MessageBoxA(
                IntPtr hWnd,
                String lpText,
                String lpCaption,
                uint uType
            );
        public DllWindow()
        {
            InitializeComponent();
        }

        private void AlertButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxA(IntPtr.Zero, "Hello world", "Header", 0x40);
        }
        public delegate void ThreadMethod();

        [DllImport("kernel32.dll", EntryPoint = "CreateThread")]
        public static extern
            IntPtr NewThread(
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            ThreadMethod lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId
            );
        public void ErrorMessage()
        {
            MessageBoxA(
                IntPtr.Zero,
                "Error message",
                "Complete",
                0x14
                );
            methodHandle.Free();
        }
        GCHandle methodHandle;

        private void ThreadBtn_Click(object sender, RoutedEventArgs e)
        {
            var method = new ThreadMethod(ErrorMessage);
            methodHandle = GCHandle.Alloc(method);
            NewThread(IntPtr.Zero, 0, method, IntPtr.Zero, 0, IntPtr.Zero);
        }
        [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        public static extern bool Sound(uint frequency, uint duration);
        private async void sound1_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run( () => Sound(440, 300));
        }

        private async void sound2_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => Sound(540, 300));
        }

        private void sound3_Click(object sender, RoutedEventArgs e)
        {
            Sound(640, 300);
        }

        private void sound4_Click(object sender, RoutedEventArgs e)
        {
            Sound(740, 300);
        }
    }
}
