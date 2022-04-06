namespace CourseWork
{
    public class User //Класс пользователя
    {
        public string Username = "<None>", Password = "<None>";

        public User(string username, string password) //Конструектор принемающий логин и пароль
        {
            //Устанавливаем соответсвующие значения
            Username = username;
            Password = password;
        }

        public bool
            IsThisUsername(string meybeUsername) //Функция сравнивающая логин пользователя с некоторой полслед. символов
        {
            return Username == meybeUsername;
        }

        public bool IsThisPassword(string meybePassword) //то же самое с паролем
        {
            return Password == meybePassword;
        }
    }
}