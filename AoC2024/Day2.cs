using System;

namespace AoC2024
{
    public class Day2 : AoCSupport.Day
    {
        public Day2(string day) : base(day) { }

        public static void Main(string[] args)
        {
            Day2 day2 = new Day2("2");
            day2.run();
        }
        private int[][] CreateCombinationsRemovingOne(int[] levels)
        {
            int[][] combinations = new int[levels.Length][];
            for (int i = 0; i < levels.Length; i++)
            {
                combinations[i] = new int[levels.Length - 1];
                int j = 0;
                int insertIndex = 0;
                while (j < levels.Length)
                {
                    if (j != i)
                    { 
                        combinations[i][insertIndex] = levels[j];
                        insertIndex++;
                    }
                    j++;
                }
            }
            return combinations;
        }
        private bool IsReportSafe(int[] levels)
        {
            bool increasing = levels[0] - levels[1] > 0;
            
            for (int i = 1; i < levels.Length; i++)
            {
                int lastNum = levels[i-1];
                int curNum = levels[i];
                int diff = lastNum - curNum;

                if (diff > 0 && increasing || diff < 0 && !increasing)
                {
                    if (Math.Abs(diff) >= 1 && Math.Abs(diff) <= 3)
                    {
                        continue;
                    }
                }

                return false;
            }
            
            return true;
        }

        public override void partA()
        {
            int total = 0;
            foreach (string l in _input.Lines)
            {
                int[] levels = Array.ConvertAll<string, int>(l.Split(" "), int.Parse);

                if (IsReportSafe(levels))
                {
                    total++;
                }
            }
            Console.WriteLine(total);
        }

        public override void partB()
        {
            int total = 0;
            foreach (string l in _input.Lines)
            {
                int[] levels = Array.ConvertAll<string, int>(l.Split(" "), int.Parse);
                int[][] reportsMissingOne = CreateCombinationsRemovingOne(levels);
                foreach (int[] r in reportsMissingOne)
                {
                    if (IsReportSafe(r))
                    {
                        total++;
                        break;
                    }
                }
            }
            Console.WriteLine(total);
        }
    }
}