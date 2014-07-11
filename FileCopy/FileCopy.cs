using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace FileCopy
{
    class FileCopy
    {
        static int Main(string[] args)
        {
            if (args.Length < 3 || (args.Length == 1 && args.First().Equals("/?")))
            {
                Help("Usage");

                return 1;
            }

            //if (Path.IsPathRooted(args[1]) || Path.IsPathRooted(args[2]))
            //{
            //    Help("FullPath");

            //    return 2;
            //}

            CopyFiles(args[0], args[1], args[2]);

            return 0;
        }

        private static void Help(string flag)
        {
            switch (flag)
            {
                case "Usage":
                    Console.WriteLine("Usage: filecopy [full path to textfile with list of files to copy] [full path of filelist to copy] [full path to copy to]");
                    Console.WriteLine(@"Example: filecopy filetocopy.txt c:\folder\files d:\folder\copied_files");
                    Console.WriteLine("/?\tShow help.");
                    break;
                case "FullPath":
                    Console.WriteLine("Full path to both files must be supplied.");
                    break;
            }
        }

        private static void CopyFiles(string listOfFilesToCopy, string pathToCopyFrom, string pathToCopyTo)
        {
            var dirs = Directory.GetDirectories(pathToCopyFrom);

            using (var text = File.OpenText(listOfFilesToCopy))
            {
                while (!text.EndOfStream)
                {
                    var fileName = text.ReadLine();

                    if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    var fileToCopy = (from dir in dirs
                                      where File.Exists(Path.Combine(dir, fileName))
                                      select Path.Combine(dir, fileName)).First();

                    File.Copy(fileToCopy, Path.Combine(pathToCopyTo, fileName), true);
                }
            }
        }
    }
}