using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EAFProject.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
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

    }
}
