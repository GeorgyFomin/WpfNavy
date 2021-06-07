using System.Collections.Generic;
using System.IO;

namespace ClassLibrary
{
    public class Bank : Named
    {
        /// <summary>
        /// Хранит список отделов банка.
        /// </summary>
        private List<Dep> deps = new List<Dep>();
        /// <summary>
        /// Устанавливает и возвращает ссылки на отделы банка.
        /// </summary>
        public List<Dep> Deps { get => deps; set { deps = value ?? new List<Dep>(); } }
        public Bank() : base() { }
        public Bank(string name = null, List<Dep> deps = null) : base(name) => Deps = deps;
        public override string ToString()
        {
            return "Bank " + base.ToString();
        }
        /// <summary>
        /// Возвращает список всех депозитов банка.
        /// </summary>
        public List<Account> Deposits
        {
            get
            {
                List<Account> deposits = new List<Account>();
                foreach (Dep dep in deps)
                {
                    foreach (Client client in dep.Clients)
                    {
                        foreach (Account account in client.Accounts)
                        {
                            if (account.Size > 0)
                                deposits.Add(account);
                        }
                    }
                }
                return deposits;
            }
        }
        /// <summary>
        /// Возвращает список всех кредитов банка.
        /// </summary>
        public List<Account> Loans
        {
            get
            {
                List<Account> loans = new List<Account>();
                foreach (Dep dep in deps)
                {
                    foreach (Client client in dep.Clients)
                    {
                        foreach (Account account in client.Accounts)
                        {
                            if (account.Size < 0)
                                loans.Add(account);
                        }
                    }
                }
                return loans;
            }
        }
        /// <summary>
        /// Возвращает список всех клиентов банка.
        /// </summary>
        public List<Client> Clients
        {
            get
            {
                List<Client> clients = new List<Client>();
                foreach (Dep dep in Deps)
                {
                    clients.AddRange(dep.Clients);
                }
                return clients;
            }
        }
        /// <summary>
        /// Печатает сведения об отделе.
        /// </summary>
        /// <param name="tw">Райтер.</param>
        public void Print(TextWriter tw)
        {
            // Печатаем информацию о банке.
            tw.WriteLine(this);
            // Печатаем сведения об отделах.
            foreach (Dep dep in Deps)
            {
                dep.Print(tw);
            }
        }

    }
}
