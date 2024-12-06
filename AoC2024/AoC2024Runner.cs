using AoCSupport;

namespace AoC2024
{
    internal class AoC2024Runner
    {
        public static void Main(string[] args)
        {
            Day day = new Day6();

            // Choose day class from args?

            DayRunner dayRunner = new DayRunner(day);
            dayRunner.Run();
        }
    }
}
