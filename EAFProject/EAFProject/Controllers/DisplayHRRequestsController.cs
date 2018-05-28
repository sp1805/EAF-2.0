using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EAFProject.ViewModels;
using DataLibrary;

namespace EAFProject.Controllers
{
    public class DisplayHRRequestsController : Controller
    {
        //
        // GET: /DisplayHRRequests/

        private EAF2Entities context;
        public DisplayHRRequestsController()
        {
            context = new EAF2Entities();
        }
        public ActionResult HRRequest(string MgrId)
        {
            return View();
        }
        // Cast method - thanks to type inference when calling methods it 
        // is possible to cast object to type without knowing the type name
        T Cast<T>(object obj, T type)
        {
            return (T)obj;
        }
        [HttpGet]
        public ActionResult HRRequest()
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            string MgrId = LG.swg;
            ViewBag.MgrId = MgrId;
            ViewBag.role = System.Web.HttpContext.Current.Session["Role"] as String;
            //here
            //var data = BusinessComponents.Request.GetHRRequest(LG);

            //IQueryable<Emp> empT = context.Emps.Where(e => e.ManagerId == null);
            //List<Emp> res = context.Emps.Where(e => e.ManagerId == null).ToList();
            List<string> empSWG = new List<string>();
            var data1 = context.Emps.Where(e => e.ManagerId == null).Select(r => new { r.SWG }).ToList();
            foreach(var e in data1)
            {
                empSWG.Add(e.SWG);
            }
            var data2 = context.Requests.Join(context.JobTitles, r => r.jobid, j => j.JobId, (r, j) => new { Requests = r, JobTitles = j }).
                Join(context.Products, r => r.Requests.ProductId, p => p.ProductId, (r, p) => new { Requests = r, Products = p }).
                Join(context.Status, r => r.Requests.Requests.StatusId, s => s.StatusId, (r, s) => new { Requests = r, Status = s }).
                Where(res => res.Requests.Requests.Requests.AssignedTo == LG.swg).
                Select( res => new
                {
                    res.Requests.Requests.Requests.ReqId,
                    res.Requests.Requests.JobTitles.JobTitle1,
                    res.Requests.Products.ProductName,
                    res.Requests.Requests.Requests.Vacancies,
                    res.Status.StatusValue,
                    res.Requests.Requests.Requests.Criticality,
                    res.Requests.Requests.Requests.StatusId
                }).
                Union(
                context.Requests.Join(context.JobTitles, r => r.jobid, j => j.JobId, (r, j) => new { Requests = r, JobTitles = j }).
                Join(context.Products, r => r.Requests.ProductId, p => p.ProductId, (r, p) => new { Requests = r, Products = p }).
                Join(context.Status, r => r.Requests.Requests.StatusId, s => s.StatusId, (r, s) => new { Requests = r, Status = s }).
                Join(context.RequestComments, r => r.Requests.Requests.Requests.ReqId, rc => rc.ReqId , (r, rc) => new { Requests = r, RequestComments = rc}).
                Where(res => res.RequestComments.statusId == 2 && empSWG.Contains(res.RequestComments.EmpId)).
                Select(res => new
                {
                    res.Requests.Requests.Requests.Requests.ReqId,
                    res.Requests.Requests.Requests.JobTitles.JobTitle1,
                    res.Requests.Requests.Products.ProductName,
                    res.Requests.Requests.Requests.Requests.Vacancies,
                    res.Requests.Status.StatusValue,
                    res.Requests.Requests.Requests.Requests.Criticality,
                    res.Requests.Requests.Requests.Requests.StatusId
                })
                );
            List<BusinessComponents.RequestData> RD = new List<BusinessComponents.RequestData>();

            foreach(var rd in data2)
            {
                BusinessComponents.RequestData temp = new BusinessComponents.RequestData();
                temp.SWGId = LG.swg;
                temp.ReqId = rd.ReqId;
                temp.JobTitle = rd.JobTitle1;
                temp.ProductName = rd.ProductName;
                temp.vacancies = rd.Vacancies;
                temp.StatusValue = rd.StatusValue;
                temp.criticality = rd.Criticality;
                if (temp.criticality == 1)
                {
                    temp.CriticalityName = "Normal";
                }
                else if (temp.criticality == 2)
                {
                    temp.CriticalityName = "Urgent";
                }
                temp.Status = (int)rd.StatusId;
                RD.Add(temp);
            }
            //
            ViewBag.pageInfo = "HRRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "HRRequests";
            return View("~/Views/DisplayRequest/Index.cshtml", RD);
        }
        public ActionResult Search(FormCollection C)
        {
            ViewBag.Department = BusinessComponents.Request.DeptNames();
            ViewBag.ManagerList = BusinessComponents.Request.ManagerLists();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Normal", Value = "1" },
                new SelectListItem { Text = "Urgent", Value = "2" },
            };
            ViewBag.Criticality = ObjList;
            ViewBag.Designation = BusinessComponents.Request.JobTitles();
            ViewBag.ProductName = BusinessComponents.Request.ProductNames();

            List<SelectListItem> ObjList1 = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Approved", Value = "2" },
                new SelectListItem { Text = "Processing HR", Value = "4" },
                new SelectListItem { Text = "Completed", Value = "6" },
            };
            ViewBag.Status = ObjList1;
            ViewBag.pageInfo = "HRRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "HRRequests";
            ViewBag.IsSubmitted = false;
            return View("~/Views/DisplayRequest/Search.cshtml");

        }
        [HttpPost]
        public ActionResult HRRequest(FormCollection C)
        {
            ViewBag.pageInfo = "HRRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "HRRequests";
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            string SearchBy = C["SearchBy"].ToString();
            string SearchFor = C["SearchFor"].ToString();
            if (SearchBy == "approvedBy")
            {
                return View("~/Views/DisplayRequest/Index.cshtml", BusinessComponents.Request.SearchApproveHR(SearchBy, SearchFor, LG.swg));
            }
            else
            {
                return View("~/Views/DisplayRequest/Index.cshtml", BusinessComponents.Request.SearchHR(SearchBy, SearchFor, LG.swg));
            }
        }
    }
}
