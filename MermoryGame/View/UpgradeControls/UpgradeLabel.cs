using System.Windows.Forms;

namespace MermoryGame.Control
{
    public class UpgradeLabel : Label, Moving_Resizing //Определяем класс улучшенной надписи. Наследуемся от
        //стандартной надписи и придерживаемся интерфейсу перемещения и изменения размеров
    {

        public UpgradeLabel() : base() //Конструктор -дубируем конструктор класса родителя
        {
            
        }
        
        public void Move(int new_X, int new_Y)
        {
            this.Left = new_X;
            this.Top = new_Y;
        }

        public void Resize(int new_Width, int new_Height)
        {
            this.Width = new_Width;
            this.Height = new_Height;
        }

        public void SetGeometry(int new_X, int new_Y, int new_Width, int new_Height)
        {
            this.Left = new_X;
            this.Top = new_Y;
            this.Width = new_Width;
            this.Height = new_Height;
        }
    }
}