namespace AoC2024
{
    internal class Day10 : AoCSupport.Day
    {
        public override string DayNumber => "10";
        public override string Year => "2024";
        public override string PartA()
        {
            int trailheadScoreSum = 0;

            List<(int, int)> trailheadLocations = GetTrailheadLocations(_input.Lines);
            foreach (var trailheadLocation in trailheadLocations)
            {
                trailheadScoreSum += FindTrailEnds(_input.Lines, trailheadLocation.Item1, trailheadLocation.Item2).Count;
            }

            return trailheadScoreSum.ToString();
        }
        public override string PartB()
        {
            int trailheadScoreSum = 0;

            List<(int, int)> trailheadLocations = GetTrailheadLocations(_input.Lines);
            foreach (var trailheadLocation in trailheadLocations)
            {
                trailheadScoreSum += CountAllPaths(_input.Lines, trailheadLocation.Item1, trailheadLocation.Item2);
            }

            return trailheadScoreSum.ToString();
        }
        private int CountAllPaths(string[] grid, int row, int col)
        {
            var searchStack = new Stack<(int, int)>();
            int count = 0;

            searchStack.Push((row, col));

            while (searchStack.Count > 0)
            {
                var step = searchStack.Pop();

                if (grid[step.Item1][step.Item2] == '9')
                {
                    count++;
                }

                foreach (var nextStep in PossibleStepsInGrid(grid, step.Item1, step.Item2))
                {
                    searchStack.Push(nextStep);
                }
            }

            return count;
        }
        private HashSet<(int,int)> FindTrailEnds(string[] grid, int row, int col)
        {
            var searchStack = new Stack<(int, int)>();
            var visited = new HashSet<(int, int)>();
            var trailEnds = new HashSet<(int, int)>();


            searchStack.Push((row, col));

            while (searchStack.Count > 0)
            {
                var step = searchStack.Pop();
                visited.Add(step);
                
                if (grid[step.Item1][step.Item2] == '9')
                {
                    trailEnds.Add(step);
                }
                
                foreach (var nextStep in PossibleStepsInGrid(grid, step.Item1, step.Item2))
                {
                    if (!visited.Contains(nextStep))
                    {
                        searchStack.Push(nextStep);
                    }
                }
            }

            return trailEnds;
        }
        private HashSet<(int, int)> PossibleStepsInGrid(string[] grid, int row, int col)
        {
            var steps = new HashSet<(int, int)>();
            foreach (var change in new (int, int)[] { (0, 1), (-1, 0), (0, -1), (1, 0) })
            {
                var newStep = (row + change.Item1, col + change.Item2);
                if (IsInGrid(grid, newStep.Item1, newStep.Item2)
                    && grid[newStep.Item1][newStep.Item2] - grid[row][col] == 1)
                {
                    steps.Add(newStep);
                }
            }
            return steps;
        }
        private bool IsInGrid(string[] grid, int row, int col)
        {
            return (row >= 0 && col >= 0
                && row < grid.Length
                && col < grid[0].Length);
        }
        private List<(int, int)> GetTrailheadLocations(string[] grid)
        {
            var locations = new List<(int, int)>();
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '0')
                    {
                        locations.Add((i, j));
                    }
                }
            }
            return locations;
        }
    }
}