namespace AoC2024
{
    internal class Day13 : AoCSupport.Day
    {
        public override string DayNumber => "13";
        public override string Year => "2024";
        private struct ClawMachine
        {
            public double A_x;
            public double A_y;
            public double B_x;
            public double B_y;
            public double P_x;
            public double P_y;
            public ClawMachine(double A_x, double A_y, double B_x, double B_y, double P_x, double P_y) {
                this.A_x = A_x;
                this.A_y = A_y;
                this.B_x = B_x;
                this.B_y = B_y;
                this.P_x = P_x;
                this.P_y = P_y;
            }
        }
        public override string PartA()
        {
            ClawMachine[] clawSystems = ParseClawMachines(_input.Lines);
            int tokenCost = 0;

            foreach (var clawMachine in clawSystems)
            {
                (double A, double B) = SolveSystem(clawMachine);
                if (Math.Abs(A - double.Round(A)) < 0.01
                    && Math.Abs(B - double.Round(B)) < 0.01)
                {
                    tokenCost += ((int) double.Round(A)) * 3 + ((int) double.Round(B));
                }
            }

            return tokenCost.ToString();
        }
        public override string PartB()
        {
            ClawMachine[] clawSystems = ParseClawMachines(_input.Lines);
            double tokenCost = 0;

            foreach (var clawMachine in clawSystems)
            {
                var updatedClawMachine = new ClawMachine(
                    clawMachine.A_x,
                    clawMachine.A_y,
                    clawMachine.B_x,
                    clawMachine.B_y,
                    10000000000000 + clawMachine.P_x,
                    10000000000000 + clawMachine.P_y
                    );
                (double A, double B) = SolveSystem(updatedClawMachine);
                if (Math.Abs(A - double.Round(A)) < 0.01
                    && Math.Abs(B - double.Round(B)) < 0.01)
                {
                    tokenCost += double.Round(A) * 3 + double.Round(B);
                }
            }

            return tokenCost.ToString();
        }
        private (double, double) SolveSystem(ClawMachine claw)
        {
            // Ax = b => x = inv(A)*b
            // A = [A_x B_x; A_y B_y]; b = [P_x; P_y]
            double A_determinant = claw.A_x * claw.B_y - claw.A_y * claw.B_x;
            double[,] inverse =
            {
                { claw.B_y / A_determinant, -claw.B_x / A_determinant },
                { -claw.A_y / A_determinant, claw.A_x / A_determinant }
            };
            
            double b_1 = inverse[0,0] * claw.P_x + inverse[0,1] * claw.P_y;
            double b_2 = inverse[1,0] * claw.P_x + inverse[1,1] * claw.P_y;
            
            return (b_1, b_2);
        }
        private ClawMachine[] ParseClawMachines(string[] input)
        {
            ClawMachine[] clawSystems = new ClawMachine[(input.Length+1)/4];
            int index = 0;
            double A_x = 0;
            double A_y = 0;
            double B_x = 0;
            double B_y = 0;
            while (index < input.Length)
            {
                string[] splitInput = input[index].Split(", ");
                if (index % 4 == 0)
                {
                    A_x = int.Parse(splitInput[0].Split("+")[1]);
                    A_y = int.Parse(splitInput[1].Split("+")[1]);
                }
                else if (index % 4 == 1)
                {
                    B_x = int.Parse(splitInput[0].Split("+")[1]);
                    B_y = int.Parse(splitInput[1].Split("+")[1]);
                }
                else if (index % 4 == 2)
                {
                    double P_x = int.Parse(splitInput[0].Split("=")[1]);
                    double P_y = int.Parse(splitInput[1].Split("=")[1]);
                    clawSystems[(index - 2) / 4] = new ClawMachine(A_x, A_y, B_x, B_y, P_x, P_y);
                }
                index++;
            }
            return clawSystems;
        }
    }
}