using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfNavy.Commands
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> action;
        readonly bool canExecute = true;
        public RelayCommand(Action<object> action) => this.action = action;
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public bool CanExecute(object parameter) => canExecute;
        public void Execute(object parameter) => action?.Invoke(parameter);
    }
}
