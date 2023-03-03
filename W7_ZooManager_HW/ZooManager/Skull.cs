using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager
{
    public class Skull : Animal
    {
        // (feature p)new subclass for Animal. It has less properties because many properties loss their meaning.
        public Skull(string name)
        {
            emoji = "☠ ";
            species = "skull";
            this.name = name;
            reactionTime = 0;
        }

        public override void Activate(List<List<Zone>> animalZones)
        {
            base.Activate(animalZones);
            Console.WriteLine("Do nothing");
        }
    }
}
