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
        public void Flee(List<List<Zone>> animalZones, String animalName)
        {
            if (Seek(location.x, location.y, animalZones, Direction.up, animalName))
            {
                if (Retreat(this, Direction.down, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.down, animalName))
            {
                if (Retreat(this, Direction.up, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.left, animalName))
            {
                if (Retreat(this, Direction.right, animalZones)) return;
            }
            if (Seek(location.x, location.y, animalZones, Direction.right, animalName))
            {
                if (Retreat(this, Direction.left, animalZones)) return;
            }
        }

        /* This method currently assumes that the attacker has determined there is prey
         * in the target direction. In addition to bug-proofing our program, can you think
         * of creative ways that NOT just assuming the attack is on the correct target (or
         * successful for that matter) could be used?
         */
        public void Attack(Animal attacker, Direction d, List<List<Zone>> animalZones)
        {
            Console.WriteLine($"{attacker.name} is attacking {d.ToString()}");
            int x = attacker.location.x;
            int y = attacker.location.y;
            //(feature p)
            attacker.turn = 0;
            switch (d)
            {
                case Direction.up:
                    animalZones[y - 1][x].occupant = attacker;
                    break;
                case Direction.down:
                    animalZones[y + 1][x].occupant = attacker;
                    break;
                case Direction.left:
                    animalZones[y][x - 1].occupant = attacker;
                    break;
                case Direction.right:
                    animalZones[y][x + 1].occupant = attacker;
                    break;
            }
            animalZones[y][x].occupant = null;
        }

        /* Note that our cat is currently not very clever about its hunting.
         * It will always try to attack "up" and will only seek "down" if there
         * is no mouse above it. This does not affect the cat's effectiveness
         * very much, since the overall logic here is "look around for a mouse and
         * attack the first one you see." This logic might be less sound once the
         * cat also has a predator to avoid, since the cat may not want to run in
         * to a square that sets it up to be attacked!
         */
        public void Hunt(List<List<Zone>> animalZones, string huntedAnimal)
        {
            if (Seek(location.x, location.y, animalZones, Direction.up, huntedAnimal))
            {
                Attack(this, Direction.up, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.down, huntedAnimal))
            {
                Attack(this, Direction.down, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.left, huntedAnimal))
            {
                Attack(this, Direction.left, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.right, huntedAnimal))
            {
                Attack(this, Direction.right, animalZones);
            }
        }
    }
}
