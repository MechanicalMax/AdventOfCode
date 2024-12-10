using System.Data.SqlTypes;

namespace AoC2024
{
    internal class Day9 : AoCSupport.Day
    {
        public override string DayNumber => "9";
        public override string Year => "2024";
        public override string PartA()
        {
            var storage = DecompressDiskMapAsArray(_input.Lines[0]);

            storage = MoveLastBlocks(storage);

            return CalculateCheckSum(storage).ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private double CalculateCheckSum(int[] storage)
        {
            double sum = 0;

            for (int i = 0; i < storage.Length; i++)
            {
                if (storage[i] == -1)
                {
                    continue;
                }
                sum += i * storage[i];
            }

            return sum;
        }
        private int[] MoveLastBlocks(int[] blocks)
        {
            int destinationIndex = 0;
            int sourceIndex = blocks.Length - 1;

            while (destinationIndex < sourceIndex)
            {
                if (blocks[destinationIndex] == -1)
                {
                    while (blocks[sourceIndex] == -1)
                    {
                        sourceIndex--;
                    }
                    blocks[destinationIndex] = blocks[sourceIndex];
                    blocks[sourceIndex] = -1;
                }
                destinationIndex++;
            }

            return blocks;
        }
        private int CalculateRequiredSize(string line)
        {
            int size = 0;
            foreach (char c in line)
            {
                size += c - '0';
            }
            return size;
        }
        private int[] DecompressDiskMapAsArray(string line)
        {
            var storage = new int[CalculateRequiredSize(line)];

            bool representsFile = true;
            int curIDNum = 0;
            int insertIndex = 0;
            foreach (char c in line)
            {
                int length = (int)char.GetNumericValue(c);
                for (int i = 0; i < length; i++) {
                    if (representsFile)
                    {
                        storage[insertIndex + i] = curIDNum;
                    }
                    else
                    {
                        storage[insertIndex + i] = -1;
                    }
                }
                if (representsFile)
                {
                    curIDNum++;
                }
                representsFile = !representsFile;
                insertIndex += length;
            }

            return storage;
        }
    }
}