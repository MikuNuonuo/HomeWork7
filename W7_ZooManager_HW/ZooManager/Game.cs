using System;
using System.Collections.Generic;

namespace ZooManager
{
    public class Game
    {
        
        static private readonly int DIE_AFTER_TURNS = 3;

        static public void SetUpGame()
        {
            Zoo.CreateZoo();
        }

        static public void AddZones(Direction d)
        {
            Zoo.AddZones(d);
        }

        static public void ZoneClick(Zone clickedZone)
        {
            Console.Write("Got animal ");
            Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            Console.Write("Held animal is ");
            Console.WriteLine(Zoo.holdingPen.emoji == "" ? "none" : Zoo.holdingPen.emoji);
            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation();
            if (Zoo.holdingPen.occupant == null && clickedZone.occupant != null)
            {
                // take animal from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                Zoo.holdingPen.occupant = clickedZone.occupant;
                Zoo.holdingPen.occupant.location.x = -1;
                Zoo.holdingPen.occupant.location.y = -1;
                clickedZone.occupant = null;
                ActivateAnimals();
            }
            else if (Zoo.holdingPen.occupant != null && clickedZone.occupant == null)
            {
                // put animal in zone from holding pen
                Console.WriteLine("Placing " + Zoo.holdingPen.emoji);
                clickedZone.occupant = Zoo.holdingPen.occupant;
                clickedZone.occupant.location = clickedZone.location;
                Zoo.holdingPen.occupant = null;
                Console.WriteLine("Empty spot now holds: " + clickedZone.emoji);
                ActivateAnimals();
            }
            else if (Zoo.holdingPen.occupant != null && clickedZone.occupant != null)
            {
                Console.WriteLine("Could not place animal.");
                // Don't activate animals since user didn't get to do anything
            }
        }

        static public void AddAnimalToHolding(string animalType)
        {
            if (Zoo.holdingPen.occupant != null) return;
            if (animalType == "cat") Zoo.holdingPen.occupant = new Cat("Fluffy");
            if (animalType == "mouse") Zoo.holdingPen.occupant = new Mouse("Squeaky");
            if (animalType == "raptor") Zoo.holdingPen.occupant = new Raptor("Woo");
            Console.WriteLine($"Holding pen occupant at {Zoo.holdingPen.occupant.location.x},{Zoo.holdingPen.occupant.location.y}");
            ActivateAnimals();
        }

        static public void ActivateAnimals()
        {
            for (var r = 1; r < 11; r++) // reaction times from 1 to 10
            {
                for (var y = 0; y < Zoo.numCellsY; y++)
                {
                    for (var x = 0; x < Zoo.numCellsX; x++)
                    {
                        var zone = Zoo.animalZones[y][x];
                        if (zone.occupant != null && zone.occupant.reactionTime == r && zone.occupant.isCurrentMoved == false)//add isCurrentMoved
                        {
                            zone.occupant.isCurrentMoved = true; //(feature o)indicate this animal is moved
                            zone.occupant.turn++;//(feature m)(feature p)(feature q)turn will add one when Activate
                            zone.occupant.Activate(Zoo.animalZones);
                        }
                    }
                }
            }

            /* iterate animalZones to do the following features after Activate
            * (feature o)reset all animals isCurrentMoved to false
            * (feature p)and cat will die when turnLastAte>3
            * (feature q)and mouse will reproduce after 3 turn
            */
            for (var y = 0; y < Zoo.numCellsY; y++)
            {
                for (var x = 0; x < Zoo.numCellsX; x++)
                {
                    var zone = Zoo.animalZones[y][x];
                    if (zone.occupant != null)
                    {
                        // (feature o)reset to not moved in new turn
                        zone.occupant.isCurrentMoved = false;
                        // (feature p) cat will die after 3 turns
                        if (zone.occupant.species == "cat" && zone.occupant.turn > DIE_AFTER_TURNS)
                        {
                            zone.occupant = new Skull(zone.occupant.name + "'s skull"); ;
                        }
                        else if (zone.occupant.species == "raptor" && zone.occupant.turn > DIE_AFTER_TURNS)
                        {
                                zone.occupant = new Skull(zone.occupant.name + "'s skull"); ;
                        }
                        // (feature q)mouse will try to Reproduce after 3 turns
                        if (zone.occupant.species == "mouse" && zone.occupant.turn > 3)
                        {
                            zone.occupant.Reproduce(x,y,Zoo.animalZones);
                        }
                    }
                }
            }
        }
    }
}

