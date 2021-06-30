using System.ComponentModel;
using System.Windows;

namespace WpfNavy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListSortDirection curDepListSortDirection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Меняем порядок сортировки на противоположный.
            curDepListSortDirection = (ListSortDirection)(((int)curDepListSortDirection + 1) % 2);
            // Очищаем список сортировки.
            depListView.Items.SortDescriptions.Clear();
            // Сортируем список отделов по имени.
            depListView.Items.SortDescriptions.Add(new SortDescription("Name", curDepListSortDirection));

        }
    }
}
