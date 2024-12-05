namespace AoC2024
{
    public sealed class Day5 : AoCSupport.Day
    {
        public override string DayNumber => "5";
        public override string Year => "2024";
        private class RuleComparison : System.Collections.IComparer
        {
            public List<(string, string)> Rules { get; init; }
            public RuleComparison(List<(string, string)> rules)
            {
                this.Rules = rules;
            }
            public int Compare(object? x, object? y)
            {
                foreach ((string left, string right) in Rules)
                {
                    if (string.Equals(x, left))
                    {
                        if (string.Equals(y, right))
                        {
                            return -1;
                        }
                    }
                }
                return 1;
            }
        }
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
            (List<(string, string)> rules, List<string[]> updates) = ParseLines(_input.Lines);
            int middlePageSum = 0;

            foreach (string[] update in updates)
            {
                if (!IsValidUpdate(rules, update))
                {
                    string[] fixedUpdateOrder = FixedUpdateOrder(rules, update);
                    middlePageSum += MiddlePageValue(fixedUpdateOrder);
                }
            }

            return middlePageSum.ToString();
        }
        private string[] FixedUpdateOrder(List<(string, string)> rules, string[] oldUpdate)
        {
            Array.Sort(oldUpdate, new RuleComparison(rules));
            return oldUpdate;
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
