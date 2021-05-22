using Core.Utilities.Security.Encyption;
using FastReport.Data;
using FastReport.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FR.Report
{
    public class ReportSetting : IReportSetting
    {
        private IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _ConfigurationManager;

        public ReportSetting(IWebHostEnvironment environment, IConfiguration configurationManager)
        {
            _ConfigurationManager = configurationManager;
            _hostingEnvironment = environment;

        }
        public string createReport(string AppName, string ReportName, string Extension, string Parameters)
        {
            byte[] Result = new byte[] { };
            string Uzanti = Extension;
            string RFN = _hostingEnvironment.ContentRootPath + "\\FR\\" + AppName + "\\" + ReportName + ".frx";

            FastReport.Report R = new FastReport.Report();
            // Connection stringi setle
            string ConnectionString = CS(AppName);
            if (ConnectionString != "" & ConnectionString != "null")
            {
                MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
                sqlConnection.ConnectionString = ConnectionString;
                R.Report.Dictionary.Connections.Add(sqlConnection);
                FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            }

            // Rapor parametrelerini setle

            string[] ParametersArray = Parameters.Split(';');

            foreach (string Parameter in ParametersArray)
            {
                string ParameterName = Parameter.Split('=')[0];
                string ParameterValue = Parameter.Split('=')[1];
                R.SetParameterValue(ParameterName, ParameterValue);
            }
            R.Load(RFN);
            R.Prepare();
            MemoryStream MS = new MemoryStream();
            FastReport.Export.Html.HTMLExport ExportHTML = new FastReport.Export.Html.HTMLExport();
            R.Export(ExportHTML, MS);
            ExportHTML.Dispose();
            R.Dispose();
            MS.Position = 0;
            Result = MS.ToArray();
            MS.Dispose();
            switch (Uzanti)
            {
                case "pdf":
                    SelectPdf.HtmlToPdf f = new SelectPdf.HtmlToPdf();
                    return (Convert.ToBase64String(f.ConvertHtmlString(System.Text.Encoding.Default.GetString(Result)).Save()));

                default:
                    return Convert.ToBase64String(Result);

            }
        }

        private string CS(string AppName)
        {
            var R = _ConfigurationManager.GetSection("CS:" + AppName).ToString();
            return EncrpytDecrypt.DecryptString(R, AppName);
        }
    }
}
