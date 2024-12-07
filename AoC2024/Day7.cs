namespace AoC2024
{
    internal class Day7 : AoCSupport.Day
    {
        public override string DayNumber => "7";
        public override string Year => "2024";
        public override string PartA()
        {
            double validSum = 0;

            foreach (string line in _input.Lines)
            {
                (double testValue, List<double> operands) = ParseEquation(line);
                validSum += CountValidCombinations(testValue, operands) > 0 ? testValue : 0;
            }
            return validSum.ToString();
        }
        public override string PartB()
        {
            return "";
        }
        private (double, List<double>) ParseEquation(string equation)
        {
            string[] eqSplit = equation.Split(": ");
            if (eqSplit.Length != 2)
            {
                throw new ArgumentException($"Unable to parse line [{equation}]");
            }
            List<double> operands = new List<double>(Array.ConvertAll<string, double>(eqSplit[1].Split(' '), double.Parse));
            return (double.Parse(eqSplit[0]), operands);
        }
        private int CountValidCombinations(double goal, List<double> operands)
        {
            int totalValid = 0;

            if (operands.Count == 1)
            {
                if (goal == operands[0])
                {
                    totalValid++;
                }
                return totalValid;
            }

            List<double> slicedOperands = operands.Slice(0, operands.Count - 1);

            totalValid += CountValidCombinations(goal - operands.Last(), slicedOperands);
            totalValid += CountValidCombinations(goal / operands.Last(), slicedOperands);

            return totalValid;
        }
    }
}