using System.Collections.ObjectModel;
using System.IO;

namespace ClassLibrary
{
    public class Client : Named
    {
        /// <summary>
        /// Хранит список вкладов.
        /// </summary>
        private ObservableCollection<Account> accounts;
        #region Properties
        /// <summary>
        /// Устанавливает и возвращает ссылки на счета клиента.
        /// </summary>
        public ObservableCollection<Account> Accounts => accounts ?? (accounts = new ObservableCollection<Account>());
        #endregion
        public Client() : base() { }
        public Client(string name = null, ObservableCollection<Account> accounts = null) : base(name) => this.accounts = accounts;
        /// <summary>
        /// Печатает сведения о клиенте.
        /// </summary>
        /// <param name="tw">Райтер.</param>
        public void Print(TextWriter tw)
        {
            // Печатаем информацию о клиенте.
            tw.WriteLine("Клиент\n" + Info() + "\nСчета");
            tw.WriteLine(Account.header);
            // Печатаем сведения о счетах.
            foreach (Account account in Accounts)
            {
                account.PrintFields(tw);
            }
        }
    }
}
