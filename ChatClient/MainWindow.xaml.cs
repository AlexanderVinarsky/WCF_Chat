using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat; //Для ServiceChatClient

namespace ChatClient
{
    public partial class MainWindow : Window, IServiceChatCallback //Даём знать клиенту о методе Callback из сервиса
    {
        bool isConnected = false; //Статус подключения - отключён
        ServiceChatClient client; //Инициализация клиента
        int ID; //Переменная Айди
        public MainWindow()
        {
            InitializeComponent(); //Инциализация окна пользовательского интерфейса
        }

        

        void ConnectUser()
        {
            if(!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this)); //инициалиация объекта client
                ID = client.Connect(tbUserName.Text); // айди возвращается из метода клиента, который вызывается с использованием имени юзера
                tbUserName.IsEnabled = false; //Выключение текстбокса с именем юзера
                bConDiscon.Content = "Disconnect"; //Изменение надписи у кнопки на Disconnect
                isConnected = true; //Статус подключения - подключён
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID); //Отключение клиента по айди
                client = null; // деинициализация объекта client
                tbUserName.IsEnabled = true; //Включение текстбокса с именем юзера
                bConDiscon.Content = "Connect"; //Изменение надписи у кнопки на Connect
                isConnected = false; //Статус подключения - отключён
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Обработка нажатия клавиши
        {
            if (isConnected) //Если подключён
            {
                DisconnectUser(); //Отключить
            }
            else //Если отключён
            {
                ConnectUser(); //Подключить
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg); //Добавить сообщение в листбокс
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]); //Скроллинг до самого нового при появлении новых сообщений, что не помещаются
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) //Обработка закрытия приложения через крестик (добавлено через свойства объекта через интерфейс иде)
        {
            DisconnectUser(); //Вызов метода отключения юзера
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e) //Обработка нажатия клавиш в текстбоксе сообщения (добавлено через свойства объекта через интерфейс иде)
        {
            if (e.Key == Key.Enter) //Если нажат Enter
            {
                if (client != null) //Если клиент инициализирован (то есть юзер подключился)
                {
                    client.SendMsg(tbMessage.Text, ID); //Вызов SendMsg из класса клиент - то есть отправить сообщение
                    tbMessage.Text = string.Empty; //Очистить строку в текстбоксе
                }
            }
        }
    }
}
