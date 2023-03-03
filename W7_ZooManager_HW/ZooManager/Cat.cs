using System;
using System.Collections.Generic;

namespace ZooManager
{
    public class Cat : Animal
    {
        
        public Cat(string name)
        {
            emoji = "🐱";
            species = "cat";
            this.name = name;
            reactionTime = new Random().Next(1, 6); // reaction time 1 (fast) to 5 (medium)
        }

        public override void Activate(List<List<Zone>> animalZones)
        {
            base.Activate(animalZones);
            Console.WriteLine("I am a cat. Meow.");
            Hunt(animalZones);
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
        public void Hunt(List<List<Zone>> animalZones)
        {
            if (Seek(location.x, location.y, animalZones, Direction.up, "mouse"))
            {
                Attack(this, Direction.up, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.down, "mouse"))
            {
                Attack(this, Direction.down, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.left, "mouse"))
            {
                Attack(this, Direction.left, animalZones);
            }
            else if (Seek(location.x, location.y, animalZones, Direction.right, "mouse"))
            {
                Attack(this, Direction.right, animalZones);
            }
        }
    }
}

