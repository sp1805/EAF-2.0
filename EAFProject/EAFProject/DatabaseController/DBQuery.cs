using EAFProject.ViewModels;
using System;
using System.Configuration;

namespace EAFProject.DatabaseController
{
    public abstract class DBQuery
    {


        //For Request Details
        public static string GET_REQUEST_DETAILS_QUERY(int ReqId)
        {
            string Query =
                "Select Requests.ReqId, Requests.RequestType, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Emp.Name , Status.StatusValue, Requests.CreatedDate,Requests.LastModifiedDate, Requests.Criticality, Requests.CostEstimated,Requests.StatusId,CreatedByEmpId,(Select Emp.Name from Emp where swg = Requests.ReplacementFor),(Select Product.ProductName from Product where ProductId=Requests.FromProductId),(Select Emp.Name from Emp where SWG = Requests.FromManagerId),(Select Dept.DeptName from Dept where DeptId=Requests.FromDepartmentId),Requests.minCostEstimated,(Select Name from Emp where SWG=Requests.AssignedTo) from Requests inner join JobTitle on Requests.jobid = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Emp on Requests.CreatedByEmpId = Emp.swg inner join Status on Requests.StatusId = Status.StatusId  where Requests.ReqId = " + ReqId;
            return Query;
        }


        //For Displaying Essential,Desired and Optional Skills for Particular ReqId
        public static string GET_REQUESTED_SKILLS(int ReqId)
        {
            String Query = "Select RequestedSkills.SkillType, SkillInfo.SkillName from RequestedSkills inner join SkillInfo on RequestedSkills.SkillId = SkillInfo.SkillId where RequestedSkills.ReqId = " + ReqId;
            return Query;
        }

        //To know who has created the requests
        public static string GET_CREATED_BY_EMPLOYEEID_QUERY(int ReqId)
        {
            string MyQuery = "Select CreatedbyEmpId from Requests where Requests.ReqId = " + ReqId;
            return MyQuery;
        }

        //Display Requested Comments on Manager Requests
        public static string GET_REQUESTED_COMMENTS(int ReqId)
        {
            string Query = "Select Emp.Name,Comments, Datetime,EmpId,Status.StatusValue from RequestComments inner join EMp on RequestComments.EmpId= Emp.SWG inner join Status on RequestComments.statusid=Status.StatusId where ReqId = " + ReqId + " order by datetime";
            return Query;
        }

        //For Cancelling The requests
        public static string CANCEL_REQUEST(int ReqId)
        {
            string Query = "Update Requests set StatusId= 7 where reqId = " + ReqId;
            return Query;
        }

        //For Pending Requests for Managers
        public static string GET_PENDING_REQUESTS(LoginCredentials LG)
        {
            return "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + LG.swg + "'  UNION ALL SELECT child.SWG, child.name, parent.level + 1  FROM emp as child JOIN tree parent on parent.SWG = child.ManagerId	where exists(Select * from Emp Emp2 where Emp2.ManagerId=Child.SWG ))   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid) and Requests.CreatedByEmpId!='" + LG.swg + "'and Requests.StatusId in (1,2,4,5,8) and Requests.StatusId!=7 ORDER BY CONVERT(DateTime, Requests.lastmodifiedDate,101)  DESC";
        }

        //Display Request Assigned To Director
        public static string GET_REQUEST_QUERY(LoginCredentials LG)
        {
            string query = "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + LG.swg + "'  )   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid) and Requests.StatusId!=6 ORDER BY CONVERT(DateTime, Requests.lastmodifiedDate,101)  DESC";
            return query;

        }

        //Director Approval Query
        public static string GET_DIRECTOR_APPROVAL_QUERY(int ReqId)
        {
            string query = "Update Requests set statusId= 2 where ReqId = " + ReqId;
            return query;
        }

        //Director ONHOLD Query
        public static string GET_ONHOLD_QUERY(int ReqId)
        {
            string query = "Update Requests set statusId= 5 where ReqId = " + ReqId;
            return query;
        }

        //HR Processing Query
        public static string GET_HR_PROCESSING_QUERY(int ReqId)
        {
            string query = "Update Requests set statusId= 4 where ReqId = " + ReqId;
            return query;
        }


        //Get Manager Id Query
        public static string GET_DIRECTORID_QUERY(int ReqID)
        {
            string query = "Select Emp.ManagerId from Emp inner join Requests on Emp.SWG = Requests.CreatedbyEmpId where reqid = " + ReqID;
            return query;
        }
        //returning my team list
        //done
        public static string MyTeam_Query(string s)
        {
            string MyTeam = "select Emp.SWG,Emp.name,JobTitle.JobTitle,Dept.DeptName,Product.ProductName,Emp.ManagerId,Emp.DOJ,Emp.Email,Emp.DOR ,Emp.DOL,Emp.DOO from JobTitle inner join Emp on JobTitle.JobId=emp.JobId inner join Dept on Emp.DeptId=Dept.DeptId inner join  Product on Emp.ProductId=Product.ProductId where ManagerId ='" + s + "'" + "and (dol is null or dol = '1900-01-01 00:00:00.000' or dol >= getdate())";
            return MyTeam;
        }
        //returning my team details
        public static string MyTeamDtails_Query(string swg)
        {
            string MyTeam = "select Emp.SWG,Emp.name,JobTitle.JobTitle,Dept.DeptName,Product.ProductName,Emp.ManagerId,Emp.DOJ,Emp.Email,Emp.DOR ,Emp.DOL,Emp.DOO,JobTitle.JobId,Product.ProductId,Dept.DeptId from JobTitle inner join Emp on JobTitle.JobId=emp.JobId inner join Dept on Emp.DeptId=Dept.DeptId inner join  Product on Emp.ProductId=Product.ProductId where Emp.SWG='" + swg + "'" + " and (dol is null or dol = '1900-01-01 00:00:00.000' or dol >= getdate())";
            return MyTeam;
        }
        //Adding to my team
        public static string AddTeam_Query(EmployeeClass empl)
        {
            string sql = "INSERT INTO dbo.Emp (SWG,Name,jobId,DeptId,ProductId,ManagerId,DOJ,Email,DOR,DOL,DOO) VALUES " +
                                    " ('" + empl.swg + "','" + empl.name + "'," + empl.jobId + "," + empl.DeptId + "," + empl.ProductId + ",'" + empl.manager + "','" + empl.dateOfJoining + "','" + empl.email + "','" + empl.dateOfResignation + "','" + empl.lastDay + "','" + empl.dateOfOffer + "')";
            return sql;
        }
        ////returning Job list
        public static string GetJobTitle()
        {
            string sql = "select JobId, JobTitle from dbo.JobTitle";
            return sql;
        }
        //returning Product name
        public static string ProductNames()
        {
            string sql = "select ProductId, ProductName from dbo.Product";
            return sql;
        }
        //returning Department name
        public static string DeptNames()
        {
            string sql = "select DeptId, DeptName from dbo.Dept";
            return sql;
        }
        //updating team member after edit
        public static string UpdateTeam_Query(EmployeeClass empl, string MgrId)
        {
            string sql = "UPDATE dbo.Emp SET Name='" + empl.name + "',jobId=" + empl.jobId + ",DeptId=" + empl.DeptId + ",ProductId=" + empl.ProductId + ",ManagerId='" + empl.manager + "',DOJ='" + empl.dateOfJoining + "',Email='" + empl.email + "',DOR='" + empl.dateOfResignation + "',DOL='" + empl.lastDay + "',DOO='" + empl.dateOfOffer + "'" + "where (SWG='" + empl.swg + "' and managerid='" + MgrId + "')";
            return sql;
        }
        //Add Comments Query
        public static string GET_COMMENT_ADD_QUERY(int ReqId, string SwgId, string Comments, int p)
        {
            string query = "Insert into RequestComments Values(" + ReqId + ",'" + SwgId + "','" + Comments + "',GetDate()," + p + ")";
            return query;
        }
        public static string CREATE_NEW_REQUEST_QUERY(int ProductId, int JobId, int DeptId, int criticality, int EstimatedCTC, string ReqType, string CreatedByEmpId, DateTime CreatedDate, DateTime LastModifiedDate, int vacancies, string mineducation, int minexperience, int minEstimatedCTC)
        {
            string query = "INSERT INTO Requests (ProductId, jobid, DepartmentId, Criticality, CostEstimated, RequestType, CreatedByEmpId, CreatedDate, LastModifiedDate, vacancies, mineducation, StatusId,minexperience,minCostEstimated) Output Inserted.ReqId VALUES " + " (" + ProductId + "," + JobId + "," + DeptId + "," + criticality + "," + EstimatedCTC + ",'" + ReqType + "','" + CreatedByEmpId + "','" + CreatedDate + "','" + LastModifiedDate + "'," + vacancies + ",'" + mineducation + "'," + 1 + "," + minexperience + "," + minEstimatedCTC + ")";
            return query;
        }
        public static string GET_REQUEST_ID(string swg)
        {
            return "select MAX(ReqId) from Requests where CreatedByEmpId='" + swg + "'";
        }
        public static string INTERNAL_HIRE(int ProductId, int JobId, int DeptId, int criticality, int EstimatedCTC, string FromManagerId, int FromDeptId, string ReqType, int FromProductId, string CreatedByEmpId, DateTime CreatedDate, DateTime LastModifiedDate, int minEstimatedCTC)
        {
            return "INSERT INTO Requests (ProductId, jobid, DepartmentId, Criticality, CostEstimated, RequestType, CreatedByEmpId, CreatedDate, LastModifiedDate,FromDepartmentId,FromManagerId, StatusId,FromProductId,Vacancies,minCostEstimated) Output Inserted.ReqId VALUES " + " (" + ProductId + "," + JobId + "," + DeptId + "," + criticality + "," + EstimatedCTC + ",'" + ReqType + "','" + CreatedByEmpId + "','" + CreatedDate + "','" + LastModifiedDate + "'," + FromDeptId + ",'" + FromManagerId + "'," + 1 + "," + FromProductId + ",1," + minEstimatedCTC + ")";
        }
        public static string STORE_DESIRED_SKILLS(int ReqId, int skill)
        {
            return "insert into dbo.RequestedSkills(ReqId, SkillId, SkillType) values" + " (" +
                           ReqId + "," + skill + "," + "'Desired'" + ");";
        }
        public static string STORE_ESSENTIAL_SKILLS(int ReqId, int skill)
        {
            return "insert into dbo.RequestedSkills(ReqId, SkillId, SkillType) values" + " (" +
                           ReqId + "," + skill + "," + "'Essential'" + ");";
        }
        public static string STORE_OPTIONAL_SKILLS(int ReqId, int skill)
        {
            return "insert into dbo.RequestedSkills(ReqId, SkillId, SkillType) values" + " (" +
                           ReqId + "," + skill + "," + "'Optional'" + ");";
        }

        public static string RESUBMIT_REQUEST(int ReqId)
        {
            string Query = "Update Requests set StatusId= 8 where reqId = " + ReqId;
            return Query;
        }
        public static string UPDATE_LASTMODIFIEDDATE_REQUESTS(int ReqId)
        {
            return "Update Requests Set LastModifiedDate=GETDATE() where ReqId = " + ReqId;
        }

        //To send Mail to Manager
        public static string GET_EMAIL_APPROVAL_BY_MANAGER(string SWG)
        {
            string Query = "Select Email E1, (Select Email from Emp where SWG=(Select ManagerId from Emp where SWG = '" + SWG + "')) from Emp where SWG='" + SWG + "' ";
            return Query;
        }

        // To send Mail to Creator
        public static string SEND_EMAIL_TO_CREATOR(int ReqId, string swg)
        {
            return "Select Email E1, (Select Email from Emp where SWG=(Select CreatedByEmpId from Requests where ReqId=" + ReqId + ")) from Emp where SWG='" + swg + "' ";

        }

        //Query to Create Replacement Requests
        public static string CREATE_REPLACEMENT_REQUEST_QUERY(int ProductId, int JobId, int DeptId, int criticality, int EstimatedCTC, string mineducation, int minexperience, string ReqType, string CreatedByEmpId, DateTime CreatedDate, DateTime LastModifiedDate, string ReplacementFor, int minEstimatedCTC)
        {
            string query = "INSERT INTO Requests (ProductId, jobid, DepartmentId, Criticality, CostEstimated, RequestType, CreatedByEmpId, CreatedDate, LastModifiedDate, mineducation, StatusId,minexperience,ReplacementFor,Vacancies,minCostEstimated) Output Inserted.ReqId VALUES (" + ProductId + "," + JobId + "," + DeptId + "," + criticality + "," + EstimatedCTC + ",'" + ReqType + "','" + CreatedByEmpId + "','" + CreatedDate + "','" + LastModifiedDate + "','" + mineducation + "'," + 1 + ", '" + minexperience + "' ,'" + ReplacementFor + "',1," + minEstimatedCTC + ")";
            return query;
        }

        //Query to get Employee Names of all Employees under manager
        public static string EmployeeNames(string swgid)
        {
            string sql = "Select SWG,Name from Emp where ManagerId='" + swgid + "'";
            return sql;
        }

        //Query For All Manager Lists
        public static string GET_LIST_MANAGERS()
        {
            return "Select SWG, Name from Emp where Jobid in (Select JobId from JobTitle where JobTitle.JobTitle Like '%manager%' or JobTitle.JobTitle Like '%Chief%' or JobTitle.JobTitle Like '%Director%')";
        }

        //Query to get mail Id of all HRs
        public static string GET_HR_MAIL_LIST(string swgid)
        {
            return "Select Email ,(Select Email from Emp where swg = '" + swgid + "')from Emp where DeptId in (Select DeptId from Dept where DeptName Like '%HR%')";
        }

        public static string GET_SKILL_LISTS()
        {
            return "select SkillId, SkillName from dbo.SkillInfo";
        }
        public static string AddCandidate_Query(CandidateClass candidate)
        {
            string sql = "INSERT INTO dbo.Candidate (Name,PhoneNumber,Email,resume,interviewdatetime) Output Inserted.TempId VALUES " +
                                    " ('" + candidate.name + "','" + candidate.phoneNumber + "','" + candidate.email + "','" + candidate.FileName + "','" + candidate.CreatedDate + "')";
            return sql;
        }
        public static string Candidate_Query(int ReqId)
        {
            string sql = "select Name,PhoneNumber,Email,CandidateStatus,resume,TempId,interviewdatetime from Candidate inner join RequestedCandidates on Candidate.TempId=RequestedCandidates.CandidateId where ReqId=" + ReqId;
            return sql;
        }
        //insert mapping req and candidate
        public static string RequestedCandidatesQuery(int ReqId, int TempId)
        {
            string sql = "Insert into RequestedCandidates (ReqId,CandidateId) values" + "(" + ReqId + "," + TempId + ")";
            return sql;
        }
        public static string GetSkills(int ReqId)
        {
            String Query = "Select RequestedSkills.SkillType,SkillInfo.SkillId, SkillInfo.SkillName from RequestedSkills inner join SkillInfo on RequestedSkills.SkillId = SkillInfo.SkillId where RequestedSkills.ReqId =" + ReqId;
            return Query;
        }
        //Get interview id first interview
        public static string InterviewsQuery(ViewModels.Interview interview, string swg)
        {
            String Query = "Insert into Interviews (InterviewerId,status) Output Inserted.interviewId values('" + swg + "','" + interview.status + "')";
            return Query;
        }
        //SEt Interviewcomments 
        public static string InterviewcommentsQuery(int InterviewerId, string swg, InterviewComments interview)
        {
            String Query = "Insert into Interviewcomments (InterviewId,empid,comments,datetime,interviewstatus) values(" + InterviewerId + ",'" + swg + "','" + interview.Comments + "','" + interview.Datetime + "','" + interview.Status + "')";
            return Query;
        }
        //not needing with temp data
        public static string GetInterviewedIdQuery(int TempId, int ReqId)
        {
            String Query = "select InterviewId from CandidateMapping where (candiadteid =" + TempId + " and ReqId =" + ReqId + ")";
            return Query;
        }
        //
        public static string GetRatingQuery(int interviewId, int rating, int skill)
        {
            String Query = "Insert into ratings values(" + interviewId + "," + skill + "," + rating + ")";
            return Query;
        }
        public static string CandidateMappingQuery(int TempId, int ReqId, int interviewId)
        {
            string Query = "Insert into CandidateMapping values(" + TempId + "," + ReqId + "," + interviewId + ")";
            return Query;
        }
        // fill interview details in Interviewcomments
        public static string InterviewsQuerycomment(ViewModels.Interview interview, string swg)
        {
            String Query = "Insert into Interviewcomments (Interviewid,empid,comments,datetime,interviewstatus) values('" + "','" + interview.status + "')";
            return Query;
        }
        //update Interveiw table 
        public static string UPDATEInterviewsQuery(int interviewID, string swg, string status)
        {
            String Query = "UPDATE Interviews SET interviewerid='" + swg + "' , " + "status='" + status + "' WHERE interviewid=" + interviewID;
            return Query;
        }

        //CheckTeamMember Query
        public static string CheckTeamMemberQuery(string swg)
        {
            String Query = "select managerid from emp where swg='" + swg + "'";
            return Query;
        }

        //done
        public static string GET_ROLE_MANAGER(string swg)
        {
            return "select 'Manager' from emp where Jobid in (Select JobId from JobTitle where JobTitle.JobTitle Like '%manager%' or JobTitle.JobTitle Like '%Chief%' or JobTitle.JobTitle Like '%Director%') and SWG='" + swg + "'";
        }
        //done
        public static string GET_ROLE_HR(string swg)
        {
            string HRIDs = ConvertStringArrayToString.ConvertArraytoString(ConfigurationManager.AppSettings["HRDepartmentID"].ToString().Split(','));
            return "select 'HR' from emp where DeptId in (" + HRIDs + ") and SWG='" + swg + "'";
        }
        //done
        public static string GET_HR_REQUEST_QUERY(LoginCredentials LG)
        {
            string query = "SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId WHERE Requests.AssignedTo= '" + LG.swg +
                "' UNION SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join RequestComments on Requests.ReqId= RequestComments.ReqId WHERE RequestComments.EmpId in (Select SWG from emp where ManagerId is null) AND RequestComments.StatusId=2";
            return query;
        }

        public static string GET_ALL_REQUESTED_SKILLS()
        {
            return "Select RequestComments.ReqId,RequestComments.EmpId,Status.StatusValue from RequestComments inner join Status on RequestComments.StatusId = Status.StatusId";
        }

        //Populate HR Names
        public static string GET_HR_NAMES()
        {
            string HRIDs = ConvertStringArrayToString.ConvertArraytoString(ConfigurationManager.AppSettings["HRDepartmentID"].ToString().Split(','));
            return "Select SWG,Name from Emp where DeptId in (" + HRIDs + ")";
        }

        //Updating HR Name as Assigned to in Requests
        public static string UPDATE_ASSIGNED_HR_QUERY(int ReqId, string AssignHR)
        {

            return "Update Requests set AssignedTo = '" + AssignHR + "' where Reqid=" + ReqId;
        }

        public static string GET_SEARCH(string SearchBy, string SearchFor, string SWG)
        {
            return "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + SWG + "'  )   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid and Requests." + SearchBy + " = '" + SearchFor + "' )";
        }

        public static string GET_SEARCH_APPROVE(string SearchBy, string SearchFor, string SWG)
        {
            return "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + SWG + "'  )   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid and (RequestComments.statusId = 2 and RequestComments.EmpId='" + SearchFor + "'))";
        }

        public static string GET_SEARCH_PENDING(string SearchBy, string SearchFor, string SWG)
        {
            return "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + SWG + "'  UNION ALL SELECT child.SWG, child.name, parent.level + 1  FROM emp as child JOIN tree parent on parent.SWG = child.ManagerId	where exists(Select * from Emp Emp2 where Emp2.ManagerId=Child.SWG ))   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid) and Requests.CreatedByEmpId!='" + SWG + "' and Requests.StatusId in (1,2,5,8) and Requests." + SearchBy + "= '" + SearchFor + "'";
        }
        public static string GET_SEARCH_APPROVE_PENDING(string SearchBy, string SearchFor, string SWG)
        {
            return "WITH tree (SWG, name, level) AS  (SELECT SWG, name, 1 as level FROM emp WHERE SWG = '" + SWG + "'  UNION ALL SELECT child.SWG, child.name, parent.level + 1  FROM emp as child JOIN tree parent on parent.SWG = child.ManagerId	where exists(Select * from Emp Emp2 where Emp2.ManagerId=Child.SWG ))   Select Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId,Emp.Name From Requests Inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join tree on Requests.CreatedbyEmpId= tree.SWG  inner join RequestComments on Requests.ReqId= RequestComments.ReqId  inner join emp on RequestComments.EmpId = emp.Swg  where requestcomments.datetime=(select max(datetime) from requestcomments req2 where req2.reqid=requestcomments.reqid) and Requests.CreatedByEmpId!='" + SWG + "' and (RequestComments.statusId = 2 and RequestComments.EmpId='" + SearchFor + "')";
        }

        public static string GET_SEARCH_APPROVE_HR(string SearchBy, string SearchFor, string swg)
        {
            return "SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join RequestComments on Requests.Reqid=RequestComments.ReqId WHERE Requests.AssignedTo='" + swg + "' and (RequestComments.statusId = 2 and RequestComments.EmpId='" + SearchFor + "')" +
                "UNION SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join RequestComments on Requests.ReqId= RequestComments.ReqId WHERE RequestComments.EmpId in (Select SWG from emp where ManagerId is null) AND RequestComments.StatusId=2 and (RequestComments.statusId = 2 and RequestComments.EmpId='" + SearchFor + "')";
        }

        public static string GET_SEARCH_HR(string SearchBy, string SearchFor, string swg)
        {
            return "SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId WHERE Requests.AssignedTo='" + swg + "' and Requests." + SearchBy + "= '" + SearchFor + "'" +
                "UNION SELECT Requests.ReqId, JobTitle.JobTitle, Product.ProductName,Requests.Vacancies, Status.StatusValue, Requests.Criticality, Requests.StatusId FROM Requests inner Join JobTitle On Requests.JobId = JobTitle.JobId inner join Product on Requests.ProductID = Product.ProductID inner join Status on Requests.StatusId = Status.StatusId inner join RequestComments on Requests.ReqId= RequestComments.ReqId WHERE RequestComments.EmpId in (Select SWG from emp where ManagerId is null) AND RequestComments.StatusId=2 and Requests." + SearchBy + "= '" + SearchFor + "'";
        }
        public static string GET_NUMBER_OF_HIRED_CANDIDATE(int reqId)
        {
            return "Select Count(Candidate.CandidateStatus) from CandidateMapping inner join Candidate on CandidateMapping.CandiadteId = Candidate.TempId where CandidateMapping.ReqId= " + reqId + " and Candidate.CandidateStatus='hire'";
        }

        public static string GET_COMPLETE(int ReqId)
        {
            return "Update Requests set StatusId= 6 where reqId = " + ReqId;
        }
    }
}