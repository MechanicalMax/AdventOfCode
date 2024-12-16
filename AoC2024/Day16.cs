﻿namespace AoC2024
{
    internal class Day16 : AoCSupport.Day
    {
        public override string DayNumber => "16";
        public override string Year => "2024";
        private class GridInfo
        {
            public char[,] Grid;
            public (int, int) StartLocation;
            public (int, int) EndLocation;
            public (int, int)[] Directions = { (0, 1), (-1, 0), (0, -1), (1, 0) };
            public int StartDirection = 0;
            public GridInfo(string[] input)
            {
                Grid = new char[input.Length, input[0].Length];
                for (int row = 0; row < input.Length; row++)
                {
                    for (int col = 0; col < input[row].Length; col++)
                    {
                        char point = input[row][col];
                        if (point == 'S')
                        {
                            StartLocation = (row, col);
                        }
                        else if (point == 'E')
                        {
                            EndLocation = (row, col);
                        }
                        Grid[row, col] = point;
                    }
                }
            }
            public HashSet<(Transform, int)> GetPossibleMoves(Transform transform)
            {
                var moves = new HashSet<(Transform, int)>();

                moves.Add((new Transform(transform.Row, transform.Col, (transform.Direction + 1) % 4), 1000));
                moves.Add((new Transform(transform.Row, transform.Col, (transform.Direction + 3) % 4), 1000));
                int stepRow = transform.Row + Directions[transform.Direction].Item1;
                int stepCol = transform.Col + Directions[transform.Direction].Item2;
                if (Grid[stepRow, stepCol] != '#')
                {
                    moves.Add((new Transform(stepRow, stepCol, transform.Direction), 1));
                }

                return moves;
            }
            public bool IsFacingWall(Transform transform)
            {
                int stepRow = transform.Row + Directions[transform.Direction].Item1;
                int stepCol = transform.Col + Directions[transform.Direction].Item2;
                return (Grid[stepRow, stepCol] == '#');
            }
            public bool IsAtEnd(Transform transform)
            {
                return (transform.Row == EndLocation.Item1 && transform.Col == EndLocation.Item2);
            }
        }
        private struct Transform
        {
            public int Row { get; init; }
            public int Col { get; init; }
            public int Direction { get; init; }
            public Transform (int row, int col, int direction)
            {
                Row = row; Col = col; Direction = direction;
            }
        }
        public override string PartA()
        {
            var gridInfo = new GridInfo(_input.Lines);// "#################\r\n#...#...#...#..E#\r\n#.#.#.#.#.#.#.#.#\r\n#.#.#.#...#...#.#\r\n#.#.#.#.###.#.#.#\r\n#...#.#.#.....#.#\r\n#.#.#.#.#.#####.#\r\n#.#...#.#.#.....#\r\n#.#.#####.#.###.#\r\n#.#.#.......#...#\r\n#.#.###.#####.###\r\n#.#.#...#.....#.#\r\n#.#.#.#####.###.#\r\n#.#.#.........#.#\r\n#.#.#.#########.#\r\n#S#.............#\r\n#################".Trim().Split("\n"));//_input.Lines);

            var unvisitedTransforms = new PriorityQueue<(Transform, int), int>();
            var visitedTransforms = new HashSet<Transform>();

            var curTransform = new Transform(
                gridInfo.StartLocation.Item1,
                gridInfo.StartLocation.Item2,
                gridInfo.StartDirection
                );

            unvisitedTransforms.Enqueue((curTransform, 0), 0);

            int smallestCost = int.MaxValue;

            while (unvisitedTransforms.Count > 0)
            {
                (curTransform, int cost) = unvisitedTransforms.Dequeue();
                if (gridInfo.IsAtEnd(curTransform))
                {
                    smallestCost = cost;
                    break;
                }

                foreach ((Transform newTransform, int additionalCost) in gridInfo.GetPossibleMoves(curTransform))
                {
                    if (gridInfo.IsFacingWall(newTransform)
                        && additionalCost == 1000)
                    {
                        continue;
                    }

                    if (!visitedTransforms.Contains(newTransform))
                    {
                        visitedTransforms.Add(newTransform);
                        unvisitedTransforms.Enqueue((newTransform, cost + additionalCost), cost + additionalCost);
                    }
                }
            }

            return smallestCost.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
    }
}