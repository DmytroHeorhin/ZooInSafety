using System;

namespace Zoo.Animals
{
    public class Cat : Animal
    {
        public Cat(IAnimalStatusTracker statusTracker) : base(statusTracker)
        {
        }

        // Release the object only in case number of corpses > 100
        public override int MinNumberOfCorpses => 100;

        public override int LifeInterval => 13;

        public override int InfectionDeathInterval => 300;

        ~Cat()
        {
            Logger.LogYellow("Finalizing cat!");
        }
    }
}