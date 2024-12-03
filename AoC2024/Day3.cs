using System.Text.RegularExpressions;

namespace AoC2024
{
    internal class Day3 : AoCSupport.Day
    {
        public override string DayNumber => "3";
        public override string Year => "2024";
        public override string PartA()
        {
            Regex mulRegex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");

            int total = 0;

            foreach (Match match in mulRegex.Matches(_input.Raw))
            {
                int numOne = int.Parse(match.Groups[1].Value);
                int numTwo = int.Parse(match.Groups[2].Value);
                total += numOne * numTwo;
            }
            
            return total.ToString();
        }
        public override string PartB()
        {
            return "";
        }
    }
}
