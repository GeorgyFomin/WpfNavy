using ClassLibrary;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNavy.Commands;

namespace WpfNavy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private Bank bank;
        private Dep dep;
        private Client client;
        private Account account;
        private ObservableCollection<Client> clients;
        private ObservableCollection<Account> accounts;
        private RelayCommand dragCommand;
        private RelayCommand minimizeCommand;
        private RelayCommand maximizeCommand;
        private RelayCommand closeCommand;
        private RelayCommand resetBankCommand;
        private RelayCommand depSelectedCommand;
        private RelayCommand clientSelectedCommand;
        private RelayCommand addDepCommand;
        private RelayCommand addClientCommand;
        private RelayCommand addAccountCommand;
        private RelayCommand removeDepCommand;
        private RelayCommand removeClientCommand;
        private RelayCommand removeAccCommand;
        private RelayCommand accSelectedCommand;
        private RelayCommand sortByNameCommand;
        private RelayCommand accSortByNmbCommand;
        private RelayCommand accSortBySizeCommand;
        private RelayCommand accSortByRateCommand;
        private bool removeClientEnabled;
        private bool removeAccEnabled;
        private bool removeDepEnabled;
        private ListSortDirection curDepListSortDirection;
        private bool clientSortEnabled;
        private bool accSortEnabled;
        #endregion
        #region Properties
        public Bank Bank { get => bank; set { bank = value; RaisePropertyChanged(nameof(Bank)); } }
        public Dep Dep { get => dep; set { dep = value; RaisePropertyChanged(nameof(Dep)); } }
        public Client Client { get => client; set { client = value; RaisePropertyChanged(nameof(Client)); } }
        public Account Account { get => account; set { account = value; RaisePropertyChanged(nameof(Account)); } }
        public ObservableCollection<Client> Clients { get => clients; private set { clients = value; RaisePropertyChanged(nameof(Clients)); } }
        public ObservableCollection<Account> Accounts { get => accounts; private set { accounts = value; RaisePropertyChanged(nameof(Accounts)); } }
        public bool RemoveAccEnabled { get => removeAccEnabled; private set { removeAccEnabled = value; RaisePropertyChanged(nameof(RemoveAccEnabled)); } }
        public bool RemoveClientEnabled { get => removeClientEnabled; private set { removeClientEnabled = value; RaisePropertyChanged(nameof(RemoveClientEnabled)); } }
        public bool RemoveDepEnabled { get => removeDepEnabled; private set { removeDepEnabled = value; RaisePropertyChanged(nameof(RemoveDepEnabled)); } }
        public bool ClientSortEnabled { get => clientSortEnabled; private set { clientSortEnabled = value; RaisePropertyChanged(nameof(ClientSortEnabled)); } }
        public bool AccSortEnabled { get => accSortEnabled; private set { accSortEnabled = value; RaisePropertyChanged(nameof(AccSortEnabled)); } }
        public ICommand DragCommand => dragCommand ?? (dragCommand = new RelayCommand(action: (e) => (e as MainWindow).DragMove()));
        public ICommand MinimizeCommand => minimizeCommand ?? (minimizeCommand = new RelayCommand((e) => (e as MainWindow).WindowState = WindowState.Minimized));
        public ICommand MaximizeCommand => maximizeCommand ?? (maximizeCommand = new RelayCommand(action: (e) =>
        {
            MainWindow window = e as MainWindow;
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            window.MaxIconBlock.Icon = window.WindowState == WindowState.Maximized ? IconChar.WindowRestore : IconChar.WindowMaximize;
        }));
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand((e) => (e as MainWindow).Close()));
        public ICommand ResetBankCommand => resetBankCommand ?? (resetBankCommand = new RelayCommand((e) => ResetBank()));
        public ICommand DepSelectedCommand => depSelectedCommand ?? (depSelectedCommand = new RelayCommand(
            action: (e) => ClientSortEnabled = RemoveDepEnabled = (Clients = (e as ListView).SelectedItem is Dep dep ? (Dep = dep).Clients : null) != null));
        public ICommand ClientSelectedCommand => clientSelectedCommand ?? (clientSelectedCommand = new RelayCommand(
            action: (e) => AccSortEnabled = RemoveClientEnabled = (Accounts = (e as ListView).SelectedItem is Client client ? (Client = client).Accounts : null) != null));
        public ICommand AccSelectedCommand => accSelectedCommand ?? (accSelectedCommand = new RelayCommand(
            action: (e) => RemoveAccEnabled = (Account = ((e as ListView).SelectedItem is Account account) ? account : null) != null));
        public ICommand AddDepCommand => addDepCommand ?? (addDepCommand = new RelayCommand((e) =>
        {
            Bank.Deps.Add(new Dep());
            GridViewColumn column = (e as MainWindow).depNameColumn;
            column.Width = double.IsNaN(column.Width) ? column.ActualWidth : double.NaN;
            // Не хочет работать.
            //(e as MainWindow).depListView.UpdateLayout();
        }));
        public ICommand AddClientCommand => addClientCommand ?? (addClientCommand = new RelayCommand((e) =>
        {
            Dep.Clients.Add(new Client());
            GridViewColumn column = (e as MainWindow).clientNameСolumn;
            column.Width = double.IsNaN(column.Width) ? column.ActualWidth : double.NaN;
            // Не хочет работать.
            (e as MainWindow).clientListView.UpdateLayout();
        }));
        public ICommand AddAccountCommand => addAccountCommand ?? (addAccountCommand = new RelayCommand((e) => Client.Accounts.Add(new Account())));
        public ICommand RemoveDepCommand => removeDepCommand ?? (removeDepCommand = new RelayCommand(RemoveDep));
        public ICommand RemoveClientCommand => removeClientCommand ?? (removeClientCommand = new RelayCommand(RemoveClient));
        public ICommand RemoveAccCommand => removeAccCommand ?? (removeAccCommand = new RelayCommand(RemoveAcc));
        public ICommand SortByNameCommand => sortByNameCommand ?? (sortByNameCommand = new RelayCommand((e) => SortBy(e, "Name")));
        public ICommand AccSortByNmbCommand => accSortByNmbCommand ?? (accSortByNmbCommand = new RelayCommand((e) => SortBy(e, "Number")));
        public ICommand AccSortBySizeCommand => accSortBySizeCommand ?? (accSortBySizeCommand = new RelayCommand((e) => SortBy(e, "Size")));
        public ICommand AccSortByRateCommand => accSortByRateCommand ?? (accSortByRateCommand = new RelayCommand((e) => SortBy(e, "Rate")));
        #endregion
        public MainViewModel() => ResetBank();
        private void ResetBank()
        {
            Bank = RandomBank.GetBank();
            Account = null; Dep = null; Client = null;
            ClientSortEnabled = AccSortEnabled = RemoveDepEnabled = RemoveClientEnabled = RemoveAccEnabled = false;
        }
        #region Handlers
        private void RemoveDep(object commandParameter)
        {
            //if (commandParameter == null)
            //{
            //    commandParameter = ((Mouse.DirectlyOver as ScrollViewer).Content as ItemsPresenter);// FrameworkElement;
            //    if (commandParameter != null)
            //        commandParameter = ((FrameworkElement)commandParameter).DataContext;
            //    else return;
            //}
            if (Dep != null && MessageBox.Show("Удалить отдел?", "Удаление отдела " + Dep.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Bank.Deps.Remove(Dep);
            }
        }
        private void RemoveClient(object commandParameter)
        {
            if (Client != null && MessageBox.Show("Удалить клиента?", "Удаление клиента " + Client.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Dep.Clients.Remove(Client);
            }
        }
        private void RemoveAcc(object commandParameter)
        {
            if (Account != null && MessageBox.Show("Удалить счет?", "Удаление счета " + Account.Number, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Client.Accounts.Remove(Account);
            }
        }
        private void SortBy(object commandParameter, string PropName)
        {
            if (commandParameter == null) return;
            ListView listView = commandParameter as ListView;
            // Меняем порядок сортировки на противоположный.
            curDepListSortDirection = (ListSortDirection)(((int)curDepListSortDirection + 1) % 2);
            // Очищаем список сортировки.
            listView.Items.SortDescriptions.Clear();
            // Сортируем список отделов по имени.
            listView.Items.SortDescriptions.Add(new SortDescription(PropName, curDepListSortDirection));

        }
        #endregion
    }
}
