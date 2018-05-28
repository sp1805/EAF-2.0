using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EAFProject.DatabaseController;
using EAFProject.Controllers;
using EAFProject.BusinessComponents;
using EAFProject.ViewModels;
using DataLibrary;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using Newtonsoft.Json;

namespace EAFProject.Controllers
{
   class MyEntity
    {
        public int ReqId {get;set;}
        public string JobTitle {get;set;}
        public string ProductName {get;set;}
        public int Vacancies {get;set;}
        public string StatusValue {get;set;}
        public int criticality {get;set;}
        public int StatusId {get;set;}
        public string Name {get;set;}
    }
    public class DisplayRequestController : Controller
    {
        
        private EAF2Entities context;

        public DisplayRequestController()
        {
            context = new EAF2Entities();
        }
        public ActionResult Index(string swg)
        {
            ViewBag.Department = BusinessComponents.Request.DeptNames();
            ViewBag.IsSubmitted = false;
            return View();
        }

        //Requets page Display
        [HttpGet]
        public ActionResult Index()
        {
            string MgrId = System.Web.HttpContext.Current.Session["swgId"] as String;
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            LG.getRole();
            System.Web.HttpContext.Current.Session["role"] = LG.role;
            if (System.Web.HttpContext.Current.Session["role"].ToString()=="null")
            {
                return Content("Not Authorized User");
            }
            //todoRK
            ViewBag.MgrId = MgrId;
            List<RequestData> rdl = new List<RequestData>();
            string query = @"WITH tree (SWG, name, level) AS (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + LG.swg + "'  ) Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid) and Requests.StatusId!=6 ORDER BY CONVERT(DateTime, Requests.lastmodifiedDate,101)  DESC";
            var obj = context.Database.SqlQuery<MyEntity>(query).ToList();
            foreach(var ob in obj)
            {
                RequestData rd = new RequestData();
                rd.SWGId = LG.swg;
                rd.ReqId = ob.ReqId;
                rd.JobTitle = ob.JobTitle;
                rd.ProductName = ob.ProductName;
                rd.vacancies = ob.Vacancies;
                rd.StatusValue = ob.StatusValue;
                rd.criticality = ob.criticality;
                if(rd.criticality == 1)
                    rd.CriticalityName = "Normal";
                else if(rd.criticality == 2)
                    rd.CriticalityName = "Urgent";
                rd.Status = ob.StatusId;
                rd.LastReviewedBy = ob.Name;
                rdl.Add(rd);  
            }
            //var data = BusinessComponents.Request.GetMyRequest(LG);
            ViewBag.Department = BusinessComponents.Request.DeptNames();
            ViewBag.pageInfo = "MyRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "MyRequests";
            ViewBag.IsSubmitted = false;
            return View("Index", rdl);

        }

        //Request Details Action
        [HttpGet]
        public ActionResult Index1(int ReqId)
        {
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            string swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session.Add("ReqId", ReqId);
            ViewBag.MgrId = swg;
            ViewBag.ReqId = ReqId;
            ViewBag.HRList = BusinessComponents.Request.GetHRList();
            var data = BusinessComponents.Request.GetMyRequestDetails(ReqId, swg);
           
           
            return View("Index1", data);
        }

        //Button Post Method : Approve
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Approve")]
        public ActionResult Approve(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            BusinessComponents.Request.DirectorApprovalRequest(ReqId, Comments, swg);
            
            LoginCredentials Lg = new LoginCredentials();
            Lg.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(Lg);
            if (System.Web.HttpContext.Current.Session["pageInfo"].ToString() == "PendingRequests")
            {
                return RedirectToAction("PendingResult", "DisplayPending", System.Web.HttpContext.Current.Session["swgId"].ToString());
            }
            else
            {
                return View("Index", data);
            }
        }

        //Button Post Method : Resubmit
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Resubmit")]
        public ActionResult Resubmit(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            BusinessComponents.Request.ResubmitRequest(ReqId, Comments, swg);
            
            LoginCredentials Lg = new LoginCredentials();
            Lg.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(Lg);
            if (System.Web.HttpContext.Current.Session["pageInfo"].ToString() == "PendingRequests")
            {
                return RedirectToAction("PendingResult", "DisplayPending", System.Web.HttpContext.Current.Session["swgId"].ToString());
            }
            else
            {
                return View("Index", data);
            }


        }

        //Button Post Method : Cancel
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cancel")]
        public ActionResult Cancel(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            BusinessComponents.Request.CancelRequest(ReqId, Comments, swg);
            
            LoginCredentials LG = new LoginCredentials();
            LG.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(LG);
            return View("Index", data);

        }

        //Button Post Method : OnHold
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "OnHold")]
        public ActionResult OnHold(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            BusinessComponents.Request.OnHoldRequest(ReqId, Comments, swg);

            LoginCredentials Lg = new LoginCredentials();
            Lg.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(Lg);
            return RedirectToAction("PendingResult", "DisplayPending", System.Web.HttpContext.Current.Session["swgId"].ToString());
        }

        //Button Post Method : Processing
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Processing")]
        public ActionResult Processing(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            string AssignHR = C["item.HRName"];
            BusinessComponents.Request.Processing(ReqId, Comments, swg, AssignHR);
            
            LoginCredentials Lg = new LoginCredentials();
            Lg.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(Lg);
            return RedirectToAction("HRRequest", "DisplayHRRequests", System.Web.HttpContext.Current.Session["swgId"].ToString());
            
        }

        //Button Post Method : Complete
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Complete")]
        public ActionResult Complete(FormCollection C)
        {
            String Comments = C["item.InputComments"];
            int ReqId = int.Parse(C["item.ReqId"]);
            string swg = C["item.SWGId"];
            BusinessComponents.Request.Complete(ReqId, Comments, swg);
            
            LoginCredentials Lg = new LoginCredentials();
            Lg.swg = swg;
            var data = BusinessComponents.Request.GetMyRequest(Lg);
            if (System.Web.HttpContext.Current.Session["pageInfo"].ToString() == "PendingRequests")
            {
                return RedirectToAction("PendingResult", "DisplayPending", System.Web.HttpContext.Current.Session["swgId"].ToString());
            }
            else
            {
                return View("Index", data);
            }
        }

        //Button For Search
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(FormCollection C)
        {

            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            string SearchBy = C["SearchBy"].ToString();
            string SearchFor = C["SearchFor"].ToString();
            if (SearchBy == "approvedBy")
            {
                return View("Index", BusinessComponents.Request.SearchApprove(SearchBy, SearchFor, LG.swg));
            }
            else
            {
                return View("Index", BusinessComponents.Request.Search(SearchBy, SearchFor, LG.swg));
            }

        }
    }
}
