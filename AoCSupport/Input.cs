using System;
using System.IO;

namespace AoCSupport
{
    public class Input
    {
        public string Raw { get; init; }
        public string[] Lines { get; init; }
        public Input(string day)
        {
            string inputFileName = "day" + day + ".txt";
            try
            {
                string inputDir = GetInputDir();

                string inputFilePath = Path.Combine(inputDir, inputFileName);
                
                if (!File.Exists(inputFilePath))
                {
                    throw new FileNotFoundException(inputFileName + " Not Found");
                }
            
                Raw = File.ReadAllText(inputFilePath).Trim();
                Lines = Raw.Split("\n");
            }
            catch (Exception ex)
            {
                Raw = "";
                Lines = new string[0];
                Console.WriteLine("Input Failed: " + ex.Message);
            }
        }

        private static string GetInputDir()
        {
            string inputDir = Directory.GetCurrentDirectory();
            try
            {
                inputDir = GetParentDirectory(inputDir);
                inputDir = GetParentDirectory(inputDir);
                inputDir = GetParentDirectory(inputDir);

                inputDir = Path.Combine(inputDir, "Input");
                
                if (!Directory.Exists(inputDir))
                {
                    throw new DirectoryNotFoundException(inputDir + " Not Found");
                }
            }
            catch (Exception)
            {
                throw;
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
