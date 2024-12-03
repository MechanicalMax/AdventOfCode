using System.Text.RegularExpressions;

namespace AoC2024
{
    public sealed class Day3 : AoCSupport.Day
    {
        public override string DayNumber => "3";
        public override string Year => "2024";
        private Regex _mulRegex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");
        public override string PartA()
        {
            int total = 0;
            foreach (Match match in _mulRegex.Matches(_input.Raw))
            {
                int numOne = int.Parse(match.Groups[1].Value);
                int numTwo = int.Parse(match.Groups[2].Value);
                total += numOne * numTwo;
            }
            
            return total.ToString();
        }
        public override string PartB()
        {
            Regex doRegex = new Regex("do\\(\\)");
            Regex dontRegex = new Regex("don't\\(\\)");

            List<Match> doMatches = doRegex.Matches(_input.Raw).Cast<Match>().ToList();
            List<Match> dontMatches = dontRegex.Matches(_input.Raw).Cast<Match>().ToList();

            List<int> doIndexes = doMatches.ConvertAll(m => m.Index);
            List<int> dontIndexes = dontMatches.ConvertAll(m => m.Index);

            int total = 0;
            bool enabled = true;
            foreach (Match match in _mulRegex.Matches(_input.Raw))
            {
                int closestDoIndex = FindClosestSmallNumber(doIndexes, match.Index);
                int closestDontIndex = FindClosestSmallNumber(dontIndexes, match.Index);

                if (closestDontIndex != -1)
                {
                    if (closestDoIndex == -1)
                    {
                        enabled = false;
                    }
                    else
                    {
                        enabled = doIndexes[closestDoIndex] > dontIndexes[closestDontIndex];
                    }
                }


                if (enabled)
                {
                    int numOne = int.Parse(match.Groups[1].Value);
                    int numTwo = int.Parse(match.Groups[2].Value);
                    total += numOne * numTwo;
                }
            }

            return total.ToString();
        }

        private static int FindClosestSmallNumber(List<int> ints, int target)
        {
            for (int i = 0; i < ints.Count; i++)
            {
                if (ints[i] >= target)
                {
                    return i - 1;
                }
            }
            return ints.Count - 1;
        }
    }
}
