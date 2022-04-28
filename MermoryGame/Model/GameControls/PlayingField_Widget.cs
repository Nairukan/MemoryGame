using System.Windows.Forms;
using MermoryGame.View.UpgradeControls;

namespace MermoryGame.Model.GameWidget
{
    public class PlayingField_Widget : Panel, Moving_Resizing, IObserver
    {
        public PlayingField_Widget()
        {
            ObsName = "PlayField";
        }

        public string ObsName { get; set; }

        public void Update(object[] ob)
        {
            var args = (string[]) ob;
            try
            {
                if (args[1] == "CREATE_WIND")
                {
                }
            }
            catch
            {
            }
        }

        public void Move(int new_X, int new_Y)
        {
            Left = new_X;
            Top = new_Y;
            RepairElements();
        }

        public void Resize(int new_Width, int new_Height)
        {
            Width = new_Width;
            Height = new_Height;
            RepairElements();
        }

        public void SetGeometry(int new_X, int new_Y, int new_Width, int new_Height)
        {
            Left = new_X;
            Top = new_Y;
            Width = new_Width;
            Height = new_Height;
            RepairElements();
        }

        public void RepairElements() //перестройка внутренних элементов
        {
        }
    }
}