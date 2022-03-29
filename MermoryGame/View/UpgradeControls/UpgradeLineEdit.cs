using System.Windows.Forms;

namespace MermoryGame.Control
{
    public class
        UpgradeLineEdit : TextBox, Moving_Resizing //Определяем класс улучшенного поля для ввода. Наследуемся от
        //стандартного поля для ввода и придерживаемся интерфейсу перемещения и изменения размеров
    {
        public void Move(int new_X, int new_Y)
        {
            Left = new_X;
            Top = new_Y;
        }

        public void Resize(int new_Width, int new_Height)
        {
            Width = new_Width;
            Height = new_Height;
        }

        public void SetGeometry(int new_X, int new_Y, int new_Width, int new_Height)
        {
            Left = new_X;
            Top = new_Y;
            Width = new_Width;
            Height = new_Height;
        }
    }
}