using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EAFProject.BusinessComponents;
using System.Web.Mvc;
using EAFProject.DatabaseController;
using EAFProject.ViewModels;


namespace EAFProject.Controllers
{
    public class CreateRequestController : Controller
    {
        public ActionResult NewHire()
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            ViewBag.MgrId = System.Web.HttpContext.Current.Session["swgId"] as String;
            string swgid = System.Web.HttpContext.Current.Session["swgId"] as String;
            ViewBag.JobList = BusinessComponents.Request.JobTitles();
            ViewBag.ProductList = BusinessComponents.Request.ProductNames();
            ViewBag.DeptList = BusinessComponents.Request.DeptNames();
            ViewBag.SkillsList = BusinessComponents.Request.Skills();
            ViewBag.ReplacementFor = BusinessComponents.Request.EmployeeNames(swgid);
            ViewBag.FromManager = BusinessComponents.Request.ManagerLists();

            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Normal", Value = "1" },
                new SelectListItem { Text = "Urgent", Value = "2" },
            };
            ViewBag.Criticality = ObjList;
            ViewBag.IsSubmitted = false;
            return View();
        }


        // New Hire
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "NewHire")]
        public ActionResult NewHire(FormCollection frm)
        {

            ViewBag.MgrId = System.Web.HttpContext.Current.Session["swgId"] as String;
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            RequestData c = new RequestData();
            if (ModelState.IsValid)
            {
                c.JobId = int.Parse(frm["JobId"]);
                c.DeptId = int.Parse(frm["DeptId"]);
                c.ProductId = int.Parse(frm["ProductId"]);
                c.criticality = int.Parse(frm["criticality"]);
                c.EstimatedCTC = 50000;
                c.minEstimatedCTC = 150000;
                c.EssentialSkillIds = frm["EssentialIdsSelectedList"].Split(',').Select(int.Parse).ToList();
                c.DesiredSkillIds = frm["DesiredIdsSelectedList"].Split(',').Select(int.Parse).ToList();
                c.OptionalSkillIds = frm["OptionalIdsSelectedList"].Split(',').Select(int.Parse).ToList();
                c.CreatedByEmpId = System.Web.HttpContext.Current.Session["swgId"] as String;
                c.vacancies = int.Parse(frm["vacancies"]);
                c.mineducation = frm["mineducation"];
                c.minexperience = int.Parse(frm["minexperience"]);
                c.InputComments = frm["InputComments"];
                c.ReqType = "New Hire";
                c.CreatedDate = DateTime.Now;
                c.LastModifiedDate = DateTime.Now;
                int ReqId = BusinessComponents.Request.CreateNewRequest(c);


                ViewBag.reqID = ReqId;
                ViewBag.JobList = BusinessComponents.Request.JobTitles();
                ViewBag.ProductList = BusinessComponents.Request.ProductNames();
                ViewBag.DeptList = BusinessComponents.Request.DeptNames();
                ViewBag.SkillsList = BusinessComponents.Request.Skills();
                ViewBag.IsSubmitted = true;

                return RedirectToAction("Index", "DisplayRequest", c.CreatedByEmpId);
            }
            return RedirectToAction("NewHire", LG);
        }

        [HttpPost]
        //Replacement
        [MultipleButton(Name = "action", Argument = "Replacement")]
        public ActionResult Replacement(FormCollection frm)
        {
            RequestData c = new RequestData();
            c.JobId = Int32.Parse(frm["JobId"]);
            c.DeptId = Int32.Parse(frm["DeptId"]);
            c.ProductId = Int32.Parse(frm["ProductId"]);
            c.ReplacementFor = frm["EmpId"];
            c.criticality = Int32.Parse(frm["criticality"]);
            c.EstimatedCTC = 50000;
            c.minEstimatedCTC = 150000;
            c.EssentialSkillIds = frm["EssentialIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.DesiredSkillIds = frm["DesiredIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.OptionalSkillIds = frm["OptionalIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.CreatedByEmpId = System.Web.HttpContext.Current.Session["swgId"] as String;
            c.minexperience = Int32.Parse(frm["minexperience"]);
            c.mineducation = frm["mineducation"];
            c.ReqType = "Replacement";
            c.CreatedDate = DateTime.Now;
            c.LastModifiedDate = DateTime.Now;
            c.InputComments = frm["InputComments"];

            int ReqId = BusinessComponents.Request.CreateReplacementRequest(c);

            ViewBag.reqID = ReqId;
            ViewBag.JobList = BusinessComponents.Request.JobTitles();
            ViewBag.ProductList = BusinessComponents.Request.ProductNames();
            ViewBag.DeptList = BusinessComponents.Request.DeptNames();
            ViewBag.SkillsList = BusinessComponents.Request.Skills();
            ViewBag.IsSubmitted = true;
            return RedirectToAction("Index", "DisplayRequest", c.CreatedByEmpId);
        }

        [HttpPost]
        //Internal Transfer
        [MultipleButton(Name = "action", Argument = "InternalHire")]
        public ActionResult InternalHire(FormCollection frm)
        {
            RequestData c = new RequestData();
            c.JobId = Int32.Parse(frm["JobId"]);
            c.DeptId = Int32.Parse(frm["DeptId"]);
            c.ProductId = Int32.Parse(frm["ProductId"]);

            c.criticality = Int32.Parse(frm["criticality"]);
            c.EstimatedCTC = 50000;
            c.minEstimatedCTC = 150000;
            c.EssentialSkillIds = frm["EssentialIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.DesiredSkillIds = frm["DesiredIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.OptionalSkillIds = frm["OptionalIdsSelectedList"].Split(',').Select(int.Parse).ToList();
            c.CreatedByEmpId = System.Web.HttpContext.Current.Session["swgId"] as String;
            c.ReqType = "Internal Hire";
            c.CreatedDate = DateTime.Now;
            c.LastModifiedDate = DateTime.Now;
            c.FromManagerId = frm["FromManagerId"];
            c.FromProductId = Int32.Parse(frm["FromProductId"]);
            c.FromDeptId = Int32.Parse(frm["FromDeptId"]);
            c.ReplacementFor = frm["ReplacementFor"];

            int ReqId = BusinessComponents.Request.CreateInternalHire(c);

            ViewBag.reqID = ReqId;
            ViewBag.JobList = BusinessComponents.Request.JobTitles();
            ViewBag.ProductList = BusinessComponents.Request.ProductNames();
            ViewBag.DeptList = BusinessComponents.Request.DeptNames();
            ViewBag.SkillsList = BusinessComponents.Request.Skills();
            //ViewBag.ManagerIDList = Helper.ManagerIDs();
            //ViewBag.EmpIds = Helper.EmpIDs("sks");
            ViewBag.IsSubmitted = true;
            return RedirectToAction("Index", "DisplayRequest", c.CreatedByEmpId);
        }
    }
}