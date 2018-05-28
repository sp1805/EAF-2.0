using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EAFProject.ViewModels;


namespace EAFProject.BusinessComponents
{
    public class Request
    {
        //Display All Requests
        public static List<RequestData> GetMyRequest(LoginCredentials LG)
        {
            var data = MvcApplication.DatabaseHandler.GetMyRequests(LG);
            return data;
        }

        //Display Employee Names
        public static List<EmployeeName> EmployeeNames(string swgid)
        {
            var data = MvcApplication.DatabaseHandler.EmployeeNames(swgid);
            return data;
        }

        //To Diaplay All Manager Lists
        public static List<EmployeeName> ManagerLists()
        {
            var data = MvcApplication.DatabaseHandler.ManagerLists();
            return data;
        }

        public static List<RequestData> GetMyRequestDetails(int ReqId, string swg)
        {
            var data = MvcApplication.DatabaseHandler.GetMyRequestsDetails(ReqId, swg);
            return data;
        }
        public static void CancelRequest(int ReqId, string comments, string SwgId)
        {
            MvcApplication.DatabaseHandler.CancelRequest(ReqId, comments, SwgId);
        }
        public static void DirectorApprovalRequest(int ReqId, string Comments, string swgId)
        {
            MvcApplication.DatabaseHandler.DirectorApprovalRequest(ReqId, Comments, swgId);
        }

        public static void OnHoldRequest(int ReqId, string Comments, string swgId)
        {
            MvcApplication.DatabaseHandler.OnHoldRequest(ReqId, Comments, swgId);
        }
        
        public static string CreatedManagerID(int ReqId)
        {
            string ManagerId = MvcApplication.DatabaseHandler.CreatedManagerID(ReqId);
            return ManagerId;
        }
        public static void ResubmitRequest(int ReqId, string Comments, string swg)
        {
            MvcApplication.DatabaseHandler.ResubmitRequest(ReqId, Comments, swg);
        }
        
        
        public static int CreateNewRequest(RequestData rd)
        {
            int i = MvcApplication.DatabaseHandler.CreateNewRequest(rd);
            return i;
        }
        public static int CreateInternalHire(RequestData Rdd)
        {
            int i = MvcApplication.DatabaseHandler.CreateInternalHire(Rdd);
            return i;
        }

        public static int CreateReplacementRequest(RequestData c)
        {
            int i = MvcApplication.DatabaseHandler.CreateReplacementRequest(c);
            return i;
        }

        public static List<ViewModels.JobTitle> JobTitles()
        {
            var data = MvcApplication.DatabaseHandler.JobTitles();
            return data;
        }

        public static List<ProductName> ProductNames()
        {
            var data = MvcApplication.DatabaseHandler.ProductNames();
            return data;
        }

        public static List<DepartmentName> DeptNames()
        {
            var data = MvcApplication.DatabaseHandler.DeptNames();
            return data;
        }

        public static List<SkillsTitle> Skills()
        {
            var data = MvcApplication.DatabaseHandler.Skills();
            return data;
        }

        public static List<RequestData> GetPendingRequest(LoginCredentials LG)
        {
            var data = MvcApplication.DatabaseHandler.GetPendingRequests(LG);
            return data;
        }

        public static string DepartmentId(string swg)
        {
            return MvcApplication.DatabaseHandler.DepartmentId(swg);
        }

        public static List<RequestData> GetHRRequest(LoginCredentials LG)
        {
            var data = MvcApplication.DatabaseHandler.GetHRRequest(LG);
            return data;
        }

        public static List<EmployeeName> GetHRList()
        {
            var data = MvcApplication.DatabaseHandler.GetHRList();
            return data;
        }

        public static void Processing(int ReqId, string Comments, string swg, string AssignHR)
        {
            MvcApplication.DatabaseHandler.Processing(ReqId, Comments, swg, AssignHR);
        }

        public static List<RequestData> Search(string SearchBy, string SearchFor, string SWG)
        {
            var data = MvcApplication.DatabaseHandler.Search(SearchBy, SearchFor, SWG);
            return data;
        }

        public static List<RequestData> SearchApprove(string SearchBy, string SearchFor, string SWG)
        {
            var data = MvcApplication.DatabaseHandler.SearchApprove(SearchBy, SearchFor, SWG);
            return data;
        }

        public static object SearchApprovePending(string SearchBy, string SearchFor, string SWG)
        {
            var data = MvcApplication.DatabaseHandler.SearchApprovePending(SearchBy, SearchFor, SWG);
            return data;
        }
        public static List<RequestData> SearchPending(string SearchBy, string SearchFor, string SWG)
        {
            var data = MvcApplication.DatabaseHandler.SearchPending(SearchBy, SearchFor, SWG);
            return data;
        }

        public static List<RequestData> SearchApproveHR(string SearchBy, string SearchFor, string swg)
        {
            var data = MvcApplication.DatabaseHandler.SearchApproveHR(SearchBy, SearchFor, swg);
            return data;
        }

        public static List<RequestData> SearchHR(string SearchBy, string SearchFor, string swg)
        {
            var data = MvcApplication.DatabaseHandler.SearchHR(SearchBy, SearchFor, swg);
            return data;
        }

        public static void Complete(int ReqId, string Comments, string swgId)
        {
            MvcApplication.DatabaseHandler.Complete(ReqId, Comments, swgId);
        }
    }
}