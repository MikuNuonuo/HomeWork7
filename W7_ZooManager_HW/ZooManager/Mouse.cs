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
            Flee(animalZones);
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

        /* We can't make the same assumptions with this method that we do with Attack, since
         * the animal here runs AWAY from where they spotted their target (using the Seek method
         * to find a predator in this case). So, we need to figure out if the direction that the
         * retreating animal wants to move is valid. Is movement in that direction still on the board?
         * Is it just going to send them into another animal? With our cat & mouse setup, one is the
         * predator and the other is prey, but what happens when we have an animal who is both? The animal
         * would want to run away from their predators but towards their prey, right? Perhaps we can generalize
         * this code (and the Attack and Seek code) to help our animals strategize more...
         */
        public bool Retreat(Animal runner, Direction d, List<List<Zone>> animalZones)
        {
            Console.WriteLine($"{runner.name} is retreating {d.ToString()}");
            int x = runner.location.x;
            int y = runner.location.y;
            //prevent null reference exception
            if (animalZones == null || animalZones.Count == 0)
            {
                return false;
            }
            int numCellsY = animalZones.Count;
            int numCellsX = animalZones[0].Count;
            switch (d)
            {
                case Direction.up:
                    /* The logic below uses the "short circuit" property of Boolean &&.
                     * If we were to check our list using an out-of-range index, we would
                     * get an error, but since we first check if the direction that we're modifying is
                     * within the ranges of our lists, if that check is false, then the second half of
                     * the && is not evaluated, thus saving us from any exceptions being thrown.
                     */
                    if (y > 0 && animalZones[y - 1][x].occupant == null)
                    {
                        animalZones[y - 1][x].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true; // retreat was successful
                    }
                    return false; // retreat was not successful
                /* Note that in these four cases, in our conditional logic we check
                 * for the animal having one square between itself and the edge that it is
                 * trying to run to. For example,in the above case, we check that y is greater
                 * than 0, even though 0 is a valid spot on the list. This is because when moving
                 * up, the animal would need to go from row 1 to row 0. Attempting to go from row 0
                 * to row -1 would cause a runtime error. This is a slightly different way of testing
                 * if 
                 */
                case Direction.down:
                    if (y < numCellsY - 1 && animalZones[y + 1][x].occupant == null)
                    {
                        animalZones[y + 1][x].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.left:
                    if (x > 0 && animalZones[y][x - 1].occupant == null)
                    {
                        animalZones[y][x - 1].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.right:
                    if (x < numCellsX - 1 && animalZones[y][x + 1].occupant == null)
                    {
                        animalZones[y][x + 1].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
            }
            return false; // fallback
        }

        /* Note that our mouse is (so far) a teeny bit more strategic than our cat.
         * The mouse looks for cats and tries to run in the opposite direction to
         * an empty spot, but if it finds that it can't go that way, it looks around
         * some more. However, the mouse currently still has a major weakness! He
         * will ONLY run in the OPPOSITE direction from a cat! The mouse won't (yet)
         * consider running to the side to escape! However, we have laid out a better
         * foundation here for intelligence, since we actually check whether our escape
         * was succcesful -- unlike our cats, who just assume they'll get their prey!
         */
        public void Flee(List<List<Zone>> animalZones)
        {
            if (Seek(location.x, location.y, animalZones, Direction.up, "cat"))
            {
                if (Retreat(this, Direction.down, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.down, "cat"))
            {
                if (Retreat(this, Direction.up, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.left, "cat"))
            {
                if (Retreat(this, Direction.right, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.right, "cat"))
            {
                if (Retreat(this, Direction.left, animalZones)) return;
            }
        }
    }
}

