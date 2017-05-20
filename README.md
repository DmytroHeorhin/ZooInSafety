# ZooInSafety

There was a problem in a cat finalization logic. 
Because of some reason, a cat should be finalized only if number of corpses is greater than 100.

        ~Cat()
        {
            Logger.LogYellow("Finalizing cat!");

            // Release the object only in case number of corpses > 100
            while (Zoo.NumCorpses > 200) { }
        }
        
This loop was removed and the problem disappeared. Still the feature of preserving cat corpses needs to be implemented.
So the Morgue class was introduced. Whenever an animal is about to garbage collected, the number of corpses is checked. If it is not enough corpses in Zoo, then the animal is moved to morgue and will not be garbage collected next time.

        ~Animal()
        {
            if (Zoo.NumCorpses < MinNumberOfCorpses)
            {
                Morgue.Receive(this);
                Logger.LogYellow("Animal #{0} has been moved to morgue", _id);
            }
            else
            {
                Logger.Log("Ruining animal: {0}, ID = {1}", GetType().Name, _id);
                Interlocked.Decrement(ref Zoo.NumCorpses);
                Logger.LogYellow("Ruining animal: {0} finished, ID= {1}", GetType().Name, _id);
            }
        }
        
 And time to time corpses are being removed from the morgue if there are enough corpses in the zoo.
 
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
        
 Now animals will live happily ever after.
