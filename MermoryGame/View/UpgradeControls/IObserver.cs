namespace MermoryGame.View.UpgradeControls
{
    public interface IObserver //Интерфейс наблюдателя
    {
        void Update(object[] ob); //Фунция принятия обновления наблюдаемого объекта с какими угодно параметрами
    }
}