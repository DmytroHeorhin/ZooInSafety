

be

# ZooInSafety

There was a problem in a cat finalization logic. 
Because of some reason, a cat should be finalized only if number of corpses is greater than 100.

        ~Cat()
         {
            Logger.LogYellow("Finalizing cat!");

            // Release the object only in case number of corpses > 100
            while (Zoo.NumCorpses > 200) { }
        }
        
This loop was removed and the problem disappeared. 

Still the feature of preserving cat corpses needs to be implemented, so the Morgue class was introduced. Whenever an animal is about to be garbage collected, the number of corpses is checked. If there is not enough corpses in the zoo, then the animal is moved to morgue and will not be garbage collected next time.

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
        
 And time to time corpses are being removed from the morgue if there is enough corpses in the zoo.
 
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
        
Also there was a problems with elephant. There was a possibility of OutOfMemoryException during the allocation of elephant's parts. A handling for such situation was added.

        public Elephant(IAnimalStatusTracker statusTracker) : base(statusTracker)
        {
            try
            {
                _rightTusk = new byte[0x14000];
                _leftTusk = new byte[0x1400];
                _leftFrontFoot = new byte[0x14000];
                _leftBackFoot = new byte[0x14000];
                _rightFrontFoot = new byte[0x14000];
                _rightBackFoot = new byte[0x14000];
            }
            catch (OutOfMemoryException)
            {
                Logger.LogYellow("Failed to create an elephant, not enough room for such giant");
                Kill();
            }
        }
        
In addition, the parts of elephant were remaining not garbage collected until the finalizer of an elephant was not executed and will survive at least one more garbage collection. So now, the parts of an elephant are made unreachable when it dies.

Animal:

        private void Die()
        {
            _isAlive = false;
            _statusTracker.Died(this);
            EarthLiveTicker.LiveTicker.Unsubscribe(this);
            OnDeath();
        }

        protected virtual void OnDeath() { }
        
Elephant:

        protected override void OnDeath()
        {
            FallApart();
            Logger.LogYellow("Elephant has fallen apart...");
        }

        private void FallApart()
        {
            _leftTusk = _rightTusk = _leftFrontFoot = _rightFrontFoot = _leftBackFoot = _rightBackFoot = null;
        }
        
 Now animals will live happily ever after.
