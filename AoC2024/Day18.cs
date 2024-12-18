namespace AoC2024
{
    internal class Day18 : AoCSupport.Day
    {
        public override string DayNumber => "18";
        public override string Year => "2024";
        private class GridInfo
        {
            public (int, int)[] FallingBytes { get; init; }
            public (int, int) Start { get; init; }
            public (int, int) End { get; init; }
            public (int, int)[] Directions = [(0, 1), (-1, 0), (0, -1), (1, 0)];
            public GridInfo(string[] fallingBytes, int endRow, int endCol)
            {
                this.FallingBytes = ParseFallingBytesInput(fallingBytes);
                this.Start = (0, 0);
                this.End = (endRow, endCol);
            }
            public string FallingByteString(int index)
            {
                return $"{FallingBytes[index].Item2},{FallingBytes[index].Item1}";
            }
            private static (int, int)[] ParseFallingBytesInput(string[] fallingBytes)
            {
                var bytes = new LinkedList<(int, int)>();
                foreach (var point in fallingBytes)
                {
                    var pointInfo = point.Split(',');
                    bytes.AddLast((int.Parse(pointInfo[1]), int.Parse(pointInfo[0])));
                }
                return bytes.ToArray();
            }
            public List<(int, int)> NeighborsInGrid((int, int) point)
            {
                return NeighborsInGrid(point.Item1, point.Item2);
            }
            public List<(int, int)> NeighborsInGrid(int row, int col)
            {
                var neighbors = new List<(int, int)>();
                foreach (var direction in Directions)
                {
                    var possibleNeighbor = (row + direction.Item1, col + direction.Item2);
                    if (IsInGrid(possibleNeighbor))
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
                return (row >= this.Start.Item1
                    && row <= this.End.Item1
                    && col >= this.Start.Item2
                    && col <= this.End.Item2);
            }
        }
        public override string PartA()
        {
            var grid = new GridInfo(_input.Lines, 70, 70);
            var locationsWithFallenBytes = new HashSet<(int, int)>(new ArraySegment<(int, int)>(grid.FallingBytes, 0, 1024));
            
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<((int, int), int)>();

            queue.Enqueue((grid.Start, 0));
            visited.Add(grid.Start);

            while (queue.Count > 0)
            {
                (var curPoint, int steps) = queue.Dequeue();
                if (curPoint == grid.End)
                {
                    return steps.ToString();
                }
                foreach (var nextPoint in grid.NeighborsInGrid(curPoint))
                {
                    if (visited.Contains(nextPoint)
                        || locationsWithFallenBytes.Contains(nextPoint))
                    {
                        continue;
                    }
                    queue.Enqueue((nextPoint, steps + 1));
                    visited.Add(nextPoint);
                }
            }
            
            return "End Not Found";
        }
        public override string PartB()
        {
            var grid = new GridInfo(_input.Lines, 70, 70);

            int fallenBytesCount = 1024;
            while (HasPathToEndAfterBytes(grid, fallenBytesCount))
            {
                fallenBytesCount++;
            }

            return grid.FallingByteString(fallenBytesCount - 1);
        }
        private bool HasPathToEndAfterBytes(GridInfo grid, int fallenBytesCount)
        {
            var locationsWithFallenBytes = new HashSet<(int, int)>(new ArraySegment<(int, int)>(grid.FallingBytes, 0, fallenBytesCount));

            var visited = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();

            queue.Enqueue(grid.Start);
            visited.Add(grid.Start);

            while (queue.Count > 0)
            {
                var curPoint = queue.Dequeue();
                if (curPoint == grid.End)
                {
                    return true;
                }
                foreach (var nextPoint in grid.NeighborsInGrid(curPoint))
                {
                    if (visited.Contains(nextPoint)
                        || locationsWithFallenBytes.Contains(nextPoint))
                    {
                        continue;
                    }
                    queue.Enqueue(nextPoint);
                    visited.Add(nextPoint);
                }
            }

            return false;
        }
    }
}