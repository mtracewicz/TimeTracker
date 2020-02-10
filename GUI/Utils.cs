using System;
using System.Windows.Media;
using System.Text.RegularExpressions;
using DataBaseLibrary;
using WCFLibrary;
using System.Collections.Generic;

namespace GUI
{
    class Utils
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
            return true;
            if (Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<RecordModel> Convert(DbRecord[] records)
        {
            List<RecordModel> output = new List<RecordModel>();
            foreach(var rec in records)
            {
                output.Add(new RecordModel(rec.AppName, rec.StartTime, rec.EndTime, rec.Id));
            }
            return output;
        }
    }
}
