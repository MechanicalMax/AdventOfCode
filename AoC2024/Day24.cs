namespace AoC2024
{
    internal class Day24 : AoCSupport.Day
    {
        public override string DayNumber => "24";
        public override string Year => "2024";
        private struct Gate
        {
            public string InputOne { get; init; }
            public string InputTwo { get; init; }
            public string Operation { get; init; }
            public string Output { get; init; }
            public Gate(string inputOne, string inputTwo, string operation, string output)
            {
                InputOne = inputOne;
                InputTwo = inputTwo;
                Operation = operation;
                Output = output;
            }
        }
        public override string PartA()
        {
            (var initialWireValues, var gateConnections) = ReadInitialGateInfo(_input.Lines);

            string curOutput = "z00";
            int curIteration = 1;
            var finalWireValues = initialWireValues;
            while (gateConnections.ContainsKey(curOutput))
            {
                (_, finalWireValues) = DetermineFinalWireValue(curOutput, finalWireValues, gateConnections);

                curOutput = $"z{curIteration:D2}";
                curIteration++;
            }

            curIteration -= 2;
            long output = 0;
            while (curIteration >= 0)
            {
                output <<= 1;
                output += finalWireValues[$"z{curIteration:D2}"] ? 1 : 0;
                curIteration--;
            }

            return output.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private (bool, Dictionary<string, bool>) DetermineFinalWireValue(string wire, Dictionary<string, bool> finalWireValues, Dictionary<string, Gate> gateConnections)
        {
            if (finalWireValues.TryGetValue(wire, out bool value))
            {
                return (value, finalWireValues);
            }
            var connectedGate = gateConnections[wire];
            (var inputOneValue, finalWireValues) = DetermineFinalWireValue(connectedGate.InputOne, finalWireValues, gateConnections);
            (var inputTwoValue, finalWireValues) = DetermineFinalWireValue(connectedGate.InputTwo, finalWireValues, gateConnections);
            bool result = false;
            switch (connectedGate.Operation)
            {
                case "AND":
                    result = inputOneValue & inputTwoValue;
                    break;
                case "OR":
                    result = inputOneValue | inputTwoValue;

                    break;
                case "XOR":
                    result = inputOneValue ^ inputTwoValue;

                    break;
                default:
                    throw new Exception("Invalid Operation");
            }
            finalWireValues.Add(wire, result);
            return (result, finalWireValues);
        }
        private (Dictionary<string, bool>, Dictionary<string, Gate>) ReadInitialGateInfo(string[] lines)
        {
            var initialWireValues = new Dictionary<string, bool>();
            var gateConnections = new Dictionary<string, Gate>();
            
            int i = 0;
            while (i < lines.Length)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    break;
                }
                string[] info = lines[i].Split(": ");
                initialWireValues.Add(info[0], info[1] == "1");
                i++;
            }
            i++;
            while (i < lines.Length)
            {
                string[] info = lines[i].Split(" ");
                var gate = new Gate(info[0], info[2], info[1], info[4]);
                gateConnections.Add(gate.Output, gate);
                i++;
            }
            return (initialWireValues, gateConnections);
        }
    }
}