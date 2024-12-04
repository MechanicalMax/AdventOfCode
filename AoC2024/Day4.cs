using System.Text.RegularExpressions;

namespace AoC2024
{
    public sealed class Day4 : AoCSupport.Day
    {
        public override string DayNumber => "4";
        public override string Year => "2024";
        public override string PartA()
        {
            Regex xmasPattern = new Regex("(?=XMAS|SAMX)");

            int total = 0;
            foreach (string[] direction in GetAllDirections(_input.Lines))
            {
                total += CountWordOccurrences(direction, xmasPattern);
            }

            return total.ToString();
        }
        public override string PartB()
        {
            return "";
        }
        private string[] GetVerticalLines(string[] horizontal)
        {
            string[] vertical = new string[horizontal[0].Length];
            for (int col = 0; col < horizontal[0].Length; col++)
            {
                string newRow = "";
                for (int row = 0; row < horizontal.Length; row++)
                {
                    newRow += horizontal[row][col];
                }
                vertical[col] = newRow;
            }
            return vertical;
        }
        private string[][] GetAllDirections(string[] horizontal)
        {
            return new string[][] { horizontal, 
                    GetVerticalLines(horizontal),
                    };
        }
        private int CountWordOccurrences(string[] search, Regex word)
        {
            int count = 0;
            foreach (string s in search)
            {
                count += word.Count(s);
            }
            return count;
        }
    }
}
