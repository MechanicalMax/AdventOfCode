namespace AoC2024
{
    internal class Day21 : AoCSupport.Day
    {
        /*
         * Mess of assorted functions; core code from HyperNeutrino:
         * https://youtu.be/dqzAaj589cM?si=z7Ph8PLwr4WjwNwU
         */
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
            var directionalSequences = ComputeSequences(DirectionalPad);

            long complexitySum = 0;
            foreach (var code in _input.Lines)//new string[] {"029A", "980A", "179A", "456A", "379A"})
            {
                var paths = new HashSet<string>();

                var possibleInputs = ConvertPadToDirectional(code, NumPad);

                long minLength = long.MaxValue;
                foreach (var input in possibleInputs)
                {
                    minLength = long.Min(minLength, ComputeLength(input, 2, directionalSequences));
                }

                complexitySum += CalculateCodeComplexity(code, minLength);
            }

            return complexitySum.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private Dictionary<(char, char), List<string>> ComputeSequences(char[,] keypad)
        {
            var positions = new Dictionary<char, (int, int)>();
            for (int row = 0; row < keypad.GetLength(0); row++)
            {
                for (int col = 0; col < keypad.GetLength(1); col++)
                {
                    if (keypad[row, col] != NoKey)
                    {
                        positions[keypad[row, col]] = ((row, col));
                    }
                }
            }
            var sequences = new Dictionary<(char, char), List<string>>();

            foreach (var startInfo in positions)
            {
                foreach (var endInfo in positions)
                {
                    if (startInfo.Key == endInfo.Key)
                    {
                        sequences[(startInfo.Key, endInfo.Key)] = new List<string>() { "A" };
                        continue;
                    }

                    var possibilePaths = new List<string>();
                    var queue = new Queue<((int, int), string)>();
                    queue.Enqueue((startInfo.Value, ""));
                    int optimalLength = int.MaxValue;
                    bool foundAllPossible = false;
                    while (queue.Count > 0)
                    {
                        (var curPoint, string moves) = queue.Dequeue();

                        foreach (var NeighborInfo in ChangeVectorArrow)
                        {
                            var nextPoint = (curPoint.Item1 + NeighborInfo.Key.Item1,
                                curPoint.Item2 + NeighborInfo.Key.Item2);
                            if (!IsInPadBounds(nextPoint.Item1, nextPoint.Item2, keypad))
                            {
                                continue;
                            }
                            if (nextPoint == endInfo.Value)
                            {
                                if (optimalLength < moves.Length + 1)
                                {
                                    foundAllPossible = true;
                                    break;
                                }
                                optimalLength = moves.Length + 1;
                                possibilePaths.Add(moves + NeighborInfo.Value + "A");
                            }
                            else
                            {
                                queue.Enqueue((nextPoint, moves + NeighborInfo.Value));
                            }
                        }
                        if (foundAllPossible == true)
                        {
                            break;
                        }
                    }

                    sequences[(startInfo.Key, endInfo.Key)] = possibilePaths;
                }
            }

            return sequences;
        }
        private Dictionary<(string, int), long> SeenComputeLengths = new Dictionary<(string, int), long>();
        private long ComputeLength(string sequence, int depth, Dictionary<(char, char), List<string>> directionalPadSequences)
        {
            if (SeenComputeLengths.ContainsKey((sequence, depth)))
            {
                return SeenComputeLengths[(sequence, depth)];
            }

            string fullSequence = "A" + sequence;
            if (depth == 1)
            {
                long sum = 0;
                for (int i = 0; i < fullSequence.Length - 1; i++)
                {
                    sum += directionalPadSequences[(fullSequence[i], fullSequence[i + 1])][0].Length;
                }
                SeenComputeLengths[(sequence, depth)] = sum;
                return sum;
            }

            long length = 0;
            for (int i = 0; i < fullSequence.Length - 1; i++)
            {
                long minimumForButton = long.MaxValue;
                foreach (var subseq in directionalPadSequences[(fullSequence[i], fullSequence[i + 1])])
                {
                    minimumForButton = long.Min(minimumForButton, ComputeLength(subseq, depth - 1, directionalPadSequences));
                }
                length += minimumForButton;
            }

            SeenComputeLengths[(sequence, depth)] = length;
            return length;
        }
        private List<string> ConvertPadToDirectional(string sequence, char[,] interfacePad)
        {
            var numpadSequences = ComputeSequences(NumPad);
            var possibleSequences = new List<string>() { "" };

            string fullSequence = "A" + sequence;
            for (int i = 0; i < fullSequence.Length - 1; i++)
            {
                var builtSequences = new List<string>();
                foreach (var nextSequence in numpadSequences[(fullSequence[i], fullSequence[i + 1])])
                {
                    foreach (var currentSequence in possibleSequences)
                    {
                        builtSequences.Add(currentSequence + nextSequence);
                    }
                }
                possibleSequences = builtSequences;
            }

            return possibleSequences;
        }
        private bool IsInPadBounds(int row, int col, char[,] pad)
        {
            return (row >= 0 && col >= 0
                && row < pad.GetLength(0) && col < pad.GetLength(1)
                && pad[row, col] != NoKey);
        }
        private long CalculateCodeComplexity(string code, long sequenceLength)
        {
            return long.Parse(code.Substring(0, 3)) * sequenceLength;
        }
    }
}