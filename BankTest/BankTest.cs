using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace BankTest
{
    class BankTest
    {
        static readonly Random random = new Random();
        static void Main(string[] args)
        {
            Bank bank;
            do
            {
                // Организация.
                // Создаем случайный банк.
                bank = RandomBank.GetBank();
                // Печатаем данные об организации.
                bank.Print(Console.Out);
                string bankName = bank.Name;
                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadKey(true);
                #region Serialization & Deserialization Test
                Console.WriteLine("Сериализация и десериализация.");
                Console.WriteLine("В формате XML (Y/N)?");
                bool mode = Console.ReadKey(true).KeyChar == 'Y';
                string ext = mode ? ".xml" : ".json";// Расширение файла.
                Console.WriteLine("\t\t\tВ формате " + (mode ? "XML." : "Json."));
                #region Bank Test
                // Организация
                Console.WriteLine("\t\tОрганизация в целом.");
                string path = "bank " + bankName + ext;// Маршрут к файлу.
                XmlJsonStudio.Serialize(path, bank, mode);// Сериализуем организацию.
                Console.WriteLine("Сериализация организации проведена.");
                Bank reBank = XmlJsonStudio.Deserialize<Bank>(path, mode);// Восстановленная организация.
                Console.WriteLine("Восстановленная организация.");
                // Печатаем восстановленную организацию.
                reBank.Print(Console.Out);
                #endregion
                if (bank.Deps.Count > 0)
                {
                    Console.WriteLine("Нажмите любую клавишу для работы со случайным отделом.");
                    Console.ReadKey(true);
                    #region Dep Test
                    // Отдел
                    //Выбираем случайный отдел.
                    int depNmb = random.Next(bank.Deps.Count);
                    Dep dep = bank.Deps[depNmb];
                    Console.WriteLine($"\nОтдел {dep.Name}.");
                    path = "dep " + dep.Name + ext;
                    XmlJsonStudio.Serialize(path, dep, mode);
                    Console.WriteLine($"Сериализация отдела {dep.Name} проведена. Нажмите любую клавишу для продолжения.");
                    Dep reDep = XmlJsonStudio.Deserialize<Dep>(path, mode);// Восстановленный отдел.
                    Console.WriteLine("Восстановленнй отдел.");
                    reDep.Print(Console.Out);
                    #endregion
                    Console.WriteLine("Нажмите любую клавишу для работы со случайным клиентом.");
                    Console.ReadKey(true);
                    #region Client Test
                    // Выбираем случайного клиента.
                    int clientNmb = random.Next(dep.Clients.Count);// Номер клиента
                    Client client = dep.Clients[clientNmb];
                    Console.WriteLine($"\nКлиент по имени {client.Name} из отдела {dep.Name}");
                    // Маршрут к файлу.
                    path = dep.Name + "." + client.Name + ext;
                    // Сериализуем его.
                    XmlJsonStudio.Serialize(path, client, mode);
                    Console.WriteLine($"Сериализация клиента по имени {client.Name} из отдела {dep.Name} проведена.");
                    // Воссоздаем менеджера из файла XML или Json.
                    Client reClient = XmlJsonStudio.Deserialize<Client>(path, mode);// Восстановленный клиент.
                    Console.WriteLine(
                        $"Данные о клиенте по имени {reClient.Name} из отдела {dep.Name} после сериализации и восстановления в " + (mode ? "xml" : "json") + "-формате.");
                    // Печатаем на экране.
                    reClient.Print(Console.Out);
                    #endregion
                    Console.WriteLine("Нажмите любую клавишу для работы со случайным счетом.");
                    Console.ReadKey(true);
                    #region Account Test
                    // Используем первого клиента.
                    client = dep.Clients[0];
                    // Выбираем случайный счет.
                    int accountNmb = random.Next(client.Accounts.Count);// Номер счета.
                    Account account = client.Accounts[accountNmb];
                    Console.WriteLine($"\nСчет № {account.Number} от клиента {client.Name}");
                    // Маршрут к файлу.
                    path = client.Name + "." + account.Number + ext;
                    // Сериализуем его.
                    XmlJsonStudio.Serialize(path, account, mode);
                    Console.WriteLine($"Сериализация счета № {account.Number} от клиента {client.Name} проведена.");
                    // Воссоздаем работника из файла XML или Json.
                    Account reAccount = XmlJsonStudio.Deserialize<Account>(path, mode);// Восстановленный счет.
                    Console.WriteLine(
                        $"Данные о счете {reAccount.Number} от клиента {client.Name} после сериализации и восстановления в " + (mode ? "xml" : "json") + "-формате.");
                    // Печатаем на экране.
                    reAccount.Print(Console.Out);
                    #endregion
                }
                #endregion
                Console.WriteLine("\nДля новой выборки нажмите любую клавишу. Для завершения нажмите esc.");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
