namespace AoC2024
{
    public sealed class Day6 : AoCSupport.Day
    {
        public override string DayNumber => "6";
        public override string Year => "2024";
        private class Grid
        {
            public char[][] Layout { get; set; }
            public (int, int) GuardStart { get; init; }
            public List<(int, int)> TraveledPoints { get; private set; }
            private (int, int)[] directions = [(-1, 0), (0, 1), (1, 0), (0, -1)];
            internal Grid(string[] lines)
            {
                Layout = new char[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    Layout[i] = lines[i].ToCharArray();
                }
                GuardStart = FindGuardStart();
                TraveledPoints = new List<(int, int)>();
            }
            private (int, int) FindGuardStart()
            {
                (int, int) guardStart = (-1, -1);
                
                bool foundStart = false;
                for (int row = 0; row < this.Layout.Length; row++)
                {
                    for (int col = 0; col < this.Layout[row].Length; col++)
                    {
                        if (this.Layout[row][col] == '^')
                        {
                            guardStart = (row, col);
                            foundStart = true;
                            break;
                        }
                    }
                    if (foundStart)
                    {
                        break;
                    }
                }
                return guardStart;
            }
            public void DisplayLayout()
            {
                foreach (char[] item in  this.Layout)
                {
                    Console.WriteLine(item);
                }
            }
            public bool IsInGrid((int, int) pos)
            {
                return (pos.Item1 >= 0 && pos.Item2 >= 0
                    && pos.Item1 < this.Layout.Length
                    && pos.Item2 < this.Layout[0].Length);
            }
            public bool IsOnEdge((int, int) pos)
            {
                return (pos.Item1 == 0 || pos.Item2 == 0
                    || pos.Item1 == this.Layout.Length - 1
                    || pos.Item2 == this.Layout[0].Length - 1);
            }
            public (int, int) MarkTraveled((int, int) curPosition, int curDirection)
            {
                var nextPosition = (curPosition.Item1 + directions[curDirection].Item1,
                    curPosition.Item2 + directions[curDirection].Item2);
                while (IsInGrid(nextPosition) && Layout[nextPosition.Item1][nextPosition.Item2] != '#')
                {
                    Layout[nextPosition.Item1][nextPosition.Item2] = '0';
                    curPosition = nextPosition;
                    TraveledPoints.Add(curPosition);
                    nextPosition = (curPosition.Item1 + directions[curDirection].Item1,
                        curPosition.Item2 + directions[curDirection].Item2);
                }
                return curPosition;
            }
            public int CountDistinctPositions()
            {
                int count = 1;
                foreach (var item in Layout)
                {
                    foreach (char c in item)
                    {
                        if (c == '0')
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
            public bool HasLoop()
            {
                // row, col, direction
                var pastTurns = new List<(int, int, int)>();
                var curPosition = GuardStart;
                int curDirection = 0;
                while (true)
                {
                    var nextPosition = (curPosition.Item1 + directions[curDirection].Item1,
                        curPosition.Item2 + directions[curDirection].Item2);
                    while (IsInGrid(nextPosition) && Layout[nextPosition.Item1][nextPosition.Item2] != '#')
                    {
                        curPosition = nextPosition;
                        nextPosition = (curPosition.Item1 + directions[curDirection].Item1,
                            curPosition.Item2 + directions[curDirection].Item2);
                    }

                    if (IsOnEdge(curPosition))
                    {
                        return false;
                    }

                    if (pastTurns.Contains((curPosition.Item1, curPosition.Item2, curDirection)))
                    {
                        Console.WriteLine("^Has Loop");
                        DisplayLayout();
                        return true;
                    }
                    pastTurns.Add((curPosition.Item1, curPosition.Item2, curDirection));

                    curDirection++;
                    curDirection %= 4;
                }
            }
        }
        public override string PartA()
        {
            Grid grid = new Grid(_input.Lines);

            (int, int) curPosition = grid.GuardStart;
            int curDirection = 0;

            while (grid.IsInGrid(curPosition))
            {
                curPosition = grid.MarkTraveled(curPosition, curDirection);
                if (grid.IsOnEdge(curPosition))
                {
                    break;
                }
                curDirection++;
                curDirection %= 4;
            }
            //grid.DisplayLayout();
            return grid.CountDistinctPositions().ToString();
        }
        public override string PartB()
        {
            Grid grid = new Grid(_input.Lines);
            int totalLoops = 0;

            (int, int) curPosition = grid.GuardStart;
            int curDirection = 0;

            while (grid.IsInGrid(curPosition))
            {
                curPosition = grid.MarkTraveled(curPosition, curDirection);
                if (grid.IsOnEdge(curPosition))
                {
                    break;
                }
                curDirection++;
                curDirection %= 4;
            }

            var searchPoints = new List<(int, int)>();
            for (int i = 0; i < grid.Layout.Length; i++)
            {
                for (int j = 0; j < grid.Layout[0].Length; j++)
                {
                    searchPoints.Add((i, j));
                }
            }

            foreach ((int, int) traveledPoint in searchPoints)
            {
                char original = grid.Layout[traveledPoint.Item1][traveledPoint.Item2];
                grid.Layout[traveledPoint.Item1][traveledPoint.Item2] = '#';

                Console.WriteLine($"{traveledPoint.Item1} {traveledPoint.Item2}");

                totalLoops += grid.HasLoop() ? 1 : 0;

                grid.Layout[traveledPoint.Item1][traveledPoint.Item2] = original;
            }

            return totalLoops.ToString();
        }
    }
}
