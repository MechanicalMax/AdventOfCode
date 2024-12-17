using Microsoft.Win32;

namespace AoC2024
{
    internal class Day17 : AoCSupport.Day
    {
        public override string DayNumber => "17";
        public override string Year => "2024";
        public override string PartA()
        {
            (int registerA, int registerB, int registerC, int[] instructions) = ParseDebuggerInput(_input.Lines);//(729, 0, 0, [0,1,5,4,3,0]);
            var programOutput = new List<int>();

            int instructionPointer = 0;

            while (instructionPointer < instructions.Length)
            {
                int instruction = instructions[instructionPointer];
                int operand = instructions[instructionPointer + 1];

                switch (instruction)
                {
                    case 0:
                        registerA = registerA / (int) Math.Pow(2, GetCombo(operand, registerA, registerB, registerC));
                        break;
                    case 1:
                        registerB ^= operand;
                        break;
                    case 2:
                        registerB = GetCombo(operand, registerA, registerB, registerC) % 8;
                        break;
                    case 3:
                        if (registerA != 0)
                        {
                            instructionPointer = operand - 2;
                        }
                        break;
                    case 4:
                        registerB ^= registerC;
                        break;
                    case 5:
                        programOutput.Add(GetCombo(operand, registerA, registerB, registerC) % 8);
                        break;
                    case 6:
                        registerB = registerA / (int)Math.Pow(2, GetCombo(operand, registerA, registerB, registerC));
                        break;
                    case 7:
                        registerC = registerA / (int)Math.Pow(2, GetCombo(operand, registerA, registerB, registerC));
                        break;
                }
                instructionPointer += 2;
            }


            return string.Join(',', programOutput);
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private int GetCombo(int instruction, int registerA, int registerB, int registerC)
        {
            switch (instruction)
            {
                case 4:
                    return registerA;
                case 5:
                    return registerB;
                case 6:
                    return registerC;
                case 7:
                    throw new Exception("7 is Reserved, program not valid");
                default:
                    return instruction;
            }
        }
        private (int, int, int, int[]) ParseDebuggerInput(string[] lines)
        {
            int registerA = int.Parse(lines[0].Substring(12));
            int registerB = int.Parse(lines[1].Substring(12));
            int registerC = int.Parse(lines[2].Substring(12));
            int[] instructions = Array.ConvertAll(lines[4].Substring(9).Split(','), int.Parse);
            return (registerA, registerB, registerC, instructions);
        }
    }
}