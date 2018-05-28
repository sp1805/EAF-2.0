using EAFProject.ViewModels;
using System;
using System.Web.Mvc;

namespace EAFProject.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult LoginError(FormCollection T)
        {
           //System.Threading.Thread.Sleep(3000);
           LoginCredentials login = new LoginCredentials(T);
            if (login.IsDesignationNullOrEmpty())
            {
                return PartialView("LoginError", login);
            }
            if (login.IsManager())
            {
                return Json(new { url = Url.Action("Index", "MyTeam") });
                //return RedirectToAction("Index", "MyTeam");
            }
            else
            {
                return Json(new { url = Url.Action("HRRequest", "DisplayHRRequests") });
                //return RedirectToAction("HRRequest", "DisplayHRRequests");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection T)
        {
            LoginCredentials login = new LoginCredentials(T);

            if (login.IsDesignationNullOrEmpty())
            {
                System.Threading.Thread.Sleep(3000);

                return View();
            }

            if (login.IsManager())
            {
                return RedirectToAction("Index", "MyTeam");
            }
            else
            {
                return RedirectToAction("HRRequest", "DisplayHRRequests");
            }

        }


        public ActionResult Release()
        {
            System.Web.HttpContext.Current.Session["Role"] = null;
            LoginCredentials Login = new LoginCredentials();
            string opl = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            Login.swg = opl.Substring(4);
            System.Web.HttpContext.Current.Session["swgId"] = Login.swg;
            string condition = BusinessComponents.Request.DepartmentId(System.Web.HttpContext.Current.Session["swgId"] as String);
            if (String.IsNullOrEmpty(condition))
            {

                return Content("Not Authorized User");
            }
            else
            {
                System.Web.HttpContext.Current.Session["role"] = condition;
            }


            if (condition == "Manager")
            {
                return RedirectToAction("Index", "MyTeam");
            }
            else
            {
                return RedirectToAction("HRRequest", "DisplayHRRequests", Login.swg);
            }

        }

        public static string role()
        {

            if (System.Web.HttpContext.Current.Session["role"] == null || String.IsNullOrEmpty(System.Web.HttpContext.Current.Session["role"].ToString()))
            {
                if (System.Web.HttpContext.Current.Session["swgId"] == null || String.IsNullOrEmpty(System.Web.HttpContext.Current.Session["swgId"].ToString()))
                {
                    System.Web.HttpContext.Current.Session["swgId"] = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                }
                LoginCredentials login = new LoginCredentials();
                login.swg = System.Web.HttpContext.Current.Session["swgId"].ToString();
                login.getRole();
                string condition = login.role;
                //BusinessComponents.Request.DepartmentId(System.Web.HttpContext.Current.User.Identity.Name.ToString());
                if (String.IsNullOrEmpty(condition))
                {

                    System.Web.HttpContext.Current.Session["role"] = "null";
                }
                else
                {
                    System.Web.HttpContext.Current.Session["role"] = condition;
                }
            }
            return System.Web.HttpContext.Current.Session["role"].ToString();
        }
    }
}
