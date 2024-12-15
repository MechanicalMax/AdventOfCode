namespace AoC2024
{
    internal class Day15 : AoCSupport.Day
    {
        public override string DayNumber => "15";
        public override string Year => "2024";
        public override string PartA()
        {
            (char[,] grid, string[] instructions) = ParseGridAndInstructions(_input.Raw);//"##########\n#..O..O.O#\n#......O.#\n#.OO..O.O#\n#..O@..O.#\n#O#..O...#\n#O..O..O.#\n#.OO.O.OO#\n#....O...#\n##########\n\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^");//_input.Raw);
            string singleInstructions = string.Join("", instructions);

            grid = FollowInstructions(grid, singleInstructions);

            return SumGPSCoordinates(grid).ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private char[,] FollowInstructions(char[,] grid, string instructions)
        {
            (int robotx, int roboty) = GetRobotPosition(grid);
            foreach (var instruction in instructions)
            {
                //WriteGrid(grid);
                (int changex, int changey) = ArrowToDirectionVector(instruction);
                int nextx = robotx + changex;
                int nexty = roboty + changey;
                
                if (grid[nextx, nexty] == '.')
                {
                    grid[robotx, roboty] = '.';
                    grid[nextx, nexty] = '@';
                    robotx += changex;
                    roboty += changey;
                }
                else if (grid[nextx, nexty] == 'O')
                {
                    bool foundAvailable = false;
                    int availablex = nextx + changex;
                    int availabley = nexty + changey;
                    while (IsInGrid(grid, availablex, availabley)
                        && grid[availablex, availabley] != '#')
                    {
                        if (grid[availablex, availabley] == '.')
                        {
                            foundAvailable = true;
                            break;
                        }
                        availablex += changex;
                        availabley += changey;
                    }
                    if (foundAvailable)
                    {
                        grid[availablex, availabley] = 'O';
                        grid[nextx, nexty] = '@';
                        grid[robotx, roboty] = '.';
                        robotx += changex;
                        roboty += changey;
                    }
                }
            }
            //WriteGrid(grid);
            return grid;
        }
        private (int, int) GetRobotPosition(char[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y] == '@')
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        private int SumGPSCoordinates(char[,] grid)
        {
            int sum = 0;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x,y] == 'O')
                    {
                        sum += 100 * y + x;
                    }
                }
            }

            return sum;
        }
        private (int, int) ArrowToDirectionVector(char arrow)
        {
            switch (arrow)
            {
                case '^':
                    return (0, -1);
                case '>':
                    return (1, 0);
                case 'v':
                    return (0, 1);
                case '<':
                    return (-1, 0);
                default:
                    return (0, 0);
            }
        }
        private (char[,], string[]) ParseGridAndInstructions(string input)
        {
            input = input.Trim();
            string[] inputSections = input.Split("\n\n");

            var gridLines = inputSections[0].Split("\n");
            var grid = new char[gridLines[0].Length,gridLines.Length];
            for (int y = 0; y < gridLines.Length; y++)
            {
                int x = 0;
                while (x < gridLines[y].Length)
                {
                    grid[x,y] = gridLines[y][x++];
                }
            }

            var instructions = inputSections[1].Split("\n");

            return (grid,  instructions);
        }
        private bool IsInGrid(char[,] grid, int x, int y)
        {
            return (x >=0 && y >= 0
                && x < grid.GetLength(0)
                && y < grid.GetLength(1));
        }
        private void WriteGrid(char[,] grid)
        {
            Console.Clear();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x,y]);
                }
                Console.WriteLine();
            }
        }
    }
}