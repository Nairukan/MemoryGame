using System;
using System.Collections.Generic;
using MermoryGame.View.UpgradeControls;

namespace MermoryGame.Control
{
    public class DominoModule : IObservable
    {
        public List<PlayCard> Bazar;
        public List<List<PlayCard>> CardsOfPlayers = new List<List<PlayCard>>();

        public uint countPlayers = 0;
        public uint[] lastValues = new uint[2];
        public List<IObserver> observers; //Список наблюдателей 
        public int TurnIndex;

        public void RegisterObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void NotifyObservers(string message)
        {
            foreach (var o in observers) //Проходит по всем элементам следящим за ним 
                if (o.ObsName == message.Split()[0])
                    o.Update(message.Split()); //Обновляя их с соответсвующим сообщением
        }

        public void InitGame(bool ChildMode = false)
        {
            CardsOfPlayers.Clear();
            Bazar.Clear();
            var random = new Random((int) DateTime.UtcNow.Ticks);
            var AllCards = new List<PlayCard>();
            for (uint i = 0; i <= 6; i++)
            for (var j = i; j <= 6; j++)
            {
                var fValue = i;
                var sValue = j;
                if (random.Next() % 2 == 0)
                {
                    var buffer = sValue;
                    sValue = fValue;
                    fValue = buffer;
                }

                AllCards.Add(new PlayCard(fValue, sValue));
            }

            for (var i = 0; i < countPlayers; i++)
            {
                CardsOfPlayers.Add(new List<PlayCard>());
                for (var j = 0; j < 5; j++)
                {
                    var ind = random.Next() % AllCards.Count;
                    CardsOfPlayers[i].Add(AllCards[ind]);
                    AllCards.RemoveAt(ind);
                }
            }

            while (AllCards.Count > 0)
            {
                var ind = random.Next() % AllCards.Count;
                Bazar.Add(AllCards[ind]);
                AllCards.RemoveAt(ind);
            }

            var firstCard = WhoTurnFirst();
            for (var i = 0; i < CardsOfPlayers.Count; i++)
            {
                CardsOfPlayers[i].Sort();
                if (CardsOfPlayers[i].Contains(firstCard))
                    TurnIndex = i;
            }

            var playGrid = new PlayGrid();
            playGrid.InitGrid(ChildMode);
            playGrid.PushCard(firstCard, playGrid.Width / 2, playGrid.Height / 2);
            NotifyObservers("PlayField CREATE_WIND");
        }


        public PlayCard WhoTurnFirst()
        {
            //Ищем максимальный дубль
            var ans = new PlayCard(10, 10); //Невозможное значение
            uint max = 0; //Устанавливаем максимальную сумму на фишке 0, тк с нулей начинать нельзя
            var answer = -1; //Устанавливаем начальный ответ (Кто должен первый ходить)
            for (var i = 0; i < CardsOfPlayers.Count; i++) //Проходим по всем участникам игры
                foreach (var someCard in CardsOfPlayers[i]) //Проходясь по каждой фишки участника
                    if (someCard.Values[0] == someCard.Values[1] //Если значения на плитке одинаковые
                        && max < someCard.Values[0] + someCard.Values[1]) //и их сумма больше максимальной суммы, то
                    {
                        ans = someCard;
                        max = someCard.Values[0] + someCard.Values[1]; //Устанавливаем новое значение максимума
                    }

            if (max != 0)
            {
                lastValues[0] = max / 2;
                lastValues[1] = max / 2;
                return ans; //Если был найден хоть один дубль кроме 0|0, то возвращаем ответ
            }

            for (var i = 0; i < CardsOfPlayers.Count; i++) //Проходим по всем участникам игры
                foreach (var someCard in CardsOfPlayers[i]) //Проходясь по каждой фишки участника
                    if (max < someCard.Values[0] + someCard.Values[1]) //и их сумма больше максимальной суммы, то
                    {
                        lastValues[0] = someCard.Values[0];
                        lastValues[1] = someCard.Values[1];
                        ans = someCard;
                        max = someCard.Values[0] + someCard.Values[1]; //Устанавливаем новое значение максимума
                    }

            return ans;
        }
    }
}