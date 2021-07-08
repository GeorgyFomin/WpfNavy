using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNavy.ViewModels;

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
            {
                string name = (sender as TextBox).Name;
                bool clientNameChanged = name == "clientNameBox" || name == "topClientNameBox";
                AdjustColumnWidth(clientNameChanged ? clientNameСolumn : depNameColumn);
                MainViewModel.Log("Изменено имя " + (clientNameChanged ? "клиента " : "отдела ") + (sender as TextBox).Text);
            }
        }
    }
}
