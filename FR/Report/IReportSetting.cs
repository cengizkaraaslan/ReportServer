using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FR.Report
{
    public interface IReportSetting
    {
        public string createReport(string AppName, string ReportName, string Extension, string Parameters);


    }
}
