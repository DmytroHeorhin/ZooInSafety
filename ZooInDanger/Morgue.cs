using System;
using System.Collections.Generic;
using Zoo.Animals;

namespace Zoo
{
    internal static class Morgue 
    {
        private static readonly IList<IAnimal> Animals = new List<IAnimal>();

        public static int Count => Animals.Count;
        public static int TotalAnimalsRecived { get; private set; }

        public static void RemoveAnimalsIfNeeded()
        {
            if (Count == 0) return;
            var snapshot = new List<IAnimal>(Animals);
            foreach (var animal in snapshot)
            {
                if (Zoo.NumCorpses > animal.MinNumberOfCorpses)
                {
                    Animals.Remove(animal);
                }
            }
        }

        public static void Receive(IAnimal animal)
        {
            Animals.Add(animal);
            GC.ReRegisterForFinalize(animal);
            TotalAnimalsRecived++;
        }
    }
}
