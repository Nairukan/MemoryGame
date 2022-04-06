namespace MermoryGame.View.UpgradeControls
{
    public interface IObservable //Интерфейс наблюдаемого объекта то есть тут хранится множество
        //функций которые должны быть реализованы в классе чтобы за ним было возможно наблюдать
    {
        void RegisterObserver(IObserver o); //Привязка некоторого наблюдателя к текущему классу
        void RemoveObserver(IObserver o); //отвязка наблюдателся
        void NotifyObservers(string message); //Уведомление всех наблюдателей о изменених с сообщениями 
    }
}