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
            Hunt(animalZones,"mouse");
        }

        
    }
}

