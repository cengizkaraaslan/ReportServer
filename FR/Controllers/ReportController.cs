using Core.Utilities.Security.Encyption;
using FastReport;
//using FastReport.Export.Csv;
//using FastReport.Export.Html;
//using FastReport.Export.Pdf;
//using FastReport.Export.RichText;
//using FastReport.Export.Text;
//using FastReport.Export.Xml;
using FastReport.Utils;
using FastReport.Data;
using FastReport.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using TowerSoft.HtmlToExcel;

namespace FR.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [Obsolete]
        IHostingEnvironment env;
        IConfiguration ConfigurationManager;
        [Obsolete]
        public ReportController(IHostingEnvironment env, IConfiguration ConfigurationManager)
        {
            this.ConfigurationManager = ConfigurationManager;
            this.env = env;
        }
        [HttpGet()]
        [Obsolete]
        public IActionResult Get([FromQuery] string AppName, string ReportName, string Extension, string Parameters)

        {
            byte[] Result = new byte[] { };
            Config.WebMode = true;

            // Uzanti

            string Uzanti = Extension;

            // Raporu belirle

            string RFN = env.ContentRootPath + "\\FR\\" + AppName + "\\" + ReportName + ".frx";

            Report R = new Report();
           

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
                    return Ok(Convert.ToBase64String(f.ConvertHtmlString(System.Text.Encoding.Default.GetString(Result)).Save()));
                case "xls":

                    
                    //  HtmlToExcelSettings settings = HtmlToExcelSettings.Defaults;
                    //  settings.AutofitColumns = false;
                    //  settings.ShowFilter = false;
                    //  settings.ShowRowStripes = false;

                    ////  byte[] fileData = new WorkbookGenerator(settings).FromHtmlString(System.Text.Encoding.Default.GetString(Result));
                    //  using (WorkbookBuilder workbookBuilder = new WorkbookBuilder(settings))
                    //  {

                    //      workbookBuilder.AddSheet("sheetName", f.ConvertHtmlString(System.Text.Encoding.Default.GetString(Result)), settings);
                    //      Ok(workbookBuilder.GetAsByteArray());
                    //  }
                    break;
                default:
                    break;
            }

            return Ok(Result);
        }

        private string CS(string AppName)
        {
            try
            {

                var R = ConfigurationManager.GetSection("CS:" + AppName).Value.ToString();
                R = EncrpytDecrypt.DecryptString(R, AppName);
                return R;
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }
    }
}
