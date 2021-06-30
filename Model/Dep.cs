﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfNavy.Commands;

namespace ClassLibrary
{
    public class Dep : Named
    {
        /// <summary>
        /// Хранит список клиентов.
        /// </summary>
        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        #region Properties
        /// <summary>
        /// Устанавливает и возвращает ссылки на клиентов.
        /// </summary>
        public ObservableCollection<Client> Clients { get => clients; set => clients = value ?? new ObservableCollection<Client>(); }
        public Dep() : base() { }
        #endregion
        public Dep(string name = null, ObservableCollection<Client> clients = null) : base(name)
        {
            Clients = clients;
        }

        /// <summary>
        /// Печатает сведения об отделе.
        /// </summary>
        /// <param name="tw">Райтер.</param>
        public void Print(TextWriter tw)
        {
            // Печатаем информацию об отделе.
            tw.WriteLine("Отдел " + this);
            // Печатаем сведения о клиентах.
            foreach (Client client in Clients)
            {
                client.Print(tw);
            }
        }
    }
}
