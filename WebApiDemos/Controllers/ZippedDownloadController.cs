using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApiDemos.Filters;
using Ionic.Zip;

namespace WebApiDemos.Controllers
{
    [System.Web.Http.RoutePrefix("FileDemos")]
    public class ZippedDownloadController : Controller
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ZippedDownload")]
        [MyweatherExceptionFilter]
        public FileContentResult Get()
        {
            FileContentResult result;
            var fileFolder = System.Web.HttpContext.Current.Server.MapPath(@"~\Files");
            var files = Directory.GetFiles(fileFolder);

            using (var zip = new ZipFile())
            {
                foreach (var file in files)
                {
                    var inputStream = new FileStream(file, FileMode.Open);
                    zip.AddEntry(new FileInfo(file).Name, inputStream);
                }

                var memoryStream = new MemoryStream();
                zip.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                result = new FileContentResult(memoryStream.ToArray(), "application/octet-stream")
                {
                    FileDownloadName = "result.zip"
                };
            }
            return result;
        }
    }
}