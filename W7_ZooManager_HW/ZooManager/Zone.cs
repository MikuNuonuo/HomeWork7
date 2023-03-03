using System;
namespace ZooManager
{
    public class Zone
    {
        private Animal _occupant = null;
        public Animal occupant
        {
            get { return _occupant; }
            set {
                _occupant = value;
                if (_occupant != null) {
                    _occupant.location = location;
                }
            }
        }

        public Point location;

        public string emoji
        {
            get
            {
                if (occupant == null) return "";
                return occupant.emoji;
            }
        }

        public string rtLabel
        {
            get
            {
                if (occupant == null) return "";
                return occupant.reactionTime.ToString();
            }
        }

        public string connection
        {
            get
            {
                if (occupant == null) return "";
                return "-";
            }
        }

        public string turn
        {
            get
            {
                if (occupant == null) return "";
                return occupant.turn.ToString();
            }
        }

        public Zone(int x, int y, Animal animal)
        {
            location.x = x;
            location.y = y;

            occupant = animal;
        }
    }
}
