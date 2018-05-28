using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using EAFProject.Models;
using EAFProject.DatabaseController;
namespace EAFProject.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/

        public ActionResult FileUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader, FormCollection frm)
        {
            if (file_Uploader != null)
            {
                DBManager db = new DBManager();
                CandidateClass uploadFileModel = new CandidateClass();
                uploadFileModel.name = frm["name"];
                uploadFileModel.email = frm["email"];
                uploadFileModel.phoneNumber = Convert.ToInt64(frm["PhoneNumber"]);
                string fileName = string.Empty;
                string destinationPath = string.Empty;
                string Path1 = ConfigurationManager.AppSettings["FilePath"].ToString();
                fileName = Path.GetFileName(file_Uploader.FileName);
                //string name = fileName.Substring(0, fileName.LastIndexOf("."));
                string extn = fileName.Substring(fileName.LastIndexOf("."));
                fileName = "Resume" + "_" + uploadFileModel.name + "_" + (Environment.TickCount & Int32.MaxValue) + extn;
                uploadFileModel.FileName = fileName;
                destinationPath = Path.Combine(Path1, fileName);
                db.AddCandidate(uploadFileModel);
                file_Uploader.SaveAs(destinationPath);
            }
            return View();
        }
    }
}
