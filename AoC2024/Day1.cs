using System;

namespace AoC2024
{
    public class Day1 : AoCSupport.Day
    {
        public override string DayNumber => "1";
        public override string Year => "2024";
        //public static void Main(string[] args)
        //{
        //    Day1 day1 = new Day1();
        //    day1.PartA();
        //    day1.PartB();
        //}
        private (int[], int[]) readLeftRight()
        {
            int[] left = new int[_input.Lines.Length];
            int[] right = new int[_input.Lines.Length];
            for (int i = 0; i < _input.Lines.Length; i++)
            {
                string[] split = _input.Lines[i].Split("   ");
                left[i] = int.Parse(split[0]);
                right[i] = int.Parse(split[1]);
            }
            return (left, right);
        }
        public override void PartA()
        {
            (int[] left, int[] right) = readLeftRight();

            Array.Sort(left);
            Array.Sort(right);

            int total = 0;
            for (int i = 0; i < left.Length; i++)
            {
                total += Math.Abs(left[i] - right[i]);
            }
            Console.WriteLine(total);
        }
        public override void PartB()
        {
            (int[] left, int[] right) = readLeftRight();

            int total = 0;
            foreach (int i in left)
            {
                foreach (int j in right)
                {
                    if (i == j)
                    {
                        total += i;
                    }
                }
            }

            Console.WriteLine(total);
        }
    }
}