using System.Text.RegularExpressions;

namespace AoC2024
{
    public sealed class Day4 : AoCSupport.Day
    {
        public override string DayNumber => "4";
        public override string Year => "2024";
        public override string PartA()
        {
            Regex xmasPattern = new Regex("(?=XMAS|SAMX)");

            int total = 0;
            foreach (string[] direction in GetAllDirections(_input.Lines))
            {
                total += CountWordOccurrences(direction, xmasPattern);
            }

            return total.ToString();
        }
        public override string PartB()
        {
            return "";
        }
        private string[] GetVerticalLines(string[] horizontal)
        {
            string[] vertical = new string[horizontal[0].Length];
            for (int col = 0; col < horizontal[0].Length; col++)
            {
                string newRow = "";
                for (int row = 0; row < horizontal.Length; row++)
                {
                    newRow += horizontal[row][col];
                }
                vertical[col] = newRow;
            }
            return vertical;
        }
        private string[] GetPositiveDiagonalLines(string[] horizontal)
        {
            int maxRow = horizontal.Length;
            int maxCol = horizontal[0].Length;
            string[] vertical = new string[maxRow + maxCol - 1];

            int startRow = 0;
            int startCol = 0;
            int diagonalRow = 0;
            while (startRow < maxRow)
            {
                string newRow = "";
                int curRow = startRow;
                int curCol = startCol;
                
                while (curRow < maxRow && curCol >= 0)
                {
                    newRow += horizontal[curRow][curCol];
                    curRow++;
                    curCol--;
                }

                vertical[diagonalRow] = newRow;
                diagonalRow++;
                
                if (startCol == maxCol - 1)
                {
                    startRow++;
                }
                else
                {
                    startCol++;
                }
            }
            return vertical;
        }
        private string[] GetNegativeDiagonalLines(string[] horizontal)
        {
            int maxRow = horizontal.Length;
            int maxCol = horizontal[0].Length;
            string[] vertical = new string[maxRow + maxCol - 1];

            int startRow = maxRow - 1;
            int startCol = 0;
            int diagonalRow = 0;
            while (startCol < maxCol)
            {
                string newRow = "";
                int curRow = startRow;
                int curCol = startCol;

                while (curRow < maxRow && curCol < maxCol)
                {
                    newRow += horizontal[curRow][curCol];
                    curRow++;
                    curCol++;
                }

                vertical[diagonalRow] = newRow;
                diagonalRow++;

                if (startRow == 0)
                {
                    startCol++;
                }
                else
                {
                    startRow--;
                }
            }
            return vertical;
        }
        private string[][] GetAllDirections(string[] horizontal)
        {
            return new string[][] { horizontal, 
                    GetVerticalLines(horizontal),
                    GetPositiveDiagonalLines(horizontal),
                    GetNegativeDiagonalLines(horizontal)
                    };
        }
        private int CountWordOccurrences(string[] search, Regex word)
        {
            int count = 0;
            foreach (string s in search)
            {
                count += word.Count(s);
            }
            return count;
        }
    }
}
