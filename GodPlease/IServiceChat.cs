using System; //Импортируем библиотеки (стандартные + ServiceModel чтобы не прописывать перед ServiceContract и OperationContract)
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GodPlease //Описывается функционал
{
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))] //Определяет какие операции поддерживает сервис, параметр означает, что присутствует метод Callback, описание Callback ниже
    public interface IServiceChat 
    {
        [OperationContract] //Атрибут, позволяющий клиенту вызывать метод
        int Connect(string name); //Метод подключения

        [OperationContract] //Атрибут, позволяющий клиенту вызывать метод
        void Disconnect(int id); //Метод отключения

        [OperationContract(IsOneWay = true)] //Атрибут, позволяющий клиенту вызывать метод (IsOneWay - не нужно дожидаться ответа от сервера)
        void SendMsg(string msg, int id); //Метод отправки сообщений
    }

    public interface IServerChatCallback //Отельный класс Callback для рассылки сообщений - вызов действия на стороне клиента с сервера, описание на сервере, реализация в клиенте
    {
        [OperationContract(IsOneWay = true)] //
        void MsgCallback(string msg);
    }
}
