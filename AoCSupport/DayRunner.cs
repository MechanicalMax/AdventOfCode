namespace AoCSupport
{
    public class DayRunner
    {
        public Day Day { get; set; }
        public DayRunner(Day day)
        {
            Day = day;
        }
        public void Run()
        {
            Console.WriteLine($"Advent of Code {Day.Year} - Day {Day.DayNumber}");
            Console.WriteLine("Part A:");
            Console.WriteLine(Day.PartA());
            Console.WriteLine("Part B:");
            Console.WriteLine(Day.PartB());
            Console.WriteLine();
        }
    }
}
