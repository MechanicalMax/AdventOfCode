namespace AoC2024
{
    public sealed class Day5 : AoCSupport.Day
    {
        public override string DayNumber => "5";
        public override string Year => "2024";
        public override string PartA()
        {
            (List<(string, string)> rules, List<string[]> updates) = ParseLines(_input.Lines);
            int middlePageSum = 0;

            foreach (string[] update in updates)
            {
                if (IsValidUpdate(rules, update))
                {
                    middlePageSum += MiddlePageValue(update);
                }
            }
            
            return middlePageSum.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private bool IsValidUpdate(List<(string, string)> rules, string[] update)
        {
            foreach ((string left, string right) in rules)
            {
                int leftIndex = Array.IndexOf(update, left);
                if (leftIndex == -1)
                {
                    continue;
                }
                int rightIndex = Array.IndexOf(update, right);
                if (rightIndex == -1)
                {
                    continue;
                }
                if (rightIndex < leftIndex)
                {
                    return false;
                }
            }
            return true;
        }
        private int MiddlePageValue(string[] update)
        {
            int halfLengthIndex = update.Length / 2;
            return int.Parse(update[halfLengthIndex]);
        }
        private (List<(string, string)>, List<string[]>) ParseLines(string[] lines)
        {
            var rules = new List<(string, string)>();
            var updates = new List<string[]>();

            bool afterSpace = false;
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    afterSpace = true;
                    continue;
                }
                if (afterSpace)
                {
                    updates.Add(line.Split(','));
                }
                else
                {
                    rules.Add((line.Substring(0, 2), line.Substring(3, 2)));
                }
            }
            
            return (rules, updates);
        }
    }
}
