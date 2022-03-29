namespace MermoryGame
{
    public class LoginForm : AbsForm //Класс окно входа или регистрации, наследуемся от базового класа
    {
        public LoginForm() //После выполнения конструктора род. класса выполняем следующее
        {
            var RegisterButton = AddButton("RegisterButton"); //Создаём элмент управления
            RegisterButton.Text = "Зарегестрироваться"; //Меняем содержимое
            RegisterButton.SetGeometry(80, 300, 180, 50); //Меняем позицию и размер
            //По аналогии дальше
            var LoginButton = AddButton("LoginButton");
            LoginButton.Text = "Войти";
            LoginButton.SetGeometry(270, 300, 120, 50);
            var EnterLogin = AddLineEdit("EnterLogin");
            EnterLogin.SetGeometry(80, 150, 350, 70);
            var EnterPassword = AddLineEdit("EnterPassword");
            EnterPassword.SetGeometry(80, 240, 350, 70);
            var LoginText = AddLabel("LoginText");
            //RegisterButton.SetGeometry(80,400, 250, 50);
            var PasswordText = AddLabel("PasswordText");
            //RegisterButton.SetGeometry(80,400, 250, 50);
            var GameTitle = AddLabel("GameTitle");
            //RegisterButton.SetGeometry(80,400, 250, 50);

            SetMaxFont(); //Вызываем функцию приведения к красивому оформлению сожержимого интерфейса
        }
    }
}