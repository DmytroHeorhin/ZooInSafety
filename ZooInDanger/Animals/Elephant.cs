using System;

namespace Zoo.Animals
{
    public class Elephant: Animal
    {
        // ReSharper disable NotAccessedField.Local
        private byte[] _leftTusk;
        private byte[] _rightTusk;
        private byte[] _leftFrontFoot;
        private byte[] _leftBackFoot;
        private byte[] _rightFrontFoot;
        private byte[] _rightBackFoot;
        // ReSharper restore NotAccessedField.Local

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
        public override int LifeInterval => 100;

        protected override void OnDeath()
        {
            FallApart();
            Logger.LogYellow("Elephant has fallen apart...");
        }

        private void FallApart()
        {
            _leftTusk = _rightTusk = _leftFrontFoot = _rightFrontFoot = _leftBackFoot = _rightBackFoot = null;
        }
    }
}