using System.Collections.Immutable;
using System.Data;

namespace AoC2024
{
    internal class Day23 : AoCSupport.Day
    {
        public override string DayNumber => "23";
        public override string Year => "2024";
        public override string PartA()
        {
            var edgeList = GetEdgeList(_input.Lines);// "kh-tc\r\nqp-kh\r\nde-cg\r\nka-co\r\nyn-aq\r\nqp-ub\r\ncg-tb\r\nvc-aq\r\ntb-ka\r\nwh-tc\r\nyn-cg\r\nkh-ub\r\nta-co\r\nde-co\r\ntc-td\r\ntb-wq\r\nwh-td\r\nta-ka\r\ntd-qp\r\naq-cg\r\nwq-ub\r\nub-vc\r\nde-ta\r\nwq-aq\r\nwq-vc\r\nwh-yn\r\nka-de\r\nkh-ta\r\nco-tc\r\nwh-qp\r\ntb-vc\r\ntd-yn".Split("\r\n"));

            var threeInterconnectedComputers = new HashSet<string>();

            foreach (var nodeInfo in edgeList)
            {
                if (nodeInfo.Key.StartsWith("t") == false)
                {
                    continue;
                }

                foreach (var secondNode in nodeInfo.Value)
                {
                    foreach (var thirdNode in edgeList[secondNode])
                    {
                        if (edgeList[thirdNode].Contains(nodeInfo.Key))
                        {
                            string[] computers = [nodeInfo.Key, secondNode, thirdNode];
                            Array.Sort(computers);
                            threeInterconnectedComputers.Add(string.Join("", computers));
                            //Console.WriteLine($"{nodeInfo.Key},{secondNode},{thirdNode}");
                        }
                    }
                }
            }

            return threeInterconnectedComputers.Count.ToString();
        }
        /*
        * PartB Based on Code from HyperNeutrino
        * https://youtu.be/kHIWvxRWQ9k?si=wRaboDPpeqC6lpYj
        */
        public override string PartB()
        {
            var edgeList = GetEdgeList(_input.Lines);//"kh-tc\r\nqp-kh\r\nde-cg\r\nka-co\r\nyn-aq\r\nqp-ub\r\ncg-tb\r\nvc-aq\r\ntb-ka\r\nwh-tc\r\nyn-cg\r\nkh-ub\r\nta-co\r\nde-co\r\ntc-td\r\ntb-wq\r\nwh-td\r\nta-ka\r\ntd-qp\r\naq-cg\r\nwq-ub\r\nub-vc\r\nde-ta\r\nwq-aq\r\nwq-vc\r\nwh-yn\r\nka-de\r\nkh-ta\r\nco-tc\r\nwh-qp\r\ntb-vc\r\ntd-yn".Split("\r\n"));

            var interconnectedComputers = new HashSet<List<string>>();

            foreach (var nodeInfo in edgeList)
            {
                interconnectedComputers = searchConnections(nodeInfo.Key, new List<string> { nodeInfo.Key },
                    interconnectedComputers, edgeList);
            }

            string[] maxConnectedComputers = [.. interconnectedComputers.MaxBy(x => x.Count)];
            Array.Sort(maxConnectedComputers);
            return string.Join(',', maxConnectedComputers);
        }
        private HashSet<List<string>> searchConnections(string node, List<string> required,
            HashSet<List<string>> interconnectedComputers, Dictionary<string, List<string>> edgeList)
        {
            required.Sort();
            if (interconnectedComputers.Contains(required))
            {
                return interconnectedComputers;
            }

            interconnectedComputers.Add(required);
            foreach (var neigbor in edgeList[node])
            {
                if (required.Contains(neigbor))
                {
                    continue;
                }

                bool neigborIsConnectedToAllRequiredPoints = true;
                foreach (var requiredConnection in required)
                {
                    if (!edgeList[requiredConnection].Contains(neigbor))
                    {
                        neigborIsConnectedToAllRequiredPoints = false;
                        break;
                    }
                }
                if (!neigborIsConnectedToAllRequiredPoints)
                {
                    continue;
                }

                var listWithNeigbor = required;
                listWithNeigbor.Add(neigbor);

                interconnectedComputers.UnionWith(searchConnections(neigbor,
                    listWithNeigbor,
                    interconnectedComputers, edgeList));
            }

            return interconnectedComputers;
        }
        private Dictionary<string, List<string>> GetEdgeList(string[] lines)
        {
            var edgeList = new Dictionary<string, List<string>>();

            foreach (var line in lines) {
                string[] nodes = line.Split('-');
                
                if (edgeList.ContainsKey(nodes[0]))
                {
                    edgeList[nodes[0]].Add(nodes[1]);
                }
                else
                {
                    edgeList[nodes[0]] = new List<string>() { nodes[1] };
                }

                if (edgeList.ContainsKey(nodes[1]))
                {
                    edgeList[nodes[1]].Add(nodes[0]);
                }
                else
                {
                    edgeList[nodes[1]] = new List<string>() { nodes[0] };
                }
            }

            return edgeList;
        }
    }
}