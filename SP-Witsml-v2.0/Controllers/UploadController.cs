using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess;
using SP_Witsml_v2._0.Services;

namespace SP_Witsml_v2._0.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if(Path.GetExtension(file.FileName).Equals(".xml"))
            {
                string FileName = Path.GetFileName(file.FileName);
                string FilePath = Path.Combine(Server.MapPath("~/Uploads"), FileName);
                file.SaveAs(FilePath);
                Log log = EnergisticsConverter.XmlToObject<Log>(System.IO.File.ReadAllText(FilePath));
                System.IO.File.Delete(FilePath);
                LogService service = new LogService(log, FileName);
                service.SaveLogToDB();

                ViewBag.Message = "File Uploaded Successfully";
                return View();
            }



            ViewBag.Message = "File Upload Failed";
            return View();
        }

    }
}