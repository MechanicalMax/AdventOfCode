namespace AoC2024
{
    internal class Day12 : AoCSupport.Day
    {
        public override string DayNumber => "12";
        public override string Year => "2024";
        internal class Region
        {
            public char Plant { get; init; }
            public int Area { get; set; }
            public int Perimeter { get; set; }
            public HashSet<(int, int)> Points { get; set; }
            public Region(char plant)
            {
                Plant = plant;
                Area = 0;
                Perimeter = 0;
                Points = new HashSet<(int, int)>();
            }
        }
        public override string PartA()
        {
            int price = 0;

            var regions = GetRegionsFromGrid(_input.Lines);
            price = CalculateTotalFencePrice(regions);

            return price.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        public List<Region> GetRegionsFromGrid(string[] grid)
        {
            var regions = new List<Region>();
            var seenPoints = new HashSet<(int, int)>();
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (seenPoints.Contains((row, col)))
                    {
                        continue;
                    }
                    var region = ExploreRegion(grid, row, col);
                    seenPoints.UnionWith(region.Points);
                    regions.Add(region);
                }
            }
            return regions;
        }
        public int CalculateTotalFencePrice(List<Region> regions)
        {
            int price = 0;
            foreach (var region in regions)
            {
                price += region.Area * region.Perimeter;
            }
            return price;
        }
        public Region ExploreRegion(string[] grid, int row, int col)
        {
            var region = new Region(grid[row][col]);
            var searchPoints = new HashSet<(int, int)>() { (row, col) };

            while (searchPoints.Count > 0)
            {
                region.Points.UnionWith(searchPoints);
                var nextSearch = new HashSet<(int, int)>();
                foreach (var point in searchPoints)
                {
                    var neighbors = PointPlantNeighbors(grid, point.Item1, point.Item2);
                    int additionalPerimeter = 4;

                    foreach (var neighbor in neighbors)
                    {
                        if (region.Points.Contains(neighbor))
                        {
                            additionalPerimeter -= 2;
                        }
                        else
                        {
                            nextSearch.Add(neighbor);
                        }
                    }

                    region.Area += 1;
                    region.Perimeter += additionalPerimeter;
                }
                searchPoints = nextSearch;
            }

            return region;
        }
        public HashSet<(int, int)> PointPlantNeighbors(string[] grid, int row, int col)
        {
            var possibleNeighbors = new HashSet<(int, int)>();
            foreach (var change in new (int, int)[] {(1, 0), (0, 1), (-1, 0), (0, -1)} )
            {
                int testRow = row + change.Item1;
                int testCol = col + change.Item2;
                if (IsInGrid(grid, testRow, testCol) && grid[testRow][testCol] == grid[row][col])
                {
                    possibleNeighbors.Add((testRow, testCol));
                }
            }
            return possibleNeighbors;
        }
        public bool IsInGrid(string[] grid, int row, int col)
        {
            return (row >= 0 && col >= 0
                && grid.Length > row
                && grid[row].Length > col);
        }
    }
}