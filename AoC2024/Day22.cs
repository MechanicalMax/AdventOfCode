namespace AoC2024
{
    internal class Day22 : AoCSupport.Day
    {
        public override string DayNumber => "22";
        public override string Year => "2024";
        public override string PartA()
        {
            long sum = 0;

            foreach (string start in _input.Lines)
            {
                long curResult = long.Parse(start);
                for (int _ = 0; _ < 2000; _++)
                {
                    curResult = evolveNumber(curResult);
                }
                sum += curResult;
            }

            return sum.ToString();
        }
        public override string PartB()
        {
            throw new NotImplementedException();
        }
        private long evolveNumber(long secretNumber)
        {
            long stepOne = secretNumber << 6;
            secretNumber ^= stepOne;
            secretNumber %= 16777216;
            //secretNumber = secretNumber >> 24;
            
            long stepTwo = secretNumber >> 5;
            secretNumber ^= stepTwo;
            secretNumber %= 16777216;

            long stepThree = secretNumber << 11;
            secretNumber ^= stepThree;
            secretNumber %= 16777216;
            return secretNumber;
        }
    }
}