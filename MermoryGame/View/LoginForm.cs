using System.Windows.Forms;
using MermoryGame.Control;

namespace MermoryGame
{
    public class LoginForm : AbsForm //Класс окно входа или регистрации, наследуемся от базового класа
    {
        public LoginForm() : base()  //После выполнения конструктора род. класса выполняем следующее
        {
            UpgradeButton RegisterButton = AddButton("RegisterButton"); //Создаём элмент управления
            RegisterButton.Text = "Зарегестрироваться"; //Меняем содержимое
            RegisterButton.SetGeometry(80,300, 180, 50); //Меняем позицию и размер
            //По аналогии дальше
            UpgradeButton LoginButton = AddButton("LoginButton");
            LoginButton.Text = "Войти";
            LoginButton.SetGeometry(270,300, 120, 50);
            UpgradeLineEdit EnterLogin = AddLineEdit("EnterLogin");
            EnterLogin.SetGeometry(80,150, 350, 70);
            UpgradeLineEdit EnterPassword = AddLineEdit("EnterPassword");
            EnterPassword.SetGeometry(80,240, 350, 70);
            UpgradeLabel LoginText = AddLabel("LoginText");
            //RegisterButton.SetGeometry(80,400, 250, 50);
            UpgradeLabel PasswordText = AddLabel("PasswordText");
            //RegisterButton.SetGeometry(80,400, 250, 50);
            UpgradeLabel GameTitle = AddLabel("GameTitle");
            //RegisterButton.SetGeometry(80,400, 250, 50);
            
            SetMaxFont(); //Вызываем функцию приведения к красивому оформлению сожержимого интерфейса
        }
        
    }
}