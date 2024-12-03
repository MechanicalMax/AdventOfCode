namespace AoCSupport
{
    public abstract class Day
    {
        protected Input _input;
        public abstract string DayNumber { get; }
        public abstract string Year { get; }
        public Day()
        {
            _input = new Input(DayNumber, Year);
        }
        public abstract string PartA();
        public abstract string PartB();
    }
}
