using System.Diagnostics.CodeAnalysis;

namespace AoC2024
{
    internal class Day11 : AoCSupport.Day
    {
        public override string DayNumber => "11";
        public override string Year => "2024";
        public override string PartA()
        {
            return StoneCountAfterBlinks(_input.Raw, 25).ToString();
        }
        public override string PartB()
        {
            return StoneCountAfterBlinks(_input.Raw, 75).ToString();
        }
        private double StoneCountAfterBlinks(string raw, int blinks)
        {
            string[] initialStones = raw.Split(' ');
            
            var stones = new Dictionary<string, double>();
            for (int i = 0; i < initialStones.Length; i++)
            {
                stones[initialStones[i]] = 1;
            }

            while (blinks > 0)
            {
                var newStones = new Dictionary<string, double>();
                foreach (var stone in stones)
                {
                    string[] subStones = UpdateStone(stone.Key);
                    foreach (string subStone in  subStones)
                    {
                        if (newStones.ContainsKey(subStone))
                        {
                            newStones[subStone] += stone.Value;
                        }
                        else
                        {
                            newStones.Add(subStone, stone.Value);
                        }
                    }
                }
                stones = newStones;
                blinks--;
            }

            double count = 0;
            foreach (var stoneCount in stones.Values)
            {
                count += stoneCount;
            }
            return count;
        }
        private string[] UpdateStone(string stone)
        {
            if (stone == "0")
            {
                return ["1"];
            }
            else if (stone.Length % 2 == 0)
            {
                string left = stone.Substring(0, stone.Length / 2);
                string right = stone.Substring(stone.Length / 2, stone.Length / 2);
                return [double.Parse(left).ToString(), double.Parse(right).ToString()];
            }
            else
            {
                return [(double.Parse(stone) * 2024).ToString()];
            }
        }
    }
}