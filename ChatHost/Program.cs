using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; //Подключаем для ServiceHost

//В App.Config скопирован конфиг из конфига сервиса (GodPlease), сервис работает на протоколе netTCP,
//хост на http, указан behaviorname, всё для обмена метаданными, всё работает на локалхосте, для реальной машины
//нужно указывать айпи адрес вместо локалхоста

namespace ChatHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(GodPlease.ServiceChat))) //Инициализация хоста, кэширование в переменную хост
            {
                host.Open(); //Открыть хост
                Console.WriteLine("Хост стартовал!"); //Вывести сообщение в консоль
                Console.ReadLine(); //Дождаться нажатия клавиши, чтобы консоль не закрылась
            }
        }
    }
}
