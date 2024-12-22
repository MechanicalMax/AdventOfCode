namespace AoC2024
{
    internal class Day22 : AoCSupport.Day
    {
        public override string DayNumber => "22";
        public override string Year => "2024";
        public override string PartA()
        {
            long sum = 0;

            foreach (string start in _input.Lines)
            {
                long curResult = long.Parse(start);
                for (int _ = 0; _ < 2000; _++)
                {
                    curResult = evolveNumber(curResult);
                }
                sum += curResult;
            }

            return sum.ToString();
        }
        public override string PartB()
        {
            var sequenceTotals = new Dictionary<(int, int, int, int), int>();

            foreach (string start in _input.Lines)
            {
                var seenSequences = new HashSet<(int, int, int, int)>();
                long curResult = long.Parse(start);
                int[] prices = new int[2000];

                for (int i = 0; i < prices.Length; i++)
                {
                    prices[i] = (int) (curResult % 10);
                    curResult = evolveNumber(curResult);
                }

                for (int i = 4; i < prices.Length; i++)
                {
                    var changes = (
                        prices[i - 3] - prices[i - 4],
                        prices[i - 2] - prices[i - 3],
                        prices[i - 1] - prices[i - 2],
                        prices[i] - prices[i - 1]
                        );
                    if (seenSequences.Contains(changes))
                    {
                        continue;
                    }
                    seenSequences.Add(changes);
                    if (sequenceTotals.ContainsKey(changes))
                    {
                        sequenceTotals[changes] += prices[i];
                    }
                    else
                    {
                        sequenceTotals[changes] = prices[i];
                    }
                }
            }

            return sequenceTotals.Values.Max().ToString();
        }
        private long evolveNumber(long secretNumber)
        {
            long stepOne = secretNumber << 6;
            secretNumber ^= stepOne;
            secretNumber %= 16777216;
            
            long stepTwo = secretNumber >> 5;
            secretNumber ^= stepTwo;
            secretNumber %= 16777216;

            long stepThree = secretNumber << 11;
            secretNumber ^= stepThree;
            secretNumber %= 16777216;
            return secretNumber;
        }
    }
}