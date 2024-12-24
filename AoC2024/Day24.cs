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
            (var initialWireValues, var gateConnections) = ReadInitialGateInfo(_input.Lines);// "x00: 1\r\nx01: 0\r\nx02: 1\r\nx03: 1\r\nx04: 0\r\ny00: 1\r\ny01: 1\r\ny02: 1\r\ny03: 1\r\ny04: 1\r\n\r\nntg XOR fgs -> mjb\r\ny02 OR x01 -> tnw\r\nkwq OR kpj -> z05\r\nx00 OR x03 -> fst\r\ntgd XOR rvg -> z01\r\nvdt OR tnw -> bfw\r\nbfw AND frj -> z10\r\nffh OR nrd -> bqk\r\ny00 AND y03 -> djm\r\ny03 OR y00 -> psh\r\nbqk OR frj -> z08\r\ntnw OR fst -> frj\r\ngnj AND tgd -> z11\r\nbfw XOR mjb -> z00\r\nx03 OR x00 -> vdt\r\ngnj AND wpb -> z02\r\nx04 AND y00 -> kjc\r\ndjm OR pbm -> qhw\r\nnrd AND vdt -> hwm\r\nkjc AND fst -> rvg\r\ny04 OR y02 -> fgs\r\ny01 AND x02 -> pbm\r\nntg OR kjc -> kwq\r\npsh XOR fgs -> tgd\r\nqhw XOR tgd -> z09\r\npbm OR djm -> kpj\r\nx03 XOR y03 -> ffh\r\nx00 XOR y04 -> ntg\r\nbfw OR bqk -> z06\r\nnrd XOR fgs -> wpb\r\nfrj XOR qhw -> z04\r\nbqk OR frj -> z07\r\ny03 OR x01 -> nrd\r\nhwm AND bqk -> z03\r\ntgd XOR rvg -> z12\r\ntnw OR pbm -> gnj".Split("\r\n"));

            string curOutput = "z00";
            int curIteration = 1;
            var finalWireValues = initialWireValues;
            while (gateConnections.ContainsKey(curOutput))
            {
                (_, finalWireValues) = DetermineFinalWireValue(curOutput, finalWireValues, gateConnections);
                Console.WriteLine(curOutput);

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