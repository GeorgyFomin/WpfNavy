using ClassLibrary;
using FontAwesome.Sharp;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfNavy.Commands;

namespace WpfNavy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private Bank bank;
        private List<Dep> deps;
        private List<Client> clients;
        private List<Account> accounts;
        private string bankName;
        private RelayCommand dragCommand;
        private RelayCommand minimizeCommand;
        private RelayCommand maximizeCommand;
        private RelayCommand closeCommand;
        private RelayCommand resetBankCommand;
        private RelayCommand depSelectedCommand;
        private RelayCommand clientSelectedCommand;
        #endregion
        #region Properties
        public string BankName
        {
            get => bankName; set
            {
                if (string.IsNullOrEmpty(value)) return;
                bank.Name = value; bankName = bank.Name; RaisePropertyChanged(nameof(BankName));
            }
        }
        public List<Dep> Deps { get => deps; private set { deps = value; RaisePropertyChanged(nameof(Deps)); } }
        public List<Client> Clients { get => clients; private set { clients = value; RaisePropertyChanged(nameof(Clients)); } }
        public List<Account> Accounts { get => accounts; private set { accounts = value; RaisePropertyChanged(nameof(Accounts)); } }
        public ICommand DragCommand => dragCommand ?? (dragCommand = new RelayCommand(Drag));
        public ICommand MinimizeCommand => minimizeCommand ?? (minimizeCommand = new RelayCommand(Minimize));
        public ICommand MaximizeCommand => maximizeCommand ?? (maximizeCommand = new RelayCommand(Maximize));
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(Close));
        public ICommand ResetBankCommand => resetBankCommand ?? (resetBankCommand = new RelayCommand(ResetBank));
        public ICommand DepSelectedCommand => depSelectedCommand ?? (depSelectedCommand = new RelayCommand(DepSelected));
        public ICommand ClientSelectedCommand => clientSelectedCommand ?? (clientSelectedCommand = new RelayCommand(ClientSelected));
        #endregion
        public MainViewModel() => ResetBank();
        private void ResetBank()
        {
            bank = RandomBank.GetBank();
            BankName = bank.Name;
            Deps = bank.Deps;
        }
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
            Clients = ((commandParameter as MainWindow).depListView.SelectedItem is Dep dep) ? dep.Clients : null;
        private void ClientSelected(object commandParameter) =>
            Accounts = ((commandParameter as MainWindow).clientListView.SelectedItem is Client client) ? client.Accounts : null;
        #endregion
    }
}
