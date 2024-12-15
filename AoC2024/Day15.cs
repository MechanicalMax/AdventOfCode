namespace AoC2024
{
    internal class Day15 : AoCSupport.Day
    {
        public override string DayNumber => "15";
        public override string Year => "2024";
        public override string PartA()
        {
            (char[,] grid, string[] instructions) = ParseGridAndInstructions(_input.Raw);
            string singleInstructions = string.Join("", instructions);

            grid = FollowInstructions(grid, singleInstructions);

            return SumGPSCoordinates(grid).ToString();
        }
        public override string PartB()
        {
            (char[,] grid, string[] instructions) = ParseGridAndInstructions(_input.Raw);
            string singleInstructions = string.Join("", instructions);
            grid = ExpandGrid(grid);

            grid = ExpandedGridFollowInstructions(grid, singleInstructions);

            return SumGPSCoordinates(grid).ToString();
        }
        private char[,] ExpandedGridFollowInstructions(char[,] grid, string instructions)
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
                else if (grid[nextx, nexty] == '[' || grid[nextx, nexty] == ']')
                {
                    if (changey == 0)
                    {
                        bool foundAvailable = false;
                        int availablex = nextx + changex;
                        while (grid[availablex, roboty] != '#')
                        {
                            if (grid[availablex, roboty] == '.')
                            {
                                foundAvailable = true;
                                break;
                            }
                            availablex += changex;
                        }
                        if (foundAvailable)
                        {
                            while (availablex != robotx)
                            {
                                grid[availablex, roboty] = grid[availablex - changex, roboty];
                                availablex -= changex;
                            }
                            grid[robotx, roboty] = '.';
                            robotx += changex;
                        }
                    }
                    else
                    {
                        int boxLeftx = nextx;
                        int boxy = nexty;
                        if (grid[nextx, nexty] == ']')
                        {
                            boxLeftx--;
                        }
                        var boxes = BoxesInMovableFormation(grid, boxLeftx, boxy, changey);
                        if (boxes.Count != 0)
                        {
                            while (boxes.Count > 0)
                            {
                                (int curBoxLeftx, int curBoxy) = boxes.Pop();
                                grid[curBoxLeftx, curBoxy] = '.';
                                grid[curBoxLeftx + 1, curBoxy] = '.';
                                grid[curBoxLeftx, curBoxy + changey] = '[';
                                grid[curBoxLeftx + 1, curBoxy + changey] = ']';
                            }
                            grid[robotx, roboty] = '.';
                            roboty += changey;
                            grid[robotx, roboty] = '@';
                        }
                    }
                }
            }
            //WriteGrid(grid);
            return grid;
        }
        private Stack<(int, int)> BoxesInMovableFormation(char[,] grid, int boxLeftx, int boxy, int changey)
        {
            var moveableBoxes = new Stack<(int, int)>();

            var nextBoxes = new Queue<(int, int)>();
            nextBoxes.Enqueue((boxLeftx, boxy));

            while (nextBoxes.Count != 0)
            {
                (int curBoxLeftx, int curBoxy) = nextBoxes.Dequeue();
                int nexty = curBoxy + changey;
                
                if (grid[curBoxLeftx,nexty] == '#' || grid[curBoxLeftx + 1,nexty] == '#')
                {
                    return new Stack<(int, int)>();
                }

                moveableBoxes.Push((curBoxLeftx, curBoxy));
                if (grid[curBoxLeftx,nexty] == '.' && grid[curBoxLeftx + 1,nexty] == '.')
                {
                    continue;
                }

                if (grid[curBoxLeftx, nexty] == '[')
                {
                    nextBoxes.Enqueue((curBoxLeftx, nexty));
                    continue;
                }

                if (grid[curBoxLeftx - 1,nexty] == '[')
                {
                    nextBoxes.Enqueue((curBoxLeftx - 1, nexty));
                }

                if (grid[curBoxLeftx + 1, nexty] == '[')
                {
                    nextBoxes.Enqueue((curBoxLeftx + 1, nexty));
                }

            }

            return moveableBoxes;
        }
        private char[,] ExpandGrid(char[,] grid)
        {
            var newGrid = new char[grid.GetLength(0) * 2, grid.GetLength(1)];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    switch (grid[x,y])
                    {
                        case '#':
                            newGrid[x * 2, y] = '#';
                            newGrid[x * 2 + 1, y] = '#';
                            break;
                        case 'O':
                            newGrid[x * 2, y] = '[';
                            newGrid[x * 2 + 1, y] = ']';
                            break;
                        case '.':
                            newGrid[x * 2, y] = '.';
                            newGrid[x * 2 + 1, y] = '.';
                            break;
                        case '@':
                            newGrid[x * 2, y] = '@';
                            newGrid[x * 2 + 1, y] = '.';
                            break;
                        default:
                            newGrid[x * 2, y] = '?';
                            newGrid[x * 2 + 1, y] = '?';
                            break;
                    }
                }
            }
            return newGrid;
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
                    if (grid[x,y] == 'O' || grid[x, y] == '[')
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