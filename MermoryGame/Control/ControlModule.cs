using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using CourseWork;
using MermoryGame.View.UpgradeControls;

namespace MermoryGame.Control
{
    public class
        ControlModule : IObserver //Заготовка под управляющий класс которая будет выполнять какие-либо действия при
        //действии пользователя или изменениях в базе данных
        //Уровень C-Control в паттерне MVC
        //Наследуем наш интерфейс наблюдателя(читай подробнее в его объявлении)
    {
        private readonly GameHubForm _gameHubForm; //Переменная для хранения формы игровой комнаты ожидания
        private LoginForm _loginForm; //Переменная для временного хранения активной формы авторизации

        private readonly List<User>
            users = new List<User>(); //Сам список пользователей (без модификатора доступа - значит private)

        public ControlModule() //Конструктор контролирующего модуля(вызывается при создании экземпляра этого класса)
        {
            GetUsers(); //Обновляем из Xml файла информацию о пользователях
            _gameHubForm = new GameHubForm(); //Создаём окно игровой комнаты
            _gameHubForm.RegisterObserver(this); //Связывем наблюдаемый объект и наблюдателя, для взаимного
            //взаимодействия между окнами при условии что окно не имеет представления о контролирующем модуле

            Application
                .EnableVisualStyles(); //Включаем для приложения визуальные стили(по умолчанию эта строка генерируется в Programm.cs сама)
            //Application.SetCompatibleTextRenderingDefault(false); //как и эта но почему-то выдаёт ошибку без неё вроде хорошо работает
            Application.Run(_gameHubForm); //Ну и определяю что главное окно на котором будет базироваться приложение -
            //это окно игровой комнаты, то есть приложение закроется и всё что внутри при закрытии этого окна
        }

        public void Update(object[] ob) //Функция имплементирующаяся от интерфейса наблюдателя (вызывается когда в
            //наблюдаемых объектах происходят изменения и передаёт их
        {
            var Params = (string[]) ob; //переданные подробности изменений
            if (Params[0] == "TryLogin") //Если Окно авторизации отправило запрос на попытку авторизации
            {
                GetUsers(); //На всякий случай обновляем список пользователей
                foreach (var user in users) //Проходимся по всем пользователям из списка
                    if (user.IsThisUsername(Params[1])
                        ) //Для каждого пользователя узнаём совпадает ли его логин с введённым
                        //в поле для ввода логина на форме входа
                    {
                        if (user.IsThisPassword(Params[2])
                        ) //Если для какого-то пользователя совпал логин, то должен совпадать и пароль
                        {
                            //Если это так, то
                            MessageBox.Show("Успешная авторизация"); //Выводим сообщение о успешном входе
                            _gameHubForm.ActiveUsername
                                .Add(Params[1]); //В комнату ожидания добавляем нового пользователя
                            _gameHubForm.PlayersButton[_gameHubForm.ActiveUsername.Count - 1]
                                .Show(); //Проявляем кнопку отвечающую теперь
                            //за только что авторизированного пользователя
                            _gameHubForm.PlayersButton[_gameHubForm.ActiveUsername.Count - 1].Text =
                                Params[1]; //Устанавливаем текст этой кнопки как
                            //логин пользователя
                            System.Windows.Forms.Control
                                AddPlayersButton; //Создаём временную переменную для последующего хранения в ней ссылки на
                            //кнопку добавления пользователя в комнату
                            _gameHubForm.tryGetControlWithName("AddPlayerButton",
                                out AddPlayersButton); //Получаем через метод из AbsForm
                            ((UpgradeButton) AddPlayersButton).Move(AddPlayersButton.Left,
                                AddPlayersButton.Top + 65); //Перемещаем эту кнопку
                            if (_gameHubForm.ActiveUsername.Count - 1 == 3)
                                AddPlayersButton.Hide(); //Если достигнуто макс. число игроков - прячем кнопку
                            _loginForm.Close(); //Закрываем окно авторизации
                            return; //Завершаем выполнение этой функции
                        }

                        //Если пароль всё же не подошёл
                        MessageBox.Show("Неверный логин или пароль", "Ошибка"); //Выводим на экран сообщение о ошибке
                        return; //Завершаем выполнение этой функции
                    }

                //Если не было найдено пользователя с введённым логином, то
                MessageBox.Show("Неверный логин или пароль", "Ошибка"); //Выводим сообщение о ошибке
                return; //Завершаем выполнение этой функции
            }

            if (Params[0] == "TryRegister") //Если Окно авторизации отправило запрос на попытку регистрации
            {
                GetUsers(); //На всякий случай обновляем список пользователей
                foreach (var user in users) //Проходимся по всем пользователям из списка
                    if (user.IsThisUsername(Params[1])) //Если для какого-то пользователя совпал логин, то
                    {
                        MessageBox.Show("Данный логин уже занят", "Ошибка"); //Выводим сообщение о ошибке
                        return; //Завершаем выполнение этой функции
                    }

                //Если не было найдено пользователей с таким же логином, то
                while (TryRegisterNewUser(Params[1], Params[2]) == false)
                    ; //Регестрируем нового пользователя с введёнными данными(смотри реализацию ниже)
                //Написано именно так, потому что Функция может не выолниться если xml файл занят другим процессом, в этом случае оно будет ждать до момента пока не
                //сможет успешно провести операцию, однако если файл повреждён может выполняться бесконечно
                MessageBox.Show(
                    "Теперь вы авторизированы. Войдите"); //Выводим сообщение о успешной регистрации(теперь пользователь должен войти)
                return; //Завершаем выполнение этой функции
            }

            if (Params[0] == "AddPlayer"
            ) //Если Окно игровой комнаты отправило запрос на Присоединение к игре нового игрока, то
            {
                if (_loginForm != null && _loginForm.Visible)
                    _loginForm.Close(); //Если у нас существует и открыто окно авторизаци то закрываем его
                _loginForm = new LoginForm(); //Создаём новый экземпляр класса формы авторизации
                _loginForm.RegisterObserver(this); //Добовляем наблюдение за процессами
                _loginForm.Owner =
                    _gameHubForm; //Явно на всякий случай указал что это окно является дочерним для окна игровой комнаты
                _loginForm.Show(); //Вывожу на экран окно регистрации (не закрывая окно игровой комнаты, значит окно авторизации - модальное)
                return; //Завершаем выполнение этой функции
            }

            if (Params[0] == "StartGame") //Если Окно игровой комнаты отправило запрос на начало игры, то
            {
                MessageBox.Show("Затычка, пока не готово"); //пока ничего
                return;
            }

            if (Params[0] == "LogoutPlayer"
            ) //Если Окно игровой комнаты отправило запрос на выход из комнаты некоторого игрока
            {
                _gameHubForm.ActiveUsername.RemoveAt(int.Parse(Params[1]) -
                                                     1); //то удаляем его из пользователей игровой комнаты
                //Значения на кнопках смещаем на один назад для кнопок пользователей идущих после удалённого
                for (var i = int.Parse(Params[1]); i < _gameHubForm.ActiveUsername.Count; i++)
                    _gameHubForm.ActiveUsername[i] = _gameHubForm.ActiveUsername[i + 1];
                _gameHubForm.PlayersButton[_gameHubForm.ActiveUsername.Count].Hide(); //Прячем освободившуюся кнопку
                //По аналогии как в TryLogin, только обратные действия
                System.Windows.Forms.Control AddPlayersButton;
                _gameHubForm.tryGetControlWithName("AddPlayerButton", out AddPlayersButton);
                ((UpgradeButton) AddPlayersButton).Move(AddPlayersButton.Left, AddPlayersButton.Top - 65);
                if (_gameHubForm.ActiveUsername.Count == 3) AddPlayersButton.Show();
            }
        }

        ~ControlModule() //Деструктор класса (Вызывается при удалении. Например когда программа завершается и она автоматически очищает ресурсы)
        {
            _loginForm.RemoveObserver(this); //Прекращаем следить за наблюдаемыми объектами
            users.Clear(); //Очищаем список пользователей которых мы считали и держим в этом классе
        }

        private void GetUsers() //Функция обновления списка пользователей из xml-файла
        {
            try //Пробуем выполнить следующий код
            {
                users.Clear(); //Очищаем список пользователей, для избежания повторений в списке
                var xDoc = new XmlDocument(); //Создаём новый эезмепляр xml-документа
                xDoc.Load("userBase.xml"); //Загружаем в него содержимое файла userBase.xml, находящегося в папке с exe-файлом  
                var Root = xDoc.DocumentElement; //Определяем корень файла
                if (Root == null) return; //Если корня нет то выходим из функции, ловить тут нечего
                foreach (XmlNode bases in Root) //Иначе проходим по элментам корня обозначающие разны базы
                    if (bases.Name == "users") //Если база имеет имя "пользователи"
                        foreach (XmlNode userElem in bases
                        ) //то проходимся пол этой базе, то есть по всем пользователям в этой базе
                        {
                            var Username =
                                userElem.Attributes.GetNamedItem("name")
                                    .Value; //Получаем для каждого пользователя его логин
                            ;
                            var passwordUser =
                                "0LvQsNC70LDQu9Cw0YHQtdGA0LbRgdCy0LDQu9C40LvRgdGP0YHQvtGB0YLQvtC70LA="; //Устанавливаем временное значение пароля
                            foreach (XmlNode attrUser in userElem.ChildNodes) //Проходимся по атрибутам пользователя
                                if (attrUser.Name == "password") //Нас интересует пароль(далее тут будет можифицировано
                                    //что будет хранится счёт и какая-нибудь статиситка)
                                    passwordUser =
                                        attrUser
                                            .InnerText; //Если нашли атрибут пользователя отвеч. за пароль запоминаем его значение
                            users.Add(new User(Username,
                                passwordUser)); //К списку пользователей добавляем нового пользователя
                        }
            }
            catch //Если в этом коде где угодно вылазит ошибка, допустим как я писал выше что нет доступа тк занято другим процессом, то
            {
            }
        }

        private bool TryRegisterNewUser(string username, string password) //Функция регистрации нового пользователя
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load("userBase.xml");
                var Root = xDoc.DocumentElement;
                if (Root != null)
                    foreach (XmlNode bases in Root)
                        if (bases.Name == "users")
                        {
                            //по аналогии с прошлой функции доходим до базы с именем "пользователи"
                            var userElem = xDoc.CreateElement("user"); //Создаём элемент "Пользователь"
                            var nameAttr = xDoc.CreateAttribute("name"); //Атрибут элмента "имя"(логин)
                            var passwordElem = xDoc.CreateElement("password"); //Создаём элемент пароль

                            //Создаём значения для элментов и атрибутов
                            var nameText = xDoc.CreateTextNode(username);
                            var passwordText = xDoc.CreateTextNode(password);

                            //Собираем структуру пользователя
                            nameAttr.AppendChild(nameText);
                            passwordElem.AppendChild(passwordText);
                            userElem.Attributes.Append(nameAttr);
                            userElem.AppendChild(passwordElem);

                            bases.AppendChild(userElem);
                            xDoc.Save("userBase.xml"); //Сохраняем изменения в файлк
                            users.Add(new User(username, password)); //К списку пользователя добовляем нового
                            return true; //Возвращаем что попытка регистрации закончилась успешно
                        }
            }
            catch
            {
                //По аналогии с прошлой функцией
                return false; //Возвращаем ложь, что заставляет повториться функцию
            }

            return true;
        }
    }
}