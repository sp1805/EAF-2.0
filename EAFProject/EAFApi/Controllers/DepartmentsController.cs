using DataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EAFApi.Controllers
{
    public class Department
    {
        public Department()
        { }
        public Department(Department dep)
        {
            this.DepartmentId = dep.DepartmentId;
            this.DepartmentName = dep.DepartmentName;
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
    public class DepartmentsController : ApiController
    {
        private readonly EAF2Entities context;

        public DepartmentsController()
        {
            context = new EAF2Entities();
        }

        public IEnumerable<Department> Get()
        //public HttpResponseMessage Get()
        {
            List<Department> departments = new List<Department>();
            var deps = context.Depts;
            foreach (var department in deps)
            {
                Department dep = new Department();
                dep.DepartmentId = Int32.Parse(department.DeptId.ToString());
                dep.DepartmentName = department.DeptName;
                departments.Add(dep);
            }
            return departments;
            //String responseJson = JsonConvert.SerializeObject(departments);
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(responseJson, System.Text.Encoding.UTF8, "application/json");
            //return response;

        }

        // GET api/departments/5
        public Department Get(int id)
        {
            var dep = context.Depts.FirstOrDefault(d => d.DeptId == id);
            Department department = new Department();
            department.DepartmentId = dep.DeptId;
            department.DepartmentName = dep.DeptName;
            return department;
        }

        // POST api/departments
        public HttpResponseMessage Post([FromBody]Department dept)
        {
            try
            {

                DataLibrary.Dept department = new Dept();
                department.DeptId = dept.DepartmentId;
                department.DeptName = dept.DepartmentName;
                context.Depts.Add(department);
                context.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, dept);
                message.Headers.Location = new Uri(Request.RequestUri + dept.DepartmentId.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/departments/5
        public HttpResponseMessage Put([FromBody]Department dept)
        {
            try
            {

                DataLibrary.Dept department = new Dept();
                department.DeptId = dept.DepartmentId;
                department.DeptName = dept.DepartmentName;
                try
                {
                    var dep = context.Depts.FirstOrDefault(d => d.DeptId == department.DeptId);
                    dep.DeptName = department.DeptName;
                    context.SaveChanges();
                }
                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Department with Id = " + department.DeptId + " not found.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, dept);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/departments/5
        public HttpResponseMessage Delete([FromBody]int id)
        {
            try
            {
                try
                {
                    context.Depts.Remove(context.Depts.FirstOrDefault(d => d.DeptId == id));
                }
                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Department with Id = " + id + " not found.");
                }

                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
