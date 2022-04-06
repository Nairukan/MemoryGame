using System;
using System.Drawing;

namespace MermoryGame
{
    public class LoginForm : AbsForm //Класс окно входа или регистрации, наследуемся от базового класа
    {
        public LoginForm() //После выполнения конструктора род. класса выполняем следующее
        {
            Text = "Игра домино. Окно авторизации";
            var RegisterButton = AddButton("RegisterButton"); //Создаём элмент управления
            RegisterButton.Text = "Зарегестрироваться"; //Меняем содержимое
            RegisterButton.SetGeometry(80, 300, 280, 60); //Меняем позицию и размер
            RegisterButton.Click += RegisterButtonClick;
            //По аналогии дальше
            var LoginButton = AddButton("LoginButton");
            LoginButton.Text = "Войти";
            LoginButton.SetGeometry(365, 300, 120, 60);
            LoginButton.Click += LoginButtonClick;
            var EnterLogin = AddLineEdit("EnterLogin");
            EnterLogin.Font = new Font(EnterLogin.Font.Name, 70, EnterLogin.Font.Style);
            EnterLogin.SetGeometry(80, 150, 350, 70);
            var EnterPassword = AddLineEdit("EnterPassword");
            EnterPassword.Font = new Font(EnterPassword.Font.Name, 70, EnterPassword.Font.Style);
            EnterPassword.SetGeometry(80, 240, 350, 70);
            var LoginText = AddLabel("LoginText");
            LoginText.SetGeometry(80, 120, 320, 50);
            LoginText.Text = "Введите Логин";
            var PasswordText = AddLabel("PasswordText");
            PasswordText.SetGeometry(80, 210, 350, 50);
            PasswordText.Text = "Введите Пароль";
            var GameTitle = AddLabel("GameTitle");
            GameTitle.SetGeometry(450, 150, 350, 350);
            GameTitle.Text = "ДОМИНО";
            countFontLayers++;
            AddToFontLevel(1, GameTitle);
            SetMaxFont(); //Вызываем функцию приведения к красивому оформлению сожержимого интерфейса
        }

        private void LoginButtonClick(object sender, EventArgs args)
        {
            NotifyObservers("TryLogin");
        }

        private void RegisterButtonClick(object sender, EventArgs args)
        {
            NotifyObservers("TryRegister");
        }

        public void NotifyObservers(string message)
        {
            System.Windows.Forms.Control loginControl, passwordControl;
            if (tryGetControlWithName("EnterLogin", out loginControl) &&
                tryGetControlWithName("EnterPassword", out passwordControl))
                foreach (var o in observers)
                    o.Update(new[] {message, loginControl.Text, passwordControl.Text});
            SetMaxFont();
        }
    }
}