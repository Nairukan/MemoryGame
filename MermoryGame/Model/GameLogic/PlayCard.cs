using System;

namespace MermoryGame.Control
{
    //компонент уровня M-Model в паттерне MVC (для отражения логики моделей)
    //Планируется добавить в данный уровень ещё 2-3 класса допустим модель записи о рекорде, модель игры который будет
    //расчитывать баллы и проверять выполнения правил игры, ну и тд

    public class PlayCard : IComparable<PlayCard>
    {
        public bool HorizontalDirection = true;

        public uint[] Values = new uint[2];

        public PlayCard(uint fValue, uint sValue)
        {
            Values[0] = fValue;
            Values[1] = sValue;
        }

        public bool IsDoubleValue => Values[0] == Values[1];

        public int CompareTo(PlayCard other)
        {
            var sum1 = Values[0] + Values[1];
            var sum2 = other.Values[0] + other.Values[1];
            if (sum1 > sum2) return -1;
            if (sum1 == sum2) return 0;
            return 1;
        }

        public bool HasThisValue(uint maybeValue)
        {
            if (Values[0] == maybeValue) return true;
            if (Values[1] == maybeValue)
            {
                var temp = Values[0];
                Values[0] = Values[1];
                Values[1] = temp;
                return true;
            }

            return false;
        }
    }
}