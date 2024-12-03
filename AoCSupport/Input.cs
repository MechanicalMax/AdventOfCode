using System;
using System.IO;

namespace AoCSupport
{
    public sealed class Input
    {
        public string Raw { get; init; }
        public string[] Lines { get; init; }
        public Input(string day, string year)
        {
            ValidateDay(day);
            ValidateYear(year);

            string inputDir = GetInputDir();
            string inputFileName = "day" + day + ".txt";
            string inputFilePath = Path.Combine(inputDir, inputFileName);
                
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException(inputFileName + " Not Found");
            }
            
            Raw = File.ReadAllText(inputFilePath).Trim();
            Lines = Raw.Split("\n");
        }
        private static void ValidateDay(string day)
        {
            try
            {
                int dayNum = int.Parse(day);
                if (dayNum < 1 || dayNum > 25)
                {
                    throw new ArgumentException("Day Number is not on the Advent Calendar!", day);
                }
                if (dayNum > DateTime.Now.Day)
                {
                    throw new ArgumentException("Cannot use input from future days!", day);
                }
            }
            catch
            {
                throw;
            }
        }
        private static void ValidateYear(string year)
        {
            try
            {
                int yearNum = int.Parse(year);
                if (yearNum < 2015)
                {
                    throw new ArgumentException("AoC did not exist before the year 2015!", year);
                }
                if (yearNum > DateTime.Now.Year)
                {
                    throw new ArgumentException("Cannot use input from future years!", year);
                }
            }
            catch
            {
                throw;
            }
        }
        private static string GetInputDir()
        {
            string inputDir = Directory.GetCurrentDirectory();
            
            inputDir = GetParentDirectory(inputDir);
            inputDir = GetParentDirectory(inputDir);
            inputDir = GetParentDirectory(inputDir);
            inputDir = Path.Combine(inputDir, "Input");
                
            if (!Directory.Exists(inputDir))
            {
                throw new DirectoryNotFoundException(inputDir + " Not Found");
            }
            
            return inputDir;
        }
        private static string GetParentDirectory(string inputDir)
        {
            DirectoryInfo? parentInfo = Directory.GetParent(inputDir);
            if (parentInfo != null)
            {
                return parentInfo.FullName;
            }
            else
            {
                throw new DirectoryNotFoundException("Parent of " + inputDir + " not found");
            }
        }
    }
}
