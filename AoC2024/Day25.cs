using System.Text;

namespace AoC2024
{
    internal class Day25 : AoCSupport.Day
    {
        public override string DayNumber => "25";
        public override string Year => "2024";
        public override string PartA()
        {
            List<int[]> lockHeights = [];
            List<int[]> keyHeights = [];

            int i = 0;
            bool isKey = false;
            while (i < _input.Lines.Length)
            {
                if (_input.Lines[i][0] == '.')
                {
                    isKey = false;
                }
                else
                {
                    isKey = true;
                }
                i++;
                var heights = new int[5];
                for (int j = 0; j < heights.Length; j++)
                {
                    var line = _input.Lines[i + j];
                    for (int k = 0; k < line.Length; k++)
                    {
                        if (line[k] == '#')
                        {
                            heights[k]++;
                        }
                    }
                }
                if (isKey)
                {
                    keyHeights.Add(heights);
                }
                else
                {
                    lockHeights.Add(heights);
                }
                i += 7;
            }

            int count = 0;
            foreach (var lockHeight in lockHeights)
            {
                foreach (var keyHeight in keyHeights)
                {
                    bool noOverlaps = true;
                    for (int w = 0; w < lockHeight.Length; w++)
                    {
                        if (lockHeight[w] + keyHeight[w] > 5)
                        {
                            noOverlaps = false;
                            break;
                        }
                    }
                    if (noOverlaps)
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
    }
}