using ClassLibrary;
using FontAwesome.Sharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #endregion
        #region Properties
        public Bank Bank { get => bank; set { bank = value; RaisePropertyChanged(nameof(Bank)); } }
        public Dep Dep { get => dep; set { dep = value; RaisePropertyChanged(nameof(Dep)); } }
        public Client Client { get => client; set { client = value; RaisePropertyChanged(nameof(Client)); } }
        public Account Account { get => account; set { account = value; RaisePropertyChanged(nameof(Account)); } }
        public ObservableCollection<Client> Clients { get => clients; private set { clients = value; RaisePropertyChanged(nameof(Clients)); } }
        public ObservableCollection<Account> Accounts { get => accounts; private set { accounts = value; RaisePropertyChanged(nameof(Accounts)); } }
        public ICommand DragCommand => dragCommand ?? (dragCommand = new RelayCommand(Drag));
        public ICommand MinimizeCommand => minimizeCommand ?? (minimizeCommand = new RelayCommand(Minimize));
        public ICommand MaximizeCommand => maximizeCommand ?? (maximizeCommand = new RelayCommand(Maximize));
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(Close));
        public ICommand ResetBankCommand => resetBankCommand ?? (resetBankCommand = new RelayCommand(ResetBank));
        public ICommand DepSelectedCommand => depSelectedCommand ?? (depSelectedCommand = new RelayCommand(DepSelected));
        public ICommand ClientSelectedCommand => clientSelectedCommand ?? (clientSelectedCommand = new RelayCommand(ClientSelected));
        public ICommand AddDepCommand => addDepCommand ?? (addDepCommand = new RelayCommand(AddDep));
        public ICommand AddClientCommand => addClientCommand ?? (addClientCommand = new RelayCommand(AddClient));
        public ICommand AddAccountCommand => addAccountCommand ?? (addAccountCommand = new RelayCommand(AddAccount));
        public ICommand RemoveDepCommand => removeDepCommand ?? (removeDepCommand = new RelayCommand(RemoveDep));
        public ICommand RemoveClientCommand => removeClientCommand ?? (removeClientCommand = new RelayCommand(RemoveClient));
        public ICommand RemoveAccCommand => removeAccCommand ?? (removeAccCommand = new RelayCommand(RemoveAcc));
        public ICommand AccSelectedCommand => accSelectedCommand ?? (accSelectedCommand = new RelayCommand(AccSelected));
        #endregion
        public MainViewModel() => ResetBank();
        private void ResetBank() => Bank = RandomBank.GetBank();
        #region Handlers
        private void Drag(object commandParameter) => (commandParameter as MainWindow).DragMove();
        private void Minimize(object commandParameter) => (commandParameter as MainWindow).WindowState = WindowState.Minimized;
        private void Maximize(object commandParameter)
        {
            MainWindow window = commandParameter as MainWindow;
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            window.MaxIconBlock.Icon = window.WindowState == WindowState.Maximized ? IconChar.WindowRestore : IconChar.WindowMaximize;
        }
        private void Close(object commandParameter) => (commandParameter as MainWindow).Close();
        private void ResetBank(object commandParameter) => ResetBank();
        private void DepSelected(object commandParameter)
        {
            if ((commandParameter as ListView).SelectedItem is Dep dep && dep != null)
            {
                Clients = dep.Clients;
                if (MessageBox.Show("Удалить отдел?", "Удаление отдела " + dep.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Bank.Deps.Remove(dep);
                    dep = null;
                }
                Dep = dep;
            }
            else
                Clients = null;
        }
        private void ClientSelected(object commandParameter)
        {
            if ((commandParameter as ListView).SelectedItem is Client client && client != null)
            {
                Accounts = client.Accounts;
                if (MessageBox.Show("Удалить клиента?", "Удаление клиента " + client.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Dep.Clients.Remove(client);
                    client = null;
                }
                Client = client;
            }
            else
                Accounts = null;
        }
        private void AddDep(object commandParameter) => Bank.Deps.Add(new Dep());
        private void AddClient(object commandParameter) => Dep.Clients.Add(new Client());
        private void AddAccount(object commandParameter) => Client.Accounts.Add(new Account());
        private void RemoveDep(object commandParameter) => Bank.Deps.Remove(Dep);
        private void RemoveClient(object commandParameter) => Dep.Clients.Remove(Client);
        private void RemoveAcc(object commandParameter) => Client.Accounts.Remove(Account);
        private void AccSelected(object commandParameter)
        {
            Account = ((commandParameter as MainWindow).accListView.SelectedItem is Account account) ? account : null;
            if (Account != null && MessageBox.Show("Удалить счет?", "Удаление счета " + Account.Number, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Client.Accounts.Remove(Account);
        }
        #endregion
    }
}
