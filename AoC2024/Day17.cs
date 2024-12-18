namespace AoC2024
{
    internal class Day17 : AoCSupport.Day
    {
        /*
         * Part B: unable to solve due to time constraints
         * Found solution from alexandrajay2002
         * Reimplemented their part2 python solution in C# below
         * https://pastebin.com/34vpnE8f
         * https://tio.run/##lVbbbtswDH33V7AoBtuIF@TWoTCSAtuAPQ7DXlPXUGwlERZLhiRv6ddnlC3fs6w1kDiiyEMekqKSv@qj4MvLZS9FBupVActyITUQefjtOE5K95ATqWgs6YEpTaV3Ypz6oQP4MJ7SM2zAiKZ7XHlu6PowgUW5TXka1yrVewLzcufPkZ1oZ3@NGJRX0EB4WiJuy72w0YqmTKXswLRn3fd9TDYIXsol1YXk6FJ7V3H8HrFcioMk2Tt4WfytcXD2YS8knNGmG3Q0VfkJI3UD14@67jwlE@slDsD6Rlcork2e@TN3A5j7pVar0g/Xvv1uRFZm/R2obqtGAtgFkAQg8kSkDdG9XZsKLEOAeyBKFRkFfaT1FlOG3E4UPFVN3q3HSqWq9qlF22xgFQ51yTW1h5HazqopOtpKLDNNlTaMLN/AdAEyK3RVYEtuhzlL8DOr8tj@rLoPBWtj2HphXGlZJJoJHjQh1k62eRLmyWQRter7roUhM2uxzEPQmsDT0@1KOG0vn8aQ8z7kDl42ddJv2i2GdpvbUcAHeLyNuOwjogKBuxHnJtWdzug@ieCa8eI/0a@usE5umzz0TbAZ8kK/ifWAlbW8ayvfNFY05mo78xvBfh0GMBpM/4r907hY7@ub7lkps/xOBKwYRrnojc@WAEaI58SevYww3gyfsJ7zUA3wZiiV8nuYmQBUOUwYZ5qRE4rMfOcCMiGpnbbVUKNKIczWmwU9LPiIozDqHFyz2bHx4anbhEgyRbqEJ7S8dhq9aS5yb@Y3imZsJ8eC/zLTTRJ@oN6j308jp2cdm1PsEVivYWmugNLEGTYNF7qaSpXJcDTVEfnhGw6EhWxZXD1ktkqVv4FxhzTJc4zAa@JqUE1W/V7BvwuOUTjoOo45yWgcG9duHJuSx7Eb2vvIXHtVF5QXmvmnsJ1HPqJdLpefttngcwiL2WLlNIIvIY7fZvXVrJwfVZbwd7AMHoIVfs/@Ag
         */
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
            (_, _, _, int[] instructions) = ParseDebuggerInput(_input.Lines);

            var nextValidNumbers = new Queue<(long, int)>();
            nextValidNumbers.Enqueue((0, instructions.Length - 1));

            while (nextValidNumbers.Count > 0)
            {
                (long currentRegisterA, int distance) = nextValidNumbers.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    long nextRegisterA = (currentRegisterA << 3) + i;

                    (bool correctOutput, int outputLength) = ProvidesCorrectOutput(instructions, distance, nextRegisterA, 0, 0);

                    if (!correctOutput)
                    {
                        continue;
                    }

                    if (distance == 0)
                    {
                        return nextRegisterA.ToString();
                    }

                    nextValidNumbers.Enqueue((nextRegisterA, distance - 1));
                }
            }
            return "Not Found";
        }
        private (bool, int) ProvidesCorrectOutput(int[] instructions, int distance, long registerA, long registerB, long registerC)
        {
            int outputIndex = distance;
            bool hadBadOutput = false;

            int instructionPointer = 0;

            while (!hadBadOutput && instructionPointer < instructions.Length)
            {
                int instruction = instructions[instructionPointer];
                int operand = instructions[instructionPointer + 1];

                switch (instruction)
                {
                    case 0:
                        registerA = registerA / (int)Math.Pow(2, GetCombo(operand, registerA, registerB, registerC));
                        break;
                    case 1:
                        registerB ^= (long)operand;
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
                        long newOutput = GetCombo(operand, registerA, registerB, registerC) % 8;
                        if (outputIndex >= instructions.Length || instructions[outputIndex] != newOutput)
                        {
                            hadBadOutput = true;
                        }
                        else
                        {
                            outputIndex++;
                        }
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

            return (!hadBadOutput, outputIndex);
        }
        private long GetCombo(int instruction, long registerA, long registerB, long registerC)
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