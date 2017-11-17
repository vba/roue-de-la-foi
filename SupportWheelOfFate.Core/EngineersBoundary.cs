namespace SupportWheelOfFate.Core
{
    public sealed class EngineersBoundary
    {
        public static EngineersBoundary Min = new EngineersBoundary(4); // Minimum of my solution 
        public static EngineersBoundary Max = new EngineersBoundary(97); // Superficial for purpose of the test but can be Int.MaxValue 

        private EngineersBoundary(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
