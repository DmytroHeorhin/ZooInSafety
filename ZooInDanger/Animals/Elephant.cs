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
            _rightTusk = new byte[0x1400000];
            _leftTusk = new byte[0x1400000];
            _leftFrontFoot=  new byte[0x1400000];
            _leftBackFoot = new byte[0x1400000];
            _rightFrontFoot = new byte[0x1400000];
            _rightBackFoot = new byte[0x1400000];

        }

        public override int LifeInterval => 100;
    }
}