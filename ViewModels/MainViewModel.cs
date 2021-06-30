using ClassLibrary;
using FontAwesome.Sharp;
using System;
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
        private void DepSelected(object commandParameter) =>
            Clients = (commandParameter as ListView).SelectedItem is Dep dep ? (Dep = dep).Clients : null;
        private void ClientSelected(object commandParameter) =>
            Accounts = (commandParameter as ListView).SelectedItem is Client client ? (Client = client).Accounts : null;
        private void AccSelected(object commandParameter) =>
            Account = ((commandParameter as ListView).SelectedItem is Account account) ? account : null;
        private void AddDep(object commandParameter) => Bank.Deps.Add(new Dep());
        private void AddClient(object commandParameter) => Dep.Clients.Add(new Client());
        private void AddAccount(object commandParameter) => Client.Accounts.Add(new Account());
        private void RemoveDep(object commandParameter)
        {
            if (Dep != null && MessageBox.Show("Удалить отдел?", "Удаление отдела " + Dep.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Bank.Deps.Remove(Dep);
                Dep = null;
            }
        }
        private void RemoveClient(object commandParameter)
        {
            if (Client != null && MessageBox.Show("Удалить клиента?", "Удаление клиента " + Client.Name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Dep.Clients.Remove(Client);
                Client = null;
            }
        }
        private void RemoveAcc(object commandParameter)
        {
            if (Account != null && MessageBox.Show("Удалить счет?", "Удаление счета " + Account.Number, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Client.Accounts.Remove(Account);
                Account = null;
            }
        }
        #endregion
    }
}
