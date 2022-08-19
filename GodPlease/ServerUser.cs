using System.ServiceModel; //Для OperationContext

namespace GodPlease
{
    class ServerUser
    {
        public int ID { get; set; } //Айди юзера (конструктор класса)

        public string Name { get; set; } //Имя юзера (конструктор класса)

        public OperationContext operationContext { get; set; }//Нужен для доступа к Callback'у (соединения сервера и клиента)
    }
}
