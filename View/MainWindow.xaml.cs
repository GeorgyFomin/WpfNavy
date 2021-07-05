﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfNavy
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
        private void AdjustColumnWidth(GridViewColumn column)
        {
            for (int i = 0; i < 2; i++)
                column.Width = double.IsNaN(column.Width) ? column.ActualWidth : double.NaN;
        }
        private void TextBoxEditNameHandler(object sender, RoutedEventArgs e)
        {
            if ((e is KeyEventArgs arg && arg.Key == Key.Enter) || !(e is KeyEventArgs))
                AdjustColumnWidth((sender as TextBox).Name == "clientNameBox" ? clientNameСolumn : depNameColumn);
        }
    }
}
