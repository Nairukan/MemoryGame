namespace MermoryGame.Control
{
    public class PlayGrid
    {
        private PlayGridElem[,] Grid;
        public uint Height = 1, Width = 1;

        public void InitGrid(bool ChildMode = false)
        {
            if (ChildMode == false) //Adult Mode
            {
                Height = 40;
                Width = 42;
                Grid = new PlayGridElem[Height, Width];
            }
        }

        //x1, y1 - Левый нижний угол; x2, y2 - Правый верхний угол
        private void HARD_PushCard(PlayCard card, uint x1, uint y1, uint x2, uint y2)
        {
            for (var i = y2; i <= y1; i++)
            for (var j = x1; j <= x2; j++)
            {
                Grid[j, i].PlateStatus = PlayGridElem.Сonditions.Busy;
                Grid[j, i].WhoIsBusy = card;
            }

            if (card.IsDoubleValue)
            {
                Grid[x1, y1 - 1].PrevValue = card.Values[0];
                AddForGamePlaces(x1, y1 - 1);
            }
        }

        private bool AddForGamePlaces(uint LeftDown_X, uint LeftDown_Y) //Передаются пординаты последней полуплиточки
        {
            var Answer = false;
            //Down
            var Rule = true;
            for (var i = LeftDown_Y + 1; i <= LeftDown_Y + 5 && Rule; i++)
            for (var j = LeftDown_X; j <= LeftDown_X + 1 && Rule; j++)
                Rule = Rule && Grid[i, j].PlateStatus == PlayGridElem.Сonditions.Free;
            if (Rule)
                for (var i = LeftDown_Y + 1; i <= LeftDown_Y + 4 && Rule; i++)
                for (var j = LeftDown_X; j <= LeftDown_X + 1 && Rule; j++)
                {
                    Grid[i, j].PlateStatus = PlayGridElem.Сonditions.ForMove;
                    Grid[i, j].MovingDirection = PlayGridElem.Movement.Down;
                    Grid[i, j].PrevValue = Grid[LeftDown_X, LeftDown_Y].PrevValue;
                }

            Answer = Answer || Rule;
            //Up
            Rule = true;
            for (var i = LeftDown_Y - 6; i <= LeftDown_Y - 2 && Rule; i++)
            for (var j = LeftDown_X; j <= LeftDown_X + 1 && Rule; j++)
                Rule = Rule && Grid[i, j].PlateStatus == PlayGridElem.Сonditions.Free;
            if (Rule)
                for (var i = LeftDown_Y - 5; i <= LeftDown_Y - 2 && Rule; i++)
                for (var j = LeftDown_X; j <= LeftDown_X + 1 && Rule; j++)
                {
                    Grid[i, j].PlateStatus = PlayGridElem.Сonditions.ForMove;
                    Grid[i, j].MovingDirection = PlayGridElem.Movement.Up;
                    Grid[i, j].PrevValue = Grid[LeftDown_X, LeftDown_Y].PrevValue;
                }

            Answer = Answer || Rule;
            //Left
            Rule = true;
            for (var i = LeftDown_Y - 1; i <= LeftDown_Y && Rule; i++)
            for (var j = LeftDown_X - 5; j <= LeftDown_X - 1 && Rule; j++)
                Rule = Rule && Grid[i, j].PlateStatus == PlayGridElem.Сonditions.Free;
            if (Rule)
                for (var i = LeftDown_Y - 1; i <= LeftDown_Y && Rule; i++)
                for (var j = LeftDown_X - 4; j <= LeftDown_X - 1 && Rule; j++)
                {
                    Grid[i, j].PlateStatus = PlayGridElem.Сonditions.ForMove;
                    Grid[i, j].MovingDirection = PlayGridElem.Movement.Left;
                    Grid[i, j].PrevValue = Grid[LeftDown_X, LeftDown_Y].PrevValue;
                }

            Answer = Answer || Rule;
            //Right
            Rule = true;
            for (var i = LeftDown_Y - 1; i <= LeftDown_Y && Rule; i++)
            for (var j = LeftDown_X + 6; j <= LeftDown_X + 2 && Rule; j++)
                Rule = Rule && Grid[i, j].PlateStatus == PlayGridElem.Сonditions.Free;
            if (Rule)
                for (var i = LeftDown_Y - 1; i <= LeftDown_Y && Rule; i++)
                for (var j = LeftDown_X + 5; j <= LeftDown_X + 2 && Rule; j++)
                {
                    Grid[i, j].PlateStatus = PlayGridElem.Сonditions.ForMove;
                    Grid[i, j].MovingDirection = PlayGridElem.Movement.Right;
                    Grid[i, j].PrevValue = Grid[LeftDown_X, LeftDown_Y].PrevValue;
                }

            Answer = Answer || Rule;
            return Answer;
        }

        public void PushCard(PlayCard card, uint x, uint y)
        {
        }
    }
}