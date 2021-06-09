using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfNavy.ViewModels;

namespace WpfNavy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainViewModel mainViewModel = new MainViewModel();
            MainWindow mainWindow = new MainWindow() { DataContext = mainViewModel };
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
