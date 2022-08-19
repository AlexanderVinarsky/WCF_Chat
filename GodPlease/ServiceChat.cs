using System; //Импортируем библиотеки (стандартные + ServiceModel чтобы не прописывать перед ServiceContract и OperationContract, ServiceBehabiour, InstanceContextMode)
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

//

namespace GodPlease
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] //Для создания единого экземпляра сервиса для всех клиентов (по умолчанию - у каждого свой)
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>(); //Создание списка юзеров
        int nextId = 1; //Начальный айди следующего задан равным единице
        
        public int Connect(string name) //Реализация метода Connect
        {
            ServerUser user = new ServerUser() //Инициализация очередного юзера при подключении
            {
                ID = nextId, //Инициализация айди из генерации nextId
                Name = name, //Инициализация имени
                operationContext = OperationContext.Current //Присваивает текущий OperationContext (пояснение в ServerUser.cs)
            };
            nextId++; //Инкремент следующего айди

            SendMsg(": "+user.Name + " подключился к чату!", 0); //Отправить сообщение: %юзернейм% подключился к чату
            users.Add(user); //Добавить юзера в список
            return user.ID; //Вернуть айди юзера
        }

        public void Disconnect(int id) //Реализация метода отключения от чата
        {
            var user = users.FirstOrDefault(i => i.ID == id); //Поиск юзера в списке по айди
            if (user!=null) //Если найден
            {
                users.Remove(user); //Убрать из списка
                SendMsg(": "+user.Name+" покинул чат!", 0); //Вывести в чат, что юзер (по имени) покинул его
            }
        }

        public void SendMsg(string msg, int id) //Реализация метода отправки сообщения
        {
            foreach (var item in users) //Для каждого юзера в списке
            {
                string answer = DateTime.Now.ToShortTimeString(); //Инициализировать строку времяенем отправки сообщения ЧЧ:ММ
                var user = users.FirstOrDefault(i => i.ID == id);  //Поиск юзера в списке по айди
                if (user != null) //Если найден
                {
                    answer += ": " + user.Name + " "; //Добавить в строку ": юзернейм "
                }

                answer += msg; //Добавить в строку сообщение юзера

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer); //Вызов Callback сервиса (реализация на стороне клиента) - передача интерфейса и вызов метода внутри него с аргументом в виде строки
            }
        }
    }
}
