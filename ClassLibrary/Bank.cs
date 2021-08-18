﻿using System.Collections.ObjectModel;
using System.IO;

namespace ClassLibrary
{
    public class Bank : Named
    {
        /// <summary>
        /// Хранит список отделов банка.
        /// </summary>
        private ObservableCollection<Dep> deps;
        /// <summary>
        /// Устанавливает и возвращает ссылки на отделы банка.
        /// </summary>
        public ObservableCollection<Dep> Deps => deps ?? (deps = new ObservableCollection<Dep>());
        public Bank() : base() { }
        public Bank(string name = null, ObservableCollection<Dep> deps = null) : base(name) => this.deps = deps;
        public override string ToString() => "Bank " + base.ToString();
        public ObservableCollection<Account> Accounts
        {
            get
            {
                ObservableCollection<Account> accounts = new ObservableCollection<Account>();
                foreach (Dep dep in deps)
                {
                    foreach (Client client in dep.Clients)
                    {
                        foreach (Account account in client.Accounts)
                        {
                            accounts.Add(account);
                        }
                    }
                }
                return accounts;
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
