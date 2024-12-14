namespace AoC2024
{
    internal class Day14 : AoCSupport.Day
    {
        public override string DayNumber => "14";
        public override string Year => "2024";
        private struct Robot
        {
            public int Px;
            public int Py;
            public int Vx;
            public int Vy;
        }
        public override string PartA()
        {
            var robots = ParseRobots(_input.Lines);

            robots = RobotsAfterSeconds(robots, 100);

            return SafetyFactor(robots).ToString();
        }
        public override string PartB()
        {
            /*
             * Strategy: Look for patterns by printing out the grid
             * - Found a horizontal and vertical rectangle where robots were more densely
             *   packed than at other times
             * - These horizontal and vertical patterns repeated with a constant interval
             *   (103 and 101, respectively)
             * - Find where the lines appear at the same time -> 7286
             * (Used graphing calculator to save time rather than programming a solution)
             */
            var robots = ParseRobots(_input.Lines);

            string gridPictures = "";
            gridPictures += GridStringAtTime(RobotsAfterSeconds(robots, 14), 14);
            gridPictures += GridStringAtTime(RobotsAfterSeconds(robots, 76), 76);
            gridPictures += GridStringAtTime(RobotsAfterSeconds(robots, 115), 115);
            gridPictures += GridStringAtTime(RobotsAfterSeconds(robots, 179), 179);

            gridPictures += GridStringAtTime(RobotsAfterSeconds(robots, 7286), 7286);

            return gridPictures;
        }
        private string GridStringAtTime(HashSet<Robot> robots, int time)
        {
            string gridString = $"Time = {time} seconds\n";
            for (int y = 0; y < 103; y++)
            {
                for (int x = 0; x < 101; x++)
                {
                    bool containsRobot = false;
                    foreach (var robot in robots)
                    {
                        if (robot.Px == x && robot.Py == y)
                        {
                            containsRobot = true;
                            break;
                        }
                    }
                    gridString += containsRobot ? "#" : ".";
                }
                gridString += "\n";
            }
            
            gridString += "\n\n";
            
            return gridString;
        }
        private HashSet<Robot> RobotsAfterSeconds(HashSet<Robot> robots, int seconds)
        {
            var newRobots = new HashSet<Robot>();

            foreach (var robot in robots)
            {
                var newRobot = new Robot();
                newRobot.Px += ((101 + robot.Vx) * seconds + robot.Px) % 101;
                newRobot.Py += ((103 + robot.Vy) * seconds + robot.Py) % 103;
                newRobot.Vx = robot.Vx;
                newRobot.Vy = robot.Vy;
                newRobots.Add(newRobot);
            }

            return newRobots;
        }
        private int SafetyFactor(HashSet<Robot> robots)
        {
            int[] robotsInQuadrant = new int[4];

            foreach (var robot in robots)
            {
                if (robot.Px < 50)
                {
                    if (robot.Py < 51)
                    {
                        robotsInQuadrant[0]++;
                    }
                    else if (robot.Py > 51)
                    {
                        robotsInQuadrant[1]++;
                    }
                }
                else if (robot.Px > 50)
                {
                    if (robot.Py < 51)
                    {
                        robotsInQuadrant[2]++;
                    }
                    else if (robot.Py > 51)
                    {
                        robotsInQuadrant[3]++;
                    }
                }
            }

            return robotsInQuadrant.Aggregate((int a, int b) => a * b);
        }
        private HashSet<Robot> ParseRobots(string[] lines)
        {
            var robots = new HashSet<Robot>();
            foreach (var line in lines)
            {
                var robot = new Robot();
                
                string[] posAndVel = line.Split(' ');
                string[] pos = posAndVel[0].TrimStart(['p', '=']).Split(',');
                string[] vel = posAndVel[1].TrimStart(['v', '=']).Split(',');

                robot.Px = int.Parse(pos[0]);
                robot.Py = int.Parse(pos[1]);
                robot.Vx = int.Parse(vel[0]);
                robot.Vy = int.Parse(vel[1]);

                robots.Add(robot);
            }
            return robots;
        }
    }
}