namespace AoC2024
{
    internal class Day12 : AoCSupport.Day
    {
        public override string DayNumber => "12";
        public override string Year => "2024";
        private class Region
        {
            public char Plant { get; init; }
            public int Area { get; set; }
            public int Perimeter { get; set; }
            public int Sides { get; set; }
            public HashSet<(int, int)> Points { get; set; }
            public Region(char plant)
            {
                Plant = plant;
                Area = 0;
                Perimeter = 0;
                Sides = 0;
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
            int price = 0;

            var regions = GetRegionsFromGrid(_input.Lines);
            price = CalculateBulkFencePrice(regions);

            return price.ToString();
        }
        private List<Region> GetRegionsFromGrid(string[] grid)
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
        private int CalculateTotalFencePrice(List<Region> regions)
        {
            int price = 0;
            foreach (var region in regions)
            {
                price += region.Area * region.Perimeter;
            }
            return price;
        }
        private int CalculateBulkFencePrice(List<Region> regions)
        {
            int price = 0;
            foreach (var region in regions)
            {
                price += region.Area * region.Sides;
            }
            return price;
        }
        private Region ExploreRegion(string[] grid, int row, int col)
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

            region.Sides = CountRegionSides(region);

            return region;
        }
        private HashSet<(int, int)> PointPlantNeighbors(string[] grid, int row, int col)
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
        private bool IsInGrid(string[] grid, int row, int col)
        {
            return (row >= 0 && col >= 0
                && grid.Length > row
                && grid[row].Length > col);
        }
        private int CountRegionSides(Region region)
        {
            int numSides = 0;

            (var rowBounds, var colBounds) = FindRegionBounds(region);

            // Row Sides
            for (int row = rowBounds.Item1; row <= rowBounds.Item2 + 1; row++)
            {
                bool AboveWasIn = false;
                bool LastWasEdge = false;
                for (int col = colBounds.Item1; col <= colBounds.Item2; col++)
                {
                    var pointAbove = (row - 1, col);
                    var point = (row, col);
                    if ((region.Points.Contains(point) && region.Points.Contains(pointAbove))
                        || (!region.Points.Contains(point) && !region.Points.Contains(pointAbove)))
                    {
                        LastWasEdge = false;
                        continue;
                    }

                    if (region.Points.Contains(point) && !region.Points.Contains(pointAbove))
                    {
                        if (!LastWasEdge || (LastWasEdge && AboveWasIn))
                        {
                            numSides++;
                            LastWasEdge = true;
                            AboveWasIn = false;
                        }
                    }
                    else if (!region.Points.Contains(point) && region.Points.Contains(pointAbove))
                    {
                        if (!LastWasEdge || (LastWasEdge && !AboveWasIn))
                        {
                            numSides++;
                            LastWasEdge = true;
                            AboveWasIn = true;
                        }
                    }
                }
            }

            // Col Sides
            for (int col = colBounds.Item1; col <= colBounds.Item2 + 1; col++)
            {
                bool LeftWasIn = false;
                bool LastWasEdge = false;
                for (int row = rowBounds.Item1; row <= rowBounds.Item2; row++)
                {
                    var pointLeft = (row, col - 1);
                    var point = (row, col);
                    if ((region.Points.Contains(point) && region.Points.Contains(pointLeft))
                        || (!region.Points.Contains(point) && !region.Points.Contains(pointLeft)))
                    {
                        LastWasEdge = false;
                        continue;
                    }

                    if (region.Points.Contains(point) && !region.Points.Contains(pointLeft))
                    {
                        if (!LastWasEdge || (LastWasEdge && LeftWasIn))
                        {
                            numSides++;
                            LastWasEdge = true;
                            LeftWasIn = false;
                        }
                    }
                    else if (!region.Points.Contains(point) && region.Points.Contains(pointLeft))
                    {
                        if (!LastWasEdge || (LastWasEdge && !LeftWasIn))
                        {
                            numSides++;
                            LastWasEdge = true;
                            LeftWasIn = true;
                        }
                    }
                }
            }

            return numSides;
        }
        private ((int, int), (int, int)) FindRegionBounds(Region region)
        {
            int maxRow = -1;
            int minRow = int.MaxValue;
            int maxCol = -1;
            int minCol = int.MaxValue;

            foreach (var point in region.Points)
            {
                maxRow = int.Max(maxRow, point.Item1);
                minRow = int.Min(minRow, point.Item1);
                maxCol = int.Max(maxCol, point.Item2);
                minCol = int.Min(minCol, point.Item2);
            }

            return ((minRow, maxRow), (minCol, maxCol));
        }
    }
}