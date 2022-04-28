namespace MermoryGame
{
    //Компоненты папки используем для создания стандартных элментов интрефейса с улучшенными возможностями, возможно
    //дальнейшее добавление новых интерфйсов и классов
    public interface Moving_Resizing //Интерфейс более удобного перемещения и изменения размеров
    {
        void Move(int new_X, int new_Y);
        void Resize(int new_Width, int new_Height);
        void SetGeometry(int new_X, int new_Y, int new_Width, int new_Height);
    }
}