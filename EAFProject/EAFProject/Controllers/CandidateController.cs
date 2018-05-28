using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using EAFProject.DatabaseController;
using EAFProject.ViewModels;

namespace EAFProject.Controllers
{
    public class CandidateController : Controller
    {
        //
        // GET: /AddCandidate/
        public ActionResult AddCandidate()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCandidates(int ReqId)
        {
            DBManager db = new DBManager();
            ViewBag.ReqId = ReqId;
            TempData["ReqId"] = ReqId;
            var data = db.GetCandidates(ReqId);
            return View("GetCandidates", data);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [MultipleButton(Name = "action", Argument = "AddCandidate")]
        public ActionResult AddCandidate(HttpPostedFileBase file_Uploader, FormCollection frm, int ReqId)
        {

            if (file_Uploader != null)
            {
                int TempId;
                DBManager db = new DBManager();
                ViewBag.ReqId = ReqId;
                CandidateClass uploadFileModel = new CandidateClass();
                uploadFileModel.name = frm["name"];
                uploadFileModel.email = frm["email"];
                uploadFileModel.phoneNumber = Convert.ToInt64(frm["PhoneNumber"]);
                uploadFileModel.CreatedDate = Convert.ToDateTime(frm["CreatedDate"].ToString());
                string fileName = string.Empty;
                string destinationPath = string.Empty;
                string Path1 = ConfigurationManager.AppSettings["FilePath"].ToString();
                fileName = Path.GetFileName(file_Uploader.FileName);
                //string name = fileName.Substring(0, fileName.LastIndexOf("."));
                string extn = fileName.Substring(fileName.LastIndexOf("."));
                fileName = "Resume" + "_" + uploadFileModel.name + "_" + (Environment.TickCount & Int32.MaxValue) + extn;
                uploadFileModel.FileName = fileName;
                destinationPath = Path.Combine(Path1, fileName);
                TempId = db.AddCandidate(uploadFileModel);
                int i = db.RequestedCandidates(ReqId, TempId);
                file_Uploader.SaveAs(destinationPath);
                if (i > 1)
                {
                    ViewBag.Status = "Submitted !";
                }
                
            }
            return RedirectToAction("GetCandidates", "Candidate", new { ReqId = ReqId });
        }

        public FileResult OpenFile(string fileName)
        {
            string Path1 = ConfigurationManager.AppSettings["FilePath"].ToString();
            string s;
            try
            {
                s = Path1 + "/" + fileName;
                return File(new FileStream(s, FileMode.Open), "application/octetstream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult Interview(int TempId)
        {
            List<InterviewComments> InterviewC = new List<InterviewComments>();
            int ReqId, Id_Interviewed;
            string swg = System.Web.HttpContext.Current.Session["swgId"].ToString();
            DBManager db = new DBManager();
            TempData["TempId"] = TempId;
            ReqId = Convert.ToInt32(TempData["ReqId"]);
            TempData.Keep();
            string CandidateName = db.CandidateName(TempId);
            ViewBag.CandidateName = CandidateName;
            var data = new List<ViewModels.Interview>();
            Id_Interviewed = db.GetInterviewedId(TempId, ReqId);
            ViewBag.Id_Interviewed = Id_Interviewed;
            TempData["Id_Interviewed"] = Id_Interviewed;
            data = db.GetSkills(ReqId, Id_Interviewed);
            if (Id_Interviewed != 0)
            {
                //display interview form
                InterviewC = db.GetInterViewComments(Id_Interviewed);
            }
            return View(Tuple.Create(data, InterviewC));
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Interview")]
        public ActionResult Interview([Bind(Prefix = "Item1")]List<ViewModels.Interview> Rating, FormCollection frm)
        {
            int Id_Interviewed;
            DBManager db = new DBManager();
            ViewModels.Interview objInt = new ViewModels.Interview();
            InterviewComments IntComments = new InterviewComments();
            int ReqId = Convert.ToInt32(TempData["ReqId"]);
            int TempId = Convert.ToInt32(TempData["TempId"]);
            Id_Interviewed = Convert.ToInt32(TempData["Id_Interviewed"]);
            TempData.Keep();
            objInt.status = frm["status"];
            objInt.comments = frm["comments"];
            string swg = System.Web.HttpContext.Current.Session["swgId"].ToString();
            IntComments.InterviewerId = swg;
            IntComments.Comments = frm["comments"];
            IntComments.Status = frm["status"];
            IntComments.Datetime = DateTime.Now;
            if (Id_Interviewed != 0)
            {
                //Update interview table(new comments)
                db.UPDATEInterviews(Id_Interviewed, swg, IntComments.Status);
            }
            else
            {
                Id_Interviewed = db.Interviews(swg, objInt);
                int f = db.SetCandidateMapping(TempId, ReqId, Id_Interviewed);
                foreach (ViewModels.Interview i in Rating)
                {
                    int rating = i.Rating;
                    int skill = i.SkillId;
                    db.SetRating(Id_Interviewed, rating, skill);
                }
            }
            int f1 = db.Interviewcomments(Id_Interviewed, swg, IntComments, TempId, ReqId);
            int f2 = db.UpdateCommentCandidate(frm["status"], TempId);
            return RedirectToAction("GetCandidates", "Candidate", new { ReqId = ReqId });
        }
    }
}
