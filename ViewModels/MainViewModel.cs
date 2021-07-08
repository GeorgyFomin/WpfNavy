using ClassLibrary;
using FontAwesome.Sharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNavy.Commands;

namespace WpfNavy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public static void Log(string report)
        {
            using (TextWriter tw = File.AppendText("log.txt"))
                tw.WriteLine(DateTime.Now.ToString() + ":" + report);
        }
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
        private RelayCommand fromSelectedCommand;
        private bool fromIsSelected;
        private RelayCommand toSelectedCommand;
        private decimal transferAmount;
        private RelayCommand doTransferCommand;
        private bool transferEnabled;
        private Account accountFrom;
        private Account accountTo;
        private RelayCommand endDepEditCommand;
        private RelayCommand endClientEditCommand;
        private RelayCommand endAccEditCommand;
        private RelayCommand addingDepCommand;
        private RelayCommand addingAccCommand;
        private RelayCommand addingClientCommand;
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
        public ICommand DepSelectedCommand => depSelectedCommand ?? (depSelectedCommand = new RelayCommand((e) => ClientSortEnabled = RemoveDepEnabled =
        (Clients = (e is ListView ? (e as ListView).SelectedItem : (e as DataGrid).SelectedItem) is Dep dep ? (Dep = dep).Clients : null) != null));
        public ICommand ClientSelectedCommand => clientSelectedCommand ?? (clientSelectedCommand =
            new RelayCommand((e) => AccSortEnabled = RemoveClientEnabled =
            (Accounts = (e is ListView ? (e as ListView).SelectedItem : (e as DataGrid).SelectedItem) is Client client ? (Client = client).Accounts : null) != null));
        public ICommand AccSelectedCommand => accSelectedCommand ?? (accSelectedCommand = new RelayCommand((e) => RemoveAccEnabled =
        (Account = ((e is ListView ? (e as ListView).SelectedItem : (e as DataGrid).SelectedItem) is Account account) ? account : null) != null));
        public ICommand AddDepCommand => addDepCommand ?? (addDepCommand = new RelayCommand((e) =>
        {
            Dep dep;
            Bank.Deps.Add(dep = new Dep());
            AdjustColumnWidth(e as GridViewColumn);
            Log($"Добавлен отдел {dep}");
        }));
        public ICommand AddClientCommand => addClientCommand ?? (addClientCommand = new RelayCommand((e) =>
        {
            Client client;
            Dep.Clients.Add(client = new Client());
            AdjustColumnWidth(e as GridViewColumn);
            Log($"Добавлен клиент {client} в отдел {Dep}");
        }));
        public ICommand AddAccountCommand => addAccountCommand ?? (addAccountCommand = new RelayCommand((e) =>
        {
            Account account;
            Client.Accounts.Add(account = new Account());
            Log($"Добавлен счет {account} клиенту {Client} в отделе {Dep}");

        }));
        public ICommand RemoveDepCommand => removeDepCommand ?? (removeDepCommand = new RelayCommand(RemoveDep));
        public ICommand RemoveClientCommand => removeClientCommand ?? (removeClientCommand = new RelayCommand(RemoveClient));
        public ICommand RemoveAccCommand => removeAccCommand ?? (removeAccCommand = new RelayCommand(RemoveAcc));
        public ICommand SortByNameCommand => sortByNameCommand ?? (sortByNameCommand = new RelayCommand((e) => SortBy(e, "Name")));
        public ICommand AccSortByNmbCommand => accSortByNmbCommand ?? (accSortByNmbCommand = new RelayCommand((e) => SortBy(e, "Number")));
        public ICommand AccSortBySizeCommand => accSortBySizeCommand ?? (accSortBySizeCommand = new RelayCommand((e) => SortBy(e, "Size")));
        public ICommand AccSortByRateCommand => accSortByRateCommand ?? (accSortByRateCommand = new RelayCommand((e) => SortBy(e, "Rate")));
        public ICommand FromSelectedCommand => fromSelectedCommand ?? (fromSelectedCommand = new RelayCommand(FromSelected));
        public ICommand ToSelectedCommand => toSelectedCommand ?? (toSelectedCommand = new RelayCommand(ToSelected));
        public ICommand DoTransferCommand => doTransferCommand ?? (doTransferCommand = new RelayCommand(DoTransfer));
        public Account AccountFrom { get => accountFrom; set { accountFrom = value; RaisePropertyChanged(nameof(Bank.Accounts)); } }
        public Account AccountTo { get => accountTo; set { accountTo = value; RaisePropertyChanged(nameof(Bank.Accounts)); } }
        public bool FromIsSelected { get => fromIsSelected; set { fromIsSelected = value; RaisePropertyChanged(nameof(FromIsSelected)); } }
        public decimal TransferAmount { get => transferAmount; set { transferAmount = value; RaisePropertyChanged(nameof(TransferAmount)); } }
        public bool TransferEnabled { get => transferEnabled; set { transferEnabled = value; RaisePropertyChanged(nameof(TransferEnabled)); } }
        public ICommand EndDepEditCommand => endDepEditCommand ?? (endDepEditCommand = new RelayCommand((e) => Log($"Имя отдела {Dep} отредактировано.")));
        public ICommand EndClientEditCommand => endClientEditCommand ?? (endClientEditCommand = new RelayCommand((e) => Log($"Имя клиента {Client} отредактировано.")));
        public ICommand EndAccEditCommand => endAccEditCommand ?? (endAccEditCommand = new RelayCommand((e) => Log($"Счет {Account} отредактирован.")));
        public ICommand AddingDepCommand => addingDepCommand ?? (addingDepCommand = new RelayCommand((e) => Log($"К банку добавлен новый отдел.")));
        public ICommand AddingClientCommand => addingClientCommand ?? (addingClientCommand = new RelayCommand((e) => Log($"В отдел {Dep} добавлен новый клиент.")));
        public ICommand AddingAccCommand => addingAccCommand ?? (addingAccCommand = new RelayCommand((e) => Log($"У клиента {Client} открыт новый счет.")));
        #endregion
        public MainViewModel() => ResetBank();
        private void ResetBank()
        {
            Bank = RandomBank.GetBank();
            Account = null; Dep = null; Client = null;
            FromIsSelected = TransferEnabled =
            ClientSortEnabled = AccSortEnabled = RemoveDepEnabled = RemoveClientEnabled = RemoveAccEnabled = false;
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
        private void AdjustColumnWidth(GridViewColumn column)
        {
            for (int i = 0; i < 2; i++)
                column.Width = double.IsNaN(column.Width) ? column.ActualWidth : double.NaN;
        }
        #region Handlers
        private void RemoveDep(object commandParameter)
        {
            if (Dep != null && MessageBox.Show("Удалить отдел?", "Удаление отдела " + Dep.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Bank.Deps.Remove(Dep);
                Log($"Удален отдел {Dep}");
            }
        }
        private void RemoveClient(object commandParameter)
        {
            if (Client != null && MessageBox.Show("Удалить клиента?", "Удаление клиента " + Client.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Dep.Clients.Remove(Client);
                Log($"Удален клиент {Client}");
            }
        }
        private void RemoveAcc(object commandParameter)
        {
            if (Account != null && MessageBox.Show("Удалить счет?", "Удаление счета " + Account.Number, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _ = Client.Accounts.Remove(Account);
                Log($"Удален счет {Account}");
            }
        }
        private void FromSelected(object commandParameter)
        {
            Account selAccount = (commandParameter as ComboBox).SelectedValue as Account;
            AccountFrom = Bank.Accounts.FirstOrDefault((e) => e.ID == selAccount.ID);
            FromIsSelected = AccountFrom != null;
        }
        private void ToSelected(object commandParameter)
        {
            Account selAccount = (commandParameter as ComboBox).SelectedValue as Account;
            if (selAccount.ID == AccountFrom.ID)
            {
                MessageBox.Show("Нельзя делать перевод внутри одного и того же счета!!");
                return;
            }
            AccountTo = Bank.Accounts.FirstOrDefault((e) => e.ID == selAccount.ID);
            TransferEnabled = AccountTo != null;
        }
        private void DoTransfer(object commandParameter)
        {
            if (MessageBox.Show($"Вы действительно хотите перевести со счета №{AccountFrom.Number} на счет №{AccountTo.Number} сумму {TransferAmount}?",
                "Перевод между счетами", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            AccountFrom.Size -= TransferAmount;
            AccountTo.Size += TransferAmount;
            Log($"Со счета {AccountFrom} переведено {TransferAmount} на счет {AccountTo}");
        }
        #endregion
    }
}
