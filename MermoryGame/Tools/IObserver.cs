namespace MermoryGame.View.UpgradeControls
{
    public interface IObserver //Интерфейс наблюдателя
    {
        string ObsName { get; set; }
        void Update(object[] ob); //Фунция принятия обновления наблюдаемого объекта с какими угодно параметрами
    }
}