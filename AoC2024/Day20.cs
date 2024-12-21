namespace AoC2024
{
    internal class Day20 : AoCSupport.Day
    {
        public override string DayNumber => "20";
        public override string Year => "2024";
        private class GridInfo
        {
            public char[,] Grid { get; init; }
            public (int, int) Start { get; init; }
            public (int, int) End { get; init; }
            private static readonly (int, int)[] Directions = [(0, 1), (-1, 0), (0, -1), (1, 0)];
            public GridInfo(string[] inputLines)
            {
                this.Grid = new char[inputLines.Length, inputLines[0].Length];
                for (int row = 0; row < inputLines.Length; row++)
                {
                    for (int col = 0; col < inputLines[row].Length; col++)
                    {
                        this.Grid[row, col] = inputLines[row][col];
                        if (Grid[row, col] == 'S')
                        {
                            this.Start = (row, col);
                        }
                        else if (Grid[row, col] == 'E')
                        {
                            this.End = (row, col);
                        }
                    }
                }
            }
            public GridInfo(GridInfo gridInfo, (int, int) cheatStart, (int, int) cheatEnd)
            {
                Grid = gridInfo.Grid.Clone() as char[,] ?? new char[0,0];
                this.Start = gridInfo.Start;
                this.End = gridInfo.End;
                this.Grid[cheatEnd.Item1, cheatEnd.Item2] = '2';
                this.Grid[(cheatStart.Item1 + cheatEnd.Item1) / 2,
                    (cheatStart.Item2 + cheatEnd.Item2) / 2] = '1';
            }
            public List<((int, int), (int, int))> PossibleCheats((int, int) point)
            {
                return PossibleCheats(point.Item1, point.Item2);
            }
            public List<((int, int), (int, int))> PossibleCheats(int row, int col)
            {
                var startEndPairs = new List<((int, int), (int, int))>();
                foreach (var direction in Directions)
                {
                    var possibleNeighbor = (row + direction.Item1, col + direction.Item2);
                    if (IsInGrid(possibleNeighbor) && Grid[possibleNeighbor.Item1, possibleNeighbor.Item2] == '#')
                    {
                        var endLocation = (possibleNeighbor.Item1 + direction.Item1, possibleNeighbor.Item2 + direction.Item2);
                        if (IsInGrid(endLocation) && Grid[endLocation.Item1, endLocation.Item2] != '#')
                        {
                            startEndPairs.Add(((row, col), endLocation));
                        }
                    }
                }
                return startEndPairs;
            }
            public List<(int, int)> AllNeighborsInGrid((int, int) point)
            {
                var neighbors = new List<(int, int)>();
                foreach (var direction in Directions)
                {
                    var possibleNeighbor = (point.Item1 + direction.Item1, point.Item2 + direction.Item2);
                    if (IsInGrid(possibleNeighbor))
                    {
                        neighbors.Add(possibleNeighbor);
                    }
                }
                return neighbors;
            }
            public List<(int, int)> ValidNeighborsInGrid((int, int) point)
            {
                return ValidNeighborsInGrid(point.Item1, point.Item2);
            }
            public List<(int, int)> ValidNeighborsInGrid(int row, int col)
            {
                var neighbors = new List<(int, int)>();
                foreach (var direction in Directions)
                {
                    var possibleNeighbor = (row + direction.Item1, col + direction.Item2);
                    if (IsInGrid(possibleNeighbor) && Grid[possibleNeighbor.Item1, possibleNeighbor.Item2] != '#')
                    {
                        neighbors.Add(possibleNeighbor);
                    }
                }
                return neighbors;
            }
            public bool IsInGrid((int, int) point)
            {
                return IsInGrid(point.Item1, point.Item2);
            }
            public bool IsInGrid(int row, int col)
            {
                return (row >= 0
                    && row < Grid.GetLength(0)
                    && col >= 0
                    && col < Grid.GetLength(1));
            }
        }
        public override string PartA()
        {
            var gridInfo = new GridInfo(_input.Lines);//"###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############".Split("\r\n"));
            var pointsFromStartToEnd = GetPointsFromStartToEnd(gridInfo);
            int minimumTimeSaved = 100;
            int maximumCheatTime = 2;

            int cheatCount = 0;
            var savingsInfo = new Dictionary<int, int>();

            for (int distanceFromStart = 0; distanceFromStart < pointsFromStartToEnd.Count - minimumTimeSaved; distanceFromStart++)
            {
                var curSearchPoint = pointsFromStartToEnd[distanceFromStart];
                for (int cheatLength = 2; cheatLength <= maximumCheatTime; cheatLength++)
                {
                    foreach (var surroundingPoint in GetPointsAroundPoint(cheatLength, curSearchPoint.Item1, curSearchPoint.Item2))
                    {
                        if (pointsFromStartToEnd.Contains(surroundingPoint))
                        {
                            int endPointDistance = pointsFromStartToEnd.IndexOf(surroundingPoint);
                            int timeSaved = endPointDistance - cheatLength - distanceFromStart;

                            if (timeSaved >= minimumTimeSaved)
                            {
                                cheatCount++;
                                if (savingsInfo.ContainsKey(timeSaved))
                                {
                                    savingsInfo[timeSaved]++;
                                }
                                else
                                {
                                    savingsInfo.Add(timeSaved, 1);
                                }
                            }
                        }
                    }
                }
            }

            return cheatCount.ToString();
        }
        public override string PartB()
        {
            var gridInfo = new GridInfo(_input.Lines);//"###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############".Split("\r\n"));
            var pointsFromStartToEnd = GetPointsFromStartToEnd(gridInfo);
            int minimumTimeSaved = 100;
            int maximumCheatTime = 20;

            int cheatCount = 0;
            var savingsInfo = new Dictionary<int, int>();

            for (int distanceFromStart = 0; distanceFromStart < pointsFromStartToEnd.Count - minimumTimeSaved; distanceFromStart++)
            {
                var curSearchPoint = pointsFromStartToEnd[distanceFromStart];
                for (int cheatLength = 2; cheatLength <= maximumCheatTime; cheatLength++)
                {
                    foreach (var surroundingPoint in GetPointsAroundPoint(cheatLength, curSearchPoint.Item1, curSearchPoint.Item2))
                    {
                        if (pointsFromStartToEnd.Contains(surroundingPoint))
                        {
                            int endPointDistance = pointsFromStartToEnd.IndexOf(surroundingPoint);
                            int timeSaved = endPointDistance - cheatLength - distanceFromStart;

                            if (timeSaved >= minimumTimeSaved)
                            {
                                cheatCount++;
                                if (savingsInfo.ContainsKey(timeSaved))
                                {
                                    savingsInfo[timeSaved]++;
                                }
                                else
                                {
                                    savingsInfo.Add(timeSaved, 1);
                                }
                            }
                        }
                    }
                }
            }

            return cheatCount.ToString();
        }
        private HashSet<(int, int)> GetPointsAroundPoint(int distance, int row, int col)
        {
            var surroundingPoints = new HashSet<(int, int)>();

            for (int rowChange = -distance; rowChange <= distance; rowChange++)
            {
                int positiveColChange = distance - Math.Abs(rowChange);
                surroundingPoints.Add((row + rowChange, col + positiveColChange));
                surroundingPoints.Add((row + rowChange, col - positiveColChange));
            }

            return surroundingPoints;
        }
        private List<(int, int)> GetPointsFromStartToEnd(GridInfo gridInfo)
        {
            var pointDistances = new List<(int, int)>();
            var queue = new Queue<(int, int)>();

            queue.Enqueue(gridInfo.Start);
            pointDistances.Add(gridInfo.Start);

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                foreach (var neighbor in gridInfo.ValidNeighborsInGrid(point))
                {
                    if (!pointDistances.Contains(neighbor))
                    {
                        pointDistances.Add(neighbor);
                        queue.Enqueue(neighbor);
                        continue;
                    }
                }
            }

            return pointDistances;
        }
        private static HashSet<((int, int), (int, int))> FindCheatLocations(GridInfo gridInfo)
        {
            var cheatPoints = new HashSet<((int, int), (int, int))>();
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<((int, int), int)>();

            queue.Enqueue((gridInfo.Start, 0));
            visited.Add(gridInfo.Start);

            while (queue.Count > 0)
            {
                (var point, int timeToReach) = queue.Dequeue();
                
                foreach (var cheatInfo in gridInfo.PossibleCheats(point))
                {
                    if (!visited.Contains(cheatInfo.Item2))
                    {
                        cheatPoints.Add(cheatInfo);
                    }
                }

                foreach (var neighbor in gridInfo.ValidNeighborsInGrid(point))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue((neighbor, timeToReach + 1));
                    }
                }
            }

            return cheatPoints;
        }
        private static int BreadthFirstSearchStartToEnd(GridInfo gridInfo)
        {
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<((int, int), int)>();

            queue.Enqueue((gridInfo.Start, 0));
            visited.Add(gridInfo.Start);

            while (queue.Count > 0)
            {
                (var point, int timeToReach) = queue.Dequeue();
                foreach (var neighbor in gridInfo.ValidNeighborsInGrid(point))
                {
                    if (neighbor == gridInfo.End)
                    {
                        return timeToReach + 1;
                    }
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue((neighbor, timeToReach + 1));
                    }
                }
            }

            return int.MaxValue;
        }
    }
}