using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportsLibrary
{
    public class ReportGenerator
    {
        public static Task WriteReportToFileAsync(string fileName, IEnumerable<ReportEntryModel> reportEntries)
        {
            return Task.Run(() => WriteReportToFile(fileName, reportEntries));
        }

        public static void WriteReportToFile(String fileName, IEnumerable<ReportEntryModel> reportEntries)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                if(reportEntries.Count() <= 0) 
                {
                    sw.WriteLine("No entries matching following criteria.");
                    return;
                }
                foreach (var entry in reportEntries)
                {
                    sw.WriteLine($"{entry.Name};{entry.TotalTime}");
                }
            }
        }
    }
}
