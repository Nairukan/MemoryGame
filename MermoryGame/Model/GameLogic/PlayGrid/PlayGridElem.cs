namespace MermoryGame.Control
{
    public class PlayGridElem
    {
        public enum Movement
        {
            Left,
            Up,
            Right,
            Down
        }

        public enum Сonditions
        {
            Free,
            Boarder,
            Busy,
            ForMove
        }

        public Movement MovingDirection;

        public Сonditions PlateStatus;
        public uint PrevValue;
        public PlayCard WhoIsBusy;
    }
}