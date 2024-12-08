namespace AoC2024
{
    internal class Day8 : AoCSupport.Day
    {
        public override string DayNumber => "8";
        public override string Year => "2024";
        public override string PartA()
        {
            var uniqueAntinodes = new HashSet<(int, int)>();

            for (int row = 0; row < _input.Lines.Length; row++)
            {
                for (int col = 0; col < _input.Lines[row].Length; col++)
                {
                    if (_input.Lines[row][col] != '.')
                    {
                        uniqueAntinodes.UnionWith(getAntinodeLocations((row, col), _input.Lines));
                    }
                }
            }

            return uniqueAntinodes.Count.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private HashSet<(int, int)> getAntinodeLocations((int, int) location, string[] grid)
        {
            var antinodes = new HashSet<(int, int)>();
            char antennaType = grid[location.Item1][location.Item2];

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (grid[row][col] == antennaType)
                    {
                        if (row == location.Item1 && col == location.Item2)
                        {
                            continue;
                        }

                        int rowDiff = row - location.Item1;
                        int colDiff = col - location.Item2;

                        var pointOne = (row + rowDiff, col + colDiff);
                        var pointTwo = (location.Item1 - rowDiff, location.Item2 - colDiff);

                        if (IsInGrid(pointOne, grid))
                        {
                            antinodes.Add(pointOne);
                        }
                        if (IsInGrid(pointTwo, grid))
                        {
                            antinodes.Add(pointTwo);
                        }
                    }
                }
            }

            return antinodes;
        }
        private bool IsInGrid((int, int) point, string[] grid)
        {
            return (point.Item1 >=0 && point.Item2 >=0
                && point.Item1 < grid.Length
                && point.Item2 < grid[0].Length);
        }
    }
}