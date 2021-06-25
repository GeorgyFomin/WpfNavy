using ClassLibrary;
using FontAwesome.Sharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfNavy.Commands;

namespace WpfNavy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private Bank bank;
        private ObservableCollection<Dep> deps;
        private ObservableCollection<Client> clients;
        private ObservableCollection<Account> accounts;
        private string bankName;
        private RelayCommand dragCommand;
        private RelayCommand minimizeCommand;
        private RelayCommand maximizeCommand;
        private RelayCommand closeCommand;
        private RelayCommand resetBankCommand;
        private RelayCommand depSelectedCommand;
        private RelayCommand clientSelectedCommand;
        private RelayCommand addDepCommand;
        #endregion
        #region Properties
        public string BankName
        {
            get => bankName;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                bank.Name = value; bankName = bank.Name; RaisePropertyChanged(nameof(BankName));
            }
        }
        public ObservableCollection<Dep> Deps { get => deps; set { bank.Deps = value; deps = bank.Deps; RaisePropertyChanged(nameof(Deps)); } }
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
        private void AddDep(object commandParameter)
        {
            Deps.Add(new Dep());
        }
        #endregion
    }
}
