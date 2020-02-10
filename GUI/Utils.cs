using DataBaseLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using WCFLibrary;

namespace GUI
{
    public class Utils
    {
        public static Brush GetBrushFromHex(String hexColor)
        {
            if (CheckValidFormatHtmlColor(hexColor))
            {
                return (Brush)new BrushConverter().ConvertFromString(hexColor);
            }
            else
            {
                throw new ArgumentException("Invalid color!");
            }
        }

        public static bool CheckValidFormatHtmlColor(string inputColor)
        {
            if (inputColor.StartsWith("#"))
            {
                inputColor = inputColor.Substring(1);
            }

            if (inputColor.Length != 6 && inputColor.Length != 8)
            {
                return false;
            }

            List<char> ValidInHex = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char c in inputColor)
            {
                if (!ValidInHex.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<RecordModel> Convert(DbRecord[] records)
        {
            List<RecordModel> output = new List<RecordModel>();
            foreach (var rec in records)
            {
                output.Add(new RecordModel(rec.AppName, rec.StartTime, rec.EndTime, rec.Id));
            }
            return output;
        }

        public static string ParseTime(string timeInSeconds)
        {
            if (double.TryParse(timeInSeconds, out double time))
            {
                if (time > 0)
                {
                    return (time * 1000.0).ToString();
                }
            }
            return "60000";
        }
    }
}
