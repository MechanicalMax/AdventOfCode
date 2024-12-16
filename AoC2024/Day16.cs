namespace AoC2024
{
    internal class Day16 : AoCSupport.Day
    {
        public override string DayNumber => "16";
        public override string Year => "2024";
        private class GridInfo
        {
            public char[,] Grid;
            public (int, int) StartLocation;
            public (int, int) EndLocation;
            public (int, int)[] Directions = { (0, 1), (-1, 0), (0, -1), (1, 0) };
            public int StartDirection = 0;
            public GridInfo(string[] input)
            {
                Grid = new char[input.Length, input[0].Length];
                for (int row = 0; row < input.Length; row++)
                {
                    for (int col = 0; col < input[row].Length; col++)
                    {
                        char point = input[row][col];
                        if (point == 'S')
                        {
                            StartLocation = (row, col);
                        }
                        else if (point == 'E')
                        {
                            EndLocation = (row, col);
                        }
                        Grid[row, col] = point;
                    }
                }
            }
            public HashSet<(Transform, int)> GetPossibleMoves(Transform transform)
            {
                var moves = new HashSet<(Transform, int)>();

                moves.Add((new Transform(transform.Row, transform.Col, (transform.Direction + 1) % 4), 1000));
                moves.Add((new Transform(transform.Row, transform.Col, (transform.Direction + 3) % 4), 1000));
                int stepRow = transform.Row + Directions[transform.Direction].Item1;
                int stepCol = transform.Col + Directions[transform.Direction].Item2;
                if (Grid[stepRow, stepCol] != '#')
                {
                    moves.Add((new Transform(stepRow, stepCol, transform.Direction), 1));
                }

                return moves;
            }
            public bool IsFacingWall(Transform transform)
            {
                int stepRow = transform.Row + Directions[transform.Direction].Item1;
                int stepCol = transform.Col + Directions[transform.Direction].Item2;
                return (Grid[stepRow, stepCol] == '#');
            }
            public bool IsAtEnd(Transform transform)
            {
                return (transform.Row == EndLocation.Item1 && transform.Col == EndLocation.Item2);
            }
        }
        private struct Transform
        {
            public int Row { get; init; }
            public int Col { get; init; }
            public int Direction { get; init; }
            public Transform (int row, int col, int direction)
            {
                Row = row; Col = col; Direction = direction;
            }
        }
        public override string PartA()
        {
            var gridInfo = new GridInfo(_input.Lines);// "#################\r\n#...#...#...#..E#\r\n#.#.#.#.#.#.#.#.#\r\n#.#.#.#...#...#.#\r\n#.#.#.#.###.#.#.#\r\n#...#.#.#.....#.#\r\n#.#.#.#.#.#####.#\r\n#.#...#.#.#.....#\r\n#.#.#####.#.###.#\r\n#.#.#.......#...#\r\n#.#.###.#####.###\r\n#.#.#...#.....#.#\r\n#.#.#.#####.###.#\r\n#.#.#.........#.#\r\n#.#.#.#########.#\r\n#S#.............#\r\n#################".Trim().Split("\n"));//_input.Lines);

            var transformCosts = new Dictionary<Transform, int>();
            var adjacentTransforms = new Stack<(Transform, int)>();
            
            var curTransform = new Transform(
                gridInfo.StartLocation.Item1,
                gridInfo.StartLocation.Item2,
                gridInfo.StartDirection
                );

            transformCosts[curTransform] = 0;
            adjacentTransforms.Push((curTransform, 0));

            while (adjacentTransforms.Count > 0)
            {
                (curTransform, int curCost) = adjacentTransforms.Pop();

                foreach ((Transform newTransform, int additionalCost) in gridInfo.GetPossibleMoves(curTransform))
                {
                    if (gridInfo.IsFacingWall(newTransform)
                        && additionalCost == 1000)
                    {
                        continue;
                    }

                    int newCost = curCost + additionalCost;
                    if (transformCosts.ContainsKey(newTransform))
                    {
                        if (transformCosts[newTransform] > newCost)
                        {
                            transformCosts[newTransform] = newCost;
                            adjacentTransforms.Push((newTransform, newCost));
                        }
                    }
                    else
                    {
                        //if (transformCosts.ContainsKey(new Transform(newTransform.Row, newTransform.Col,
                        //    (newTransform.Direction + 2) % 4))
                        //    )
                        //{
                        //    continue;
                        //}
                        transformCosts.Add(newTransform, newCost);
                        adjacentTransforms.Push((newTransform, newCost));
                    }
                }
            }

            var endLocationOne = new Transform(gridInfo.EndLocation.Item1, gridInfo.EndLocation.Item2, 0);
            var endLocationTwo = new Transform(gridInfo.EndLocation.Item1, gridInfo.EndLocation.Item2, 1);

            int endCostOne = transformCosts.ContainsKey(endLocationOne) ? transformCosts[endLocationOne] : int.MaxValue;
            int endCostTwo = transformCosts.ContainsKey(endLocationTwo) ? transformCosts[endLocationTwo] : int.MaxValue;

            return int.Min(endCostOne, endCostTwo).ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
    }
}