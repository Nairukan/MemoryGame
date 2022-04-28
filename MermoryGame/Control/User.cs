using System;

namespace CourseWork
{
    public class User : IComparable<User> //Класс пользователя
    {
        private readonly string
            Password = "<None>"; //Приватное поле хранящее текст, являющийся Паролем пользователя, по умолчанию <None>

        private readonly uint
            Score; //Приватное поле хранящее положительное число, являющиеся счётом пользователя(Глобальный рейтинг), по умолчанию 0

        private readonly string
            Username = "<None>"; //Приватное поле хранящее текст, являющийся Логином пользователя, по умолчанию <None>

        public User(string username, string password, uint score) //Конструектор принемающий логин и пароль
        {
            //Устанавливаем соответсвующие значения
            Username = username;
            Password = password;
            Score = score;
        }

        public int
            CompareTo(User other) //Функция сравнения двух пользователей (Используем для сортировки Листов Пользователей, по их счёту)
        {
            if (GetScore() > other.GetScore())
                return -1; //Минус один означает что текущий элемент находится в нужной позиции относительно other
            if (GetScore() == other.GetScore()) return 0; //Ноль означает что текущий элемент равен other
            return 1; //Один означает что текущий элемент нужно переместить
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

        public uint GetScore()
        {
            return Score; //Геттер Счёта
        }

        public string GetUsername()
        {
            return Username; //Геттер Логина
        }
    }
}