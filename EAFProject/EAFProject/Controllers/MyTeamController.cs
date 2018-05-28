using DataLibrary;
using EAFProject.DatabaseController;
using EAFProject.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EAFProject.Controllers
{
    public class Department
    {
        public Department()
        { }
        public Department(int depId, string depName)
        {
            this.DepartmentId = depId;
            this.DepartmentName = depName;
        }
        public Department(Department dep)
        {
            this.DepartmentId = dep.DepartmentId;
            this.DepartmentName = dep.DepartmentName;
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
    public class MyTeamController : Controller
    {


        private EAF2Entities context;
        public MyTeamController()
        {
            context = new EAF2Entities();
        }
        [HttpGet]
        public ActionResult Index()
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }

            DateTime value = new DateTime(1900, 1, 1);
            string MgrId = System.Web.HttpContext.Current.Session["swgId"].ToString();

            List<EmployeeClass> ecl = new List<EmployeeClass>();

            #region api
            //* GET list sample
            string url = "http://localhost:26091/api/departments";
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(url);
            GETRequest.Method = "GET";
            GETRequest.ContentType = "application/json";
            var getResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream getResponseStream = getResponse.GetResponseStream();
            StreamReader sr = new StreamReader(getResponseStream);
            var responseFromServer = sr.ReadToEnd();

            List<Department> departments = JsonConvert.DeserializeObject<List<Department>>(responseFromServer);
            foreach (var dep in departments)
            {
                int id = dep.DepartmentId;
                string depName = dep.DepartmentName;
            }
            //

            /* GET from uri sample
            string url = "http://localhost:26091/api/departments/20";
            System.Net.HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(url);
            GETRequest.Method = "GET";
            GETRequest.ContentType = "application/json";
            var getResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream getResponseStream = getResponse.GetResponseStream();
            StreamReader sr = new StreamReader(getResponseStream);
            var responseFromServer = sr.ReadToEnd();

            Department department = JsonConvert.DeserializeObject<Department>(responseFromServer);

            int id = department.DepartmentId;
            string depName = department.DepartmentName;
            */

            /* POST sample
            string url = "http://localhost:26091/api/departments";
            HttpWebRequest POSTRequest = (HttpWebRequest)WebRequest.Create(url2);
            POSTRequest.Method = "POST";
            POSTRequest.ContentType = "application/json";
            Department dep2 = new Department(97, "from code - department");
            string serializedDep2 = JsonConvert.SerializeObject(dep2);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(serializedDep2);
            POSTRequest.ContentLength = byteArray.Length;
            POSTRequest.Connection = "true";
            Stream dataStream = POSTRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            HttpWebResponse response = (HttpWebResponse)POSTRequest.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer2 = HttpUtility.UrlDecode(reader.ReadToEnd());
            dataStream.Close();
            */

            /* DELETE from uri sample
            string url3 = "http://localhost:26091/api/departments/98";
            System.Net.HttpWebRequest DELETERequest = (HttpWebRequest)WebRequest.Create(url3);
            DELETERequest.Method = "DELETE";
            DELETERequest.ContentType = "application/json";
           
            DELETERequest.Connection = "true";
            Stream dataStream3 = DELETERequest.GetRequestStream();

            HttpWebResponse response3 = (HttpWebResponse)DELETERequest.GetResponse();
            dataStream3 = response3.GetResponseStream();
            StreamReader reader3 = new StreamReader(dataStream3);
            string responseFromServer3 = HttpUtility.UrlDecode(reader3.ReadToEnd());
            dataStream3.Close();
            */

            /* DELETE from body sample
            string url3 = "http://localhost:26091/api/departments";
            System.Net.HttpWebRequest DELETERequest = (HttpWebRequest)WebRequest.Create(url3);
            DELETERequest.Method = "DELETE";
            DELETERequest.ContentType = "application/json";
            //Department dep2 = new Department(97, "from code - department");
            int depId = 99;
            string serializedDep3 = JsonConvert.SerializeObject(depId);
            byte[] byteArray3 = System.Text.Encoding.UTF8.GetBytes(serializedDep3);
            DELETERequest.ContentLength = byteArray3.Length;
            DELETERequest.Connection = "true";
            Stream dataStream3 = DELETERequest.GetRequestStream();
            dataStream3.Write(byteArray3, 0, byteArray3.Length);
            dataStream3.Close();

            HttpWebResponse response3 = (HttpWebResponse)DELETERequest.GetResponse();
            dataStream3 = response3.GetResponseStream();
            StreamReader reader3 = new StreamReader(dataStream3);
            string responseFromServer3 = HttpUtility.UrlDecode(reader3.ReadToEnd());
            dataStream3.Close();
            */

            /* PUT Sample
            string url5 = "http://localhost:26091/api/departments";
            System.Net.HttpWebRequest PUTRequest = (HttpWebRequest)WebRequest.Create(url5);
            PUTRequest.Method = "PUT";
            PUTRequest.ContentType = "application/json";
            Department dep5 = new Department(97, "department 97");
            string serializedDep5 = JsonConvert.SerializeObject(dep5);
            byte[] byteArray5 = System.Text.Encoding.UTF8.GetBytes(serializedDep5);
            PUTRequest.ContentLength = byteArray5.Length;
            PUTRequest.Connection = "true";
            Stream dataStream5 = PUTRequest.GetRequestStream();
            dataStream5.Write(byteArray5, 0, byteArray5.Length);
            dataStream5.Close();

            HttpWebResponse response5 = (HttpWebResponse)PUTRequest.GetResponse();
            dataStream5 = response5.GetResponseStream();
            StreamReader reader5 = new StreamReader(dataStream5);
            string responseFromServer5 = HttpUtility.UrlDecode(reader5.ReadToEnd());
            dataStream5.Close();
            */
            #endregion
            
            var data = context.JobTitles.Join(context.Emps, j => j.JobId, e => e.JobId, (j, e) => new { JobTitles = j, Emps = e }).
                Join(context.Depts, e => e.Emps.DeptId, d => d.DeptId, (e, d) => new { Emps = e, Depts = d }).
                Join(context.Products, e => e.Emps.Emps.ProductId, p => p.ProductId, (e, p) => new { Emps = e, Products = p }).
                Where(ssa => ssa.Emps.Emps.Emps.ManagerId == MgrId &&
                (string.IsNullOrEmpty(ssa.Emps.Emps.Emps.DOL.ToString()) || SqlFunctions.DateDiff("day", ssa.Emps.Emps.Emps.DOL, value) == 0 || ssa.Emps.Emps.Emps.DOL >= SqlFunctions.GetDate())).
                Select(ssa => new
                {
                    ssa.Emps.Emps.Emps.SWG,
                    ssa.Emps.Emps.Emps.Name,
                    ssa.Emps.Emps.JobTitles.JobTitle1,
                    ssa.Emps.Depts.DeptName,
                    ssa.Products.ProductName,
                    ssa.Emps.Emps.Emps.ManagerId,
                    ssa.Emps.Emps.Emps.DOJ,
                    ssa.Emps.Emps.Emps.Email,
                    ssa.Emps.Emps.Emps.DOR,
                    ssa.Emps.Emps.Emps.DOL,
                    ssa.Emps.Emps.Emps.DOO
                });

            foreach (var o in data)
            {
                EmployeeClass temp = new EmployeeClass();
                temp.swg = o.SWG;
                temp.name = o.Name;
                temp.jobTitle = o.JobTitle1;
                temp.deptName = o.DeptName;
                temp.product = o.ProductName;
                temp.manager = o.ManagerId;
                temp.dateOfJoining = o.DOJ;
                temp.email = o.Email;
                temp.dateOfResignation = o.DOR;
                temp.lastDay = o.DOL;
                temp.dateOfOffer = o.DOO;
                ecl.Add(temp);
            }

            //DBManager db = new DBManager();
            //ViewBag.MgrId = MgrId;
            //var data1 = db.GetMyTeam(MgrId);



            if (System.Web.HttpContext.Current.Session["swgId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            //return View("Index", data);
            return View("Index", ecl);
        }
        [HttpGet]
        public ActionResult MyTeamDetails(string swg, string MgrId)
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            DBManager db = new DBManager();
            EmployeeClass e = new EmployeeClass();
            ViewBag.MgrId = MgrId;
            e = db.GetMyTeamDetails(swg);
            if (Session["swgId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            return View("MyTeamDetails", e);
        }
        public ActionResult MyTeamEdit(string swg)
        {
            DBManager db = new DBManager();
            ViewBag.MgrId = System.Web.HttpContext.Current.Session["swgId"].ToString();
            ViewBag.JobList = db.JobTitles();
            ViewBag.ProductList = db.ProductNames();
            ViewBag.DeptList = db.DeptNames();
            ViewBag.IsSubmitted = false;
            EmployeeClass e = new EmployeeClass();
            ViewBag.SWGId = swg;
            e = db.GetMyTeamDetails(swg);
            ViewBag.JobID = e.jobId;
            ViewBag.DeptID = e.DeptId;
            ViewBag.Product = e.ProductId;
            return View("MyTeamEdit", e);
        }
        //[HttpGet]
        //public ActionResult MyTeamEdit(string swg)
        //{
        //    DBManager db = new DBManager();
        //    EmployeeClass e = new EmployeeClass();
        //    ViewBag.SWGId = swg;
        //    e = db.GetMyTeamDetails(swg);
        //    return View("MyTeamEdit",e);
        //}
        [HttpPost]
        public ActionResult MyTeamEdit(FormCollection frm)
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            DBManager db = new DBManager();
            EmployeeClass emp = new EmployeeClass();
            string MgrId = System.Web.HttpContext.Current.Session["swgId"].ToString(); ;
            string swg;
            emp.swg = frm["swg"];
            swg = frm["swg"];
            emp.name = frm["name"];
            emp.jobId = Int32.Parse(frm["jobId"]);
            emp.DeptId = Int32.Parse(frm["DeptId"]);
            emp.ProductId = Int32.Parse(frm["ProductId"]);
            emp.manager = frm["manager"];
            if ((frm["dateOfJoining"]) != null && frm["dateOfJoining"].Length > 0)
            {
                emp.dateOfJoining = Convert.ToDateTime(frm["dateOfJoining"]);
            }
            emp.email = frm["email"];
            if (frm["dateOfResignation"] != null && frm["dateOfResignation"].Length > 0)
                emp.dateOfResignation = Convert.ToDateTime(frm["dateOfResignation"].ToString());
            else
                emp.dateOfResignation = null;
            if (frm["lastDay"] != null && frm["lastDay"].Length > 0)
                emp.lastDay = Convert.ToDateTime(frm["lastDay"].ToString());
            else
                emp.lastDay = null;
            if (frm["dateOfOffer"] != null && frm["dateOfOffer"].Length > 0)
                emp.dateOfOffer = Convert.ToDateTime(frm["dateOfOffer"].ToString());
            else
                emp.dateOfOffer = null;
            int i = db.UpdateTeam(emp, MgrId);
            if (i == 1)
            {
                ViewBag.Status = "Submitted !";
            }
            //ViewBag.JobList = db.JobTitles();
            //ViewBag.ProductList = db.ProductNames();
            //ViewBag.DeptList = db.DeptNames();
            //ViewBag.IsSubmitted = true;
            return RedirectToAction("Index", "MyTeam");
        }
        public ActionResult AddTeam(string MgrId)
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            DBManager db = new DBManager();
            ViewBag.MgrId = MgrId;
            ViewBag.JobList = db.JobTitles();
            ViewBag.ProductList = db.ProductNames();
            ViewBag.DeptList = db.DeptNames();
            ViewBag.IsSubmitted = false;
            return View();
        }
        [HttpPost]
        //[MultipleButton(Name = "action", Argument = "AddTeam")]
        public ActionResult AddTeam(FormCollection frm, string MgrId)
        {
            LoginCredentials LG = new LoginCredentials();
            LG.swg = System.Web.HttpContext.Current.Session["swgId"] as String;
            System.Web.HttpContext.Current.Session["role"] = LoginController.role();
            if (System.Web.HttpContext.Current.Session["role"].ToString() == "null")
            {
                return Content("Not Authorized User");
            }
            DBManager db = new DBManager();
            EmployeeClass emp = new EmployeeClass();
            ViewBag.MgrId = MgrId;
            string MgrSWG = db.CheckTeamMember(frm["swg"]);
            if (MgrSWG != null)
            {
                db.UpdateMgrSwg(MgrId, frm["swg"], Int32.Parse(frm["jobId"]), Int32.Parse(frm["DeptId"]), Int32.Parse(frm["ProductId"]));
            }
            else
            {
                emp.swg = frm["swg"];
                emp.name = frm["name"];
                emp.jobId = Int32.Parse(frm["jobId"]);
                emp.DeptId = Int32.Parse(frm["DeptId"]);
                emp.ProductId = Int32.Parse(frm["ProductId"]);
                emp.manager = MgrId;

                if ((frm["dateOfJoining"]) != null && frm["dateOfJoining"].Length > 0)
                {
                    emp.dateOfJoining = Convert.ToDateTime(frm["dateOfJoining"]);
                }
                emp.email = frm["email"];
                if (frm["dateOfResignation"] != null && frm["dateOfResignation"].Length > 0)
                    emp.dateOfResignation = Convert.ToDateTime(frm["dateOfResignation"].ToString());
                else
                    emp.dateOfResignation = null;
                if (frm["lastDay"] != null && frm["lastDay"].Length > 0)
                    emp.lastDay = Convert.ToDateTime(frm["lastDay"].ToString());
                else
                    emp.lastDay = null;
                if (frm["dateOfOffer"] != null && frm["dateOfOffer"].Length > 0)
                    emp.dateOfOffer = Convert.ToDateTime(frm["dateOfOffer"].ToString());
                else
                    emp.dateOfOffer = null;
                int i = db.AddTeam(emp);
                if (i == 1)
                {
                    ViewBag.Status = "Submitted !";
                }
            }
            //ViewBag.JobList = db.JobTitles();
            //ViewBag.ProductList = db.ProductNames();
            //ViewBag.DeptList = db.DeptNames();
            //ViewBag.IsSubmitted = true;
            return RedirectToAction("Index", "MyTeam");
        }
        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "CheckTeamMember")]
        public string CheckTeamMember(string swg)
        {
            DBManager db = new DBManager();
            string name = null;
            string MgrSWG = db.CheckTeamMember(swg);
            TempData["MgrSWG"] = MgrSWG;
            if (MgrSWG != null)
            {
                name = db.GetName(MgrSWG);
            }
            return name;
        }
    }
}
