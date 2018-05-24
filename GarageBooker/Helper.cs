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
    class Helper
    {
        internal static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        internal static void CheckFileExists(string filePath)
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

        internal static TimeSpan ParseTime(string time)
        {
            if (time.Length == 4)
            {
                time = "0" + time;
            }
            var timeFormat = TimeSpan.ParseExact(time, @"hh\.mm", CultureInfo.InvariantCulture);
            return timeFormat;
        }
    }
}
