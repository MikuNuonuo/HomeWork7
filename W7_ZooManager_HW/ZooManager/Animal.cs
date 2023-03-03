using System;
using System.Collections.Generic;

namespace ZooManager
{
    public class Animal
    {
        public string emoji;
        public string species;
        public string name;
        public int reactionTime = 5; // default reaction time for animals (1 - 10)
        public bool isCurrentMoved = false;//(feature o) control not move two or more
        public int turn = 0;// (feature m)(feature p)(feature q)on board turn

        public Point location;

        public void ReportLocation()
        {
            Console.WriteLine($"I am at {location.x},{location.y}");
        }

        virtual public void Activate(List<List<Zone>> animalZones)
        {
            Console.WriteLine($"Animal {name} at {location.x},{location.y} activated");
        }

        virtual public void Reproduce(int x, int y,List<List<Zone>> animalZones)
        {
            Console.WriteLine($"Animal {name} at try to reproduce");
        }

        protected bool Seek(int x, int y, List<List<Zone>> animalZones, Direction d, string target)
        {
            // prevent null reference exception
            if (animalZones == null || animalZones.Count == 0) {
                return false;
            }
            int numCellsY = animalZones.Count;
            int numCellsX = animalZones[0].Count;
            switch (d)
            {
                case Direction.up:
                    y--;
                    break;
                case Direction.down:
                    y++;
                    break;
                case Direction.left:
                    x--;
                    break;
                case Direction.right:
                    x++;
                    break;
            }
            if (y < 0 || x < 0 || y > numCellsY - 1 || x > numCellsX - 1) return false;
            if (animalZones[y][x].occupant == null) return false;
            if (animalZones[y][x].occupant.species == target)
            {
                return true;
            }
            return false;
        }
    }
}
