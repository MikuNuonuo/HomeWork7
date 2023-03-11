using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager
{
    public class Raptor:Bird
    {  //(feature a) new subclass Raptor.
        public Raptor(string name)
        {
            emoji = "🦅";
            species = "raptor";
            this.name = name; // "this" to clarify instance vs. method parameter
            reactionTime = 1; // reaction time of 1 
        }

        public override void Activate(List<List<Zone>> animalZones)
        {
            base.Activate(animalZones);
            Console.WriteLine("I am a raptor.woo~");
            Hunt(animalZones,"cat");//(feature a) raptor hunt cat
        }

    }
}
