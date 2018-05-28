using DataLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace EAFProject.ViewModels
{
    public class LoginCredentials
    {
        public string swg { get; set; }
        public string role { get; set; }

        private EAF2Entities context;

        public LoginCredentials()
        {
        }

        public LoginCredentials(FormCollection T)
        {
            System.Web.HttpContext.Current.Session["Role"] = null;
            swg = T["swg"];
            getRole();
        }
        public void getRole()
        {
            string opl = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            System.Web.HttpContext.Current.Session["swgId"] = swg;



            context = new EAF2Entities();

            var emp = context.Emps.FirstOrDefault(e => e.SWG == swg);


            if (emp != null)
            {
                var jobTitles = context.JobTitles.FirstOrDefault(j => j.JobId == emp.JobId
                                                                      &&
                                                                      (j.JobTitle1.Contains("manager") ||
                                                                       j.JobTitle1.Contains("Chief") ||
                                                                       j.JobTitle1.Contains("Director")));

                if (jobTitles != null)
                {
                    role = jobTitles.JobTitle1;
                    role = "Manager";
                }
                else
                {
                    string hrdIds = ConfigurationManager.AppSettings["HRDepartmentID"];

                    List<int> ids = hrdIds.Split(',').Select(int.Parse).ToList();

                    var hr = context.Emps.FirstOrDefault(e => ids.Contains(e.DeptId) && e.SWG == swg);

                    if (hr != null)
                    {
                        role = "HR";
                    }
                    else
                    {
                        role = null;
                    }
                }
            }

            if (role != null)
            {
                System.Web.HttpContext.Current.Session["role"] = role;
            }
        }
        public bool IsDesignationNullOrEmpty()
        {
            if (String.IsNullOrEmpty(role))
            {
                return true;
            }
            return false;
        }

        public bool IsManager()
        {

            if (role == "Manager")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}