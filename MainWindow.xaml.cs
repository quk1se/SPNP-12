﻿using System;
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
            new ThreadingWindow().ShowDialog();
            this.Show();
        }

        private void SynchroBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new SynchroWindow().ShowDialog();
            this.Show();
        }

        private void ManyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new TaskWindow().ShowDialog();
            this.Show();
        }
    }
}
