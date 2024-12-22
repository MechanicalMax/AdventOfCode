namespace AoC2024
{
    internal class Day21 : AoCSupport.Day
    {
        public override string DayNumber => "21";
        public override string Year => "2024";
        private const char NoKey = 'X';
        private static readonly char[,] NumPad =
        {
            {'7', '8', '9'},
            {'4', '5', '6'},
            {'1', '2', '3'},
            {NoKey, '0', 'A'}
        };
        private static readonly char[,] DirectionalPad =
        {
            {NoKey, '^', 'A'},
            {'<', 'v', '>'}
        };
        private static readonly Dictionary<(int, int), char> ChangeVectorArrow = new Dictionary<(int, int), char>
        {
            [(0, 1)] = '>',
            [(-1, 0)] = '^',
            [(0, -1)] = '<',
            [(1, 0)] = 'v',
        };
        public override string PartA()
        {
            var codeSequences = new Dictionary<string, string>();

            foreach (var code in new string[] {"029A", "980A", "179A", "456A", "379A"})
            {
                var paths = new HashSet<string>();

                string robotTwoSequence = ConvertPadToDirectional(code, NumPad);
                paths.UnionWith(GetAllShortestPaths(robotTwoSequence));

                var robotThreePaths = new HashSet<string>();
                foreach (var path in paths)
                {
                    string robotThreeSequence = ConvertPadToDirectional(path, DirectionalPad);
                    robotThreePaths.UnionWith(GetAllShortestPaths(robotThreeSequence));
                }
                paths = robotThreePaths;

                var operatorPaths = new HashSet<string>();
                foreach (var path in paths)
                {
                    string operatorSequence = ConvertPadToDirectional(path, DirectionalPad);
                    operatorPaths.UnionWith(GetAllShortestPaths(operatorSequence));
                }
                paths = operatorPaths;

                var finalPaths = paths.ToList();
                var minPathLength = int.MaxValue;
                string minPath = "";
                foreach (var path in finalPaths)
                {
                    if (path.Length < minPathLength)
                    {
                        minPathLength = path.Length;
                        minPath = path;
                    }
                }

                codeSequences[code] = minPath;
            }

            int complexitySum = 0;
            foreach (var codeInfo in codeSequences)
            {
                complexitySum += CalculateCodeComplexity(codeInfo.Key, codeInfo.Value.Length);
            }
            return complexitySum.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private string DirectionalPath(char startKey, char endKey, char[,] interfacePad)
        {
            var startPoint = FindKeyPointOnPad(startKey, interfacePad);
            var endPoint = FindKeyPointOnPad(endKey, interfacePad);

            var visitedPoints = new HashSet<(int, int)>();
            var queue = new Queue<((int, int), string)>();

            queue.Enqueue((startPoint, ""));
            visitedPoints.Add(startPoint);

            while (queue.Count > 0)
            {
                (var curPoint, string curPath) = queue.Dequeue();
                if (curPoint == endPoint)
                {
                    return curPath + 'A';
                }

                foreach (var NeighborInfo in ChangeVectorArrow)
                {
                    var nextPoint = (curPoint.Item1 + NeighborInfo.Key.Item1,
                        curPoint.Item2 + NeighborInfo.Key.Item2);
                    if (IsInPadBounds(nextPoint.Item1, nextPoint.Item2, interfacePad)
                        && !visitedPoints.Contains(nextPoint))
                    {
                        queue.Enqueue(((nextPoint), curPath + NeighborInfo.Value));
                    }
                }
            }

            throw new Exception("End Key Not Found");
        }
        private string ConvertPadToDirectional(string sequence, char[,] interfacePad)
        {
            string newSequence = "";
            char lastKey = 'A';
            foreach (var keypress in sequence)
            {
                newSequence += DirectionalPath(lastKey, keypress, interfacePad);
                lastKey = keypress;
            }
            return newSequence;
        }
        private (int, int) FindKeyPointOnPad(char key, char[,] pad)
        {
            for (int row = 0; row < pad.GetLength(0); row++)
            {
                for (int col = 0; col < pad.GetLength(1); col++)
                {
                    if (pad[row, col] == key)
                    {
                        return (row, col);
                    }
                }
            }
            throw new Exception("Key Not Found");
        }
        private bool IsInPadBounds(int row, int col, char[,] pad)
        {
            return (row >= 0 && col >= 0
                && row < pad.GetLength(0) && col < pad.GetLength(1)
                && pad[row, col] != NoKey);
        }
        private int CalculateCodeComplexity(string code, int sequenceLength)
        {
            return int.Parse(code.Substring(0, 3)) * sequenceLength;
        }
        private HashSet<string> GetAllShortestPaths(string directionalInstructions)
        {
            var result = new HashSet<string>();

            int nextAIndex = directionalInstructions.IndexOf('A');
            if (nextAIndex == -1)
            {
                result.Add("");
                return result;
            }

            var firstMovementsPermutations = Permute(directionalInstructions.Substring(0, nextAIndex));

            foreach (var ending in GetAllShortestPaths(directionalInstructions.Substring(nextAIndex + 1)))
            {
                foreach (var permutation in firstMovementsPermutations)
                {
                    result.Add(permutation + "A" + ending);
                }
            }

            return result;
        }
        // Below functions modified from Geeks For Geeks
        // https://www.geeksforgeeks.org/write-a-c-program-to-print-all-permutations-of-a-given-string/
        private static string Swap(string s, int i, int j)
        {
            char[] charArray = s.ToCharArray();
            char temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            return new string(charArray);
        }
        private static HashSet<string> PermuteRec(string s, int idx, HashSet<string> permutations)
        {
            if (idx == s.Length - 1)
            {
                permutations.Add(s);
                return permutations;
            }

            for (int i = idx; i < s.Length; i++)
            {
                s = Swap(s, idx, i);

                permutations.UnionWith(PermuteRec(s, idx + 1, permutations));

                s = Swap(s, idx, i);
            }

            return permutations;
        }
        private Dictionary<string, HashSet<string>> SeenPermutes = new Dictionary<string, HashSet<string>>();
        private HashSet<string> Permute(string s)
        {
            if (SeenPermutes.ContainsKey(s))
            {
                return SeenPermutes[s];
            }
            else
            {
                if (s == "")
                {
                    var emptyStringSet = new HashSet<string>();
                    emptyStringSet.Add("");
                    return emptyStringSet;
                }
                var permutes = PermuteRec(s, 0, new HashSet<string>());
                SeenPermutes[s] = permutes;
                return permutes;
            }
        }
    }
}