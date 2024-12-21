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
            var originalGrid = new GridInfo(_input.Lines);
            int originalFinishTime = BreadthFirstSearchStartToEnd(originalGrid);

            int count = 0;

            //foreach (var cheatInfo in FindCheatLocations(originalGrid))
            //{
            //    var updatedGrid = new GridInfo(originalGrid, cheatInfo.Item1, cheatInfo.Item2);
            //    int newFinishTime = BreadthFirstSearchStartToEnd(updatedGrid);

            //    if (originalFinishTime - newFinishTime >= 100)
            //    {
            //        count++;
            //    }
            //}

            return count.ToString();
        }
        public override string PartB()
        {
            var gridInfo = new GridInfo(_input.Lines);//"###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############".Split("\r\n"));
            var distancesFromStart = GetPointDistances(gridInfo);
            int minimumTimeSaved = 2;
            int maximumCheatTime = 2;

            int cheatCount = 0;
            var savingsInfo = new Dictionary<int, int>();

            for (int distanceFromStart = 0; distanceFromStart < distancesFromStart.Count - minimumTimeSaved; distanceFromStart++)
            {
                var cheatStartPoint = distancesFromStart[distanceFromStart];
                var visitedPoints = new HashSet<(int, int)>();
                var wallSearchFrontier = new List<(int, int)>() { cheatStartPoint };

                visitedPoints.Add(cheatStartPoint);

                for (int cheatTime = 1; cheatTime <= maximumCheatTime; cheatTime++)
                {
                    var newWallSearchFrontier = new List<(int, int)>();
                    foreach (var curFrontierPoint in wallSearchFrontier)
                    {
                        foreach (var nextFrontierpoint in gridInfo.AllNeighborsInGrid(curFrontierPoint))
                        {
                            if (visitedPoints.Contains(nextFrontierpoint))
                            {
                                continue;
                            }
                            visitedPoints.Add(nextFrontierpoint);

                            if (gridInfo.Grid[nextFrontierpoint.Item1, nextFrontierpoint.Item2] != '#')
                            {
                                int endpointDistancefromStart = distancesFromStart.IndexOf(nextFrontierpoint);
                                if (endpointDistancefromStart == -1)
                                {
                                    throw new Exception("Point not in path");
                                }

                                int timeSaved = endpointDistancefromStart - cheatTime - distanceFromStart;
                                Console.WriteLine($"Start:{distancesFromStart[distanceFromStart]} [{distanceFromStart}]; End: {nextFrontierpoint} [{endpointDistancefromStart}]; Savings: {timeSaved} [cheatTime: {cheatTime}]");

                                if (timeSaved >= minimumTimeSaved)
                                {
                                    cheatCount++;
                                    if (savingsInfo.ContainsKey(timeSaved))
                                    {
                                        savingsInfo[timeSaved]++;
                                    }
                                    else
                                    {
                                        savingsInfo.Add(timeSaved, 1 );
                                    }
                                }
                            }
                            else
                            {
                                newWallSearchFrontier.Add(nextFrontierpoint);
                            }
                        }
                    }
                    wallSearchFrontier = newWallSearchFrontier;
                }
            }

            return cheatCount.ToString();
        }
        private List<(int, int)> GetPointDistances(GridInfo gridInfo)
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