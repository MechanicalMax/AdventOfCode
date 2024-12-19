using System.Text.RegularExpressions;

namespace AoC2024
{
    internal class Day19 : AoCSupport.Day
    {
        public override string DayNumber => "19";
        public override string Year => "2024";
        public override string PartA()
        {
            (string[] patterns, string[] designs) = ParsePatternsAndDesigns(_input.Lines);

            string regexString = $"^({string.Join('|',patterns)})+$";
            var patternMatcher = new Regex(regexString);

            int matches = 0;
            foreach (var design in designs)
            {
                if (patternMatcher.IsMatch(design)) {
                    matches++;
                }
            }

            return matches.ToString();
        }
        public override string PartB()
        {
            (string[] patterns, string[] designs) = ParsePatternsAndDesigns(_input.Lines);
            
            // Remember, AoC likes HUGE NUMBERS -> ints are too small
            long permutationSum = 0;
            foreach (var design in designs)
            {
                permutationSum += CountPermutations(design, patterns);
            }

            return permutationSum.ToString();
        }
        private Dictionary<string, long> PermutationMemo = [];
        private long CountPermutations(string design, string[] patterns)
        {
            if (design == "")
            {
                return 1;
            }

            if (PermutationMemo.ContainsKey(design))
            {
                return PermutationMemo[design];
            }

            long permutations = 0;
            foreach (var pattern in patterns)
            {
                if (design.StartsWith(pattern))
                {
                    permutations += CountPermutations(design[pattern.Length..], patterns);
                }
            }

            PermutationMemo[design] = permutations;

            return permutations;
        }
        private (string[], string[]) ParsePatternsAndDesigns(string[] lines)
        {
            return (lines[0].Split(", "), new ArraySegment<string>(lines, 2, lines.Length - 2).ToArray());
        }
    }
}