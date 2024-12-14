namespace AoC2024
{
    internal class Day14 : AoCSupport.Day
    {
        public override string DayNumber => "14";
        public override string Year => "2024";
        private struct Robot
        {
            public double Px;
            public double Py;
            public double Vx;
            public double Vy;
        }
        public override string PartA()
        {
            var robots = ParseRobots(_input.Lines);

            robots = RobotsAfterSeconds(robots, 100);

            return SafetyFactor(robots).ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private HashSet<Robot> RobotsAfterSeconds(HashSet<Robot> robots, double seconds)
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

                robot.Px = double.Parse(pos[0]);
                robot.Py = double.Parse(pos[1]);
                robot.Vx = double.Parse(vel[0]);
                robot.Vy = double.Parse(vel[1]);

                robots.Add(robot);
            }
            return robots;
        }
    }
}