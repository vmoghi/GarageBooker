using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
namespace GarageBooker
{
    public class Helper
    {
        internal static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static List<string> GetRowsFromFile(string pathFile, int maxNextDays)
        {
            var listRows = new List<string>();
            CheckFileExists(pathFile);

            var info = new FileInfo(pathFile);
            if (info.CreationTime <= DateTime.Now.AddDays(maxNextDays))
            {
                using (var fs = new FileStream(info.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            var nextRow = sr.ReadLine().Trim();
                            if (nextRow == "")
                            {
                                continue;
                            }
                            listRows.Add(nextRow);
                        }
                    }
                }
            }
            return listRows;
        }


        internal static TimeSpan ParseTime(string time)
        {
            if (time.Length == 4)
            {
                time = "0" + time;
            }
            var timeFormat = TimeSpan.ParseExact(time, @"hh\.mm", CultureInfo.InvariantCulture);
            return timeFormat;
        }
        internal static void WriteToFile(string filePath, string newAppoinment)
        {
            using (StreamWriter tw = File.AppendText(filePath))
            {
                tw.WriteLine(newAppoinment);
                tw.WriteLine("");
                tw.Flush();
            }
        }

        private static void CheckFileExists(string filePath)
        {
            var info = new FileInfo(filePath);
            var dir = info.DirectoryName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(filePath))
            {
                var newFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                var writerObj = new StreamWriter(newFile);
                newFile.Close();
            }
        }
    }
}
