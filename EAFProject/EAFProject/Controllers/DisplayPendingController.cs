using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EAFProject.DatabaseController;
using EAFProject.Controllers;
using EAFProject.BusinessComponents;
using EAFProject.ViewModels;

namespace EAFProject.Controllers
{
    public class DisplayPendingController : Controller
    {
        //
        // GET: /DisplayPending/

        public ActionResult PendingRequests(string MgrId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult PendingResult()
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
            var data = BusinessComponents.Request.GetPendingRequest(LG);
            ViewBag.pageInfo = "PendingRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "PendingRequests";
            return View("~/Views/DisplayRequest/Index.cshtml", data);
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
            ViewBag.pageInfo = "PendingRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "PendingRequests";

            List<SelectListItem> ObjList1 = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Created", Value = "1" },
                new SelectListItem { Text = "Approved", Value = "2" },
                new SelectListItem { Text = "Processing HR", Value = "4" },
                new SelectListItem { Text = "On Hold", Value = "5" },
                new SelectListItem { Text = "Completed", Value = "6" },
                new SelectListItem { Text = "Cancelled", Value = "7" },
                new SelectListItem { Text = "Resubmitted", Value = "8" },
            };
            ViewBag.Status = ObjList1;

            ViewBag.IsSubmitted = false;
            return View("~/Views/DisplayRequest/Search.cshtml");

        }
        [HttpPost]
        public ActionResult PendingResult(FormCollection C)
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            string SearchBy = C["SearchBy"].ToString();
            string SearchFor = C["SearchFor"].ToString();
            ViewBag.pageInfo = "PendingRequests";
            System.Web.HttpContext.Current.Session["pageInfo"] = "PendingRequests";
            if (SearchBy == "approvedBy")
            {
                return View("~/Views/DisplayRequest/Index.cshtml", BusinessComponents.Request.SearchApprovePending(SearchBy, SearchFor, LG.swg));
            }
            else
            {
                return View("~/Views/DisplayRequest/Index.cshtml", BusinessComponents.Request.SearchPending(SearchBy, SearchFor, LG.swg));
            }
        }
    }
}
