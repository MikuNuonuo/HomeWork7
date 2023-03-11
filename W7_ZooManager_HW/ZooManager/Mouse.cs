using System;
using System.Collections.Generic;

namespace ZooManager
{
    public class Mouse : Animal
    {
        private bool isReproduce = false;
        public Mouse(string name)
        {
            emoji = "🐭";
            species = "mouse";
            this.name = name; // "this" to clarify instance vs. method parameter
            reactionTime = new Random().Next(1, 4); // reaction time of 1 (fast) to 3
            /* Note that Mouse reactionTime range is smaller than Cat reactionTime,
             * so mice are more likely to react to their surroundings faster than cats!
             */
        }

        public override void Activate(List<List<Zone>> animalZones)
        {
            base.Activate(animalZones);
            Console.WriteLine("I am a mouse. Squeak.");
            Flee(animalZones,"cat");
        }

        /* (feature q)Mouse will reproduce after 3 turns. We need 3 parameters. X and Y represent the current location of mouse
         * animalZones refer to the board. We can calculate numCellsY and numCellsX by animalZones.
         * I use the logic which same as Retreat to find the direction to reproduce new Mouse;
         */
        public override void Reproduce(int x, int y,List<List<Zone>> animalZones)
        {
            // return if already reproduce
            if (isReproduce) {
                return;
            }
            int numCellsY = animalZones.Count;
            int numCellsX = animalZones[0].Count;
            base.Reproduce(x,y,animalZones);
            if (y > 0 && animalZones[y - 1][x].occupant == null)
            {// reproduce up
                animalZones[y - 1][x].occupant = new Mouse("Squeaky");
                isReproduce = true;
            }
            else if (y < numCellsY - 1 && animalZones[y + 1][x].occupant == null)
            {// reproduce down
                animalZones[y + 1][x].occupant = new Mouse("Squeaky");
                isReproduce = true;
            }
            else if (x > 0 && animalZones[y][x - 1].occupant == null)
            {// reproduce left
                animalZones[y][x - 1].occupant = new Mouse("Squeaky");
                isReproduce = true;
            }
            else if (x < numCellsX - 1 && animalZones[y][x + 1].occupant == null) 
            {
                animalZones[y][x + 1].occupant = new Mouse("Squeaky");
                isReproduce = true;
            }
        }                
    }
}

