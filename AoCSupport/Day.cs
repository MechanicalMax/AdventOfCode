using System;

namespace AoCSupport
{
    public class Day
    {
        public string DayNumber { get; init; }
        protected Input _input;
        public Day(string dayNumber)
        {
            int numericDay = int.Parse(dayNumber);
            if (!(numericDay > 0 && numericDay <= 25))
            { 
                throw new ArgumentException("Provided day is not in [1, 25]");
            }
            DayNumber = dayNumber;
            _input = new Input(dayNumber);
        }
        public void run()
        {
            this.partA();
            this.partB();
        }
        public virtual void partA() { Console.WriteLine("No Part A"); }
        public virtual void partB() { Console.WriteLine("No Part B"); }
    }
}
