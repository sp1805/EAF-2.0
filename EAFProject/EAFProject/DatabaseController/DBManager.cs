using EAFProject.BusinessComponents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EAFProject.ViewModels;


namespace EAFProject.DatabaseController
{
    public class DBManager
    {
        //Building SQL Connection
        SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        int i;

        //For Getting Data.This will return Data set
        public DataSet GetData(string sql)
        {
            DataSet dsResult = new DataSet();
            try
            {
                connection.Open();
                SqlDataAdapter dadapter = new SqlDataAdapter(sql, connection);
                dadapter.Fill(dsResult, "dsResultSet");
            }
            catch
            {
            }
            finally
            {

                if (connection != null)
                    connection.Close();
            }
            return dsResult;
        }
        //For storing data in Database
        public int setData(string sql)
        {

            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {

                if (connection != null)
                    connection.Close();
            }
            return i;
        }
        //For Setting Data with Output
        public int setDataScalar(string sql)
        {

            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                using (cmd)
                {
                    connection.Open();

                    int modified = (int)cmd.ExecuteScalar();

                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();

                    return modified;
                }
            }
            catch
            {
            }
            finally
            {

                if (connection != null)
                    connection.Close();
            }
            return 0;
        }
        //Request Display for Manager 
        public List<RequestData> GetMyRequests(LoginCredentials lg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_REQUEST_QUERY(lg);

            DataSet dsResult = GetData(query);

            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = lg.swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }

            return myRequests;
        }

        // For Request Details
        public List<RequestData> GetMyRequestsDetails(int ReqId, string swg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_REQUEST_DETAILS_QUERY(ReqId);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = swg;
                RDD.EssentialSkills = new List<string>();
                RDD.DesiredSkills = new List<string>();
                RDD.OptionalSkills = new List<string>();
                RDD.ReviewedBy = new List<string>();
                RDD.ReviewedStatus = new List<string>();
                RDD.LastReviewedDate = new List<DateTime>();
                RDD.ReviewedComments = new List<string>();
                RDD.ReviewedEmpId = new List<string>();
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.ReqType = dr[1].ToString();
                RDD.JobTitle = dr[2].ToString();
                RDD.ProductName = dr[3].ToString();
                RDD.vacancies = int.Parse(dr[4].ToString());
                RDD.CreatedByName = dr[5].ToString();
                RDD.CreatedDate = Convert.ToDateTime(dr[7].ToString());
                RDD.criticality = int.Parse(dr[9].ToString());
                RDD.EstimatedCTC = int.Parse(dr[10].ToString());
                RDD.Status = int.Parse(dr[11].ToString());
                RDD.CreatedByEmpId = dr[12].ToString();
                RDD.ReplacementFor = dr[13].ToString();
                RDD.FromProductName = dr[14].ToString();
                RDD.FromManagerName = dr[15].ToString();
                RDD.FromDeptName = dr[16].ToString();

                if (String.IsNullOrEmpty(dr[17].ToString()) == false)
                {
                    RDD.minEstimatedCTC = int.Parse(dr[17].ToString());
                }
                RDD.HRName = dr[18].ToString();
                //for Essential,Desired and Optional Skills
                string query1 = DBQuery.GET_REQUESTED_SKILLS(ReqId);
                DataSet dsResult1 = GetData(query1);
                foreach (DataRow dr1 in dsResult1.Tables["dsResultSet"].Rows)
                {
                    if (dr1[0].ToString().Equals("Essential"))
                    {
                        RDD.EssentialSkills.Add(dr1[1].ToString());
                    }
                    else if (dr1[0].ToString().Equals("Desired"))
                    {
                        RDD.DesiredSkills.Add(dr1[1].ToString());
                    }
                    else if (dr1[0].ToString().Equals("Optional"))
                    {
                        RDD.OptionalSkills.Add(dr1[1].ToString());

                    }
                }

                string query2 = DBQuery.GET_REQUESTED_COMMENTS(ReqId);
                DataSet dsResult2 = GetData(query2);
                foreach (DataRow dr2 in dsResult2.Tables["dsResultSet"].Rows)
                {
                    RDD.ReviewedBy.Add(dr2[0].ToString());
                    RDD.ReviewedComments.Add(dr2[1].ToString());
                    RDD.LastReviewedDate.Add(Convert.ToDateTime(dr2[2].ToString()));
                    RDD.ReviewedEmpId.Add(dr2[3].ToString());
                    RDD.ReviewedStatus.Add(dr2[4].ToString());
                }

                string query3 = DBQuery.GET_NUMBER_OF_HIRED_CANDIDATE(ReqId);

                RDD.hiredNumberCandidate = setDataScalar(query3);
                //TempData["hiredNumberCandidate"] = RDD.hiredNumberCandidate;
                myRequests.Add(RDD);
            }

            return myRequests;
        }

        // For Cancelling Request
        public void CancelRequest(int ReqId, string Comments, String SwgId)
        {
            string query3 = DBQuery.CANCEL_REQUEST(ReqId);
            DataSet dsResult3 = GetData(query3);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, SwgId, Comments, 7);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(ReqId);
            DataSet dsResult2 = GetData(query2);
            //Mail Portion
            //To Creator
            string body = EAFConstants.BODY_REQUEST_CANCEL(ReqId);
            string subject = EAFConstants.SUBJECT_REQUEST_CANCEL();
            string query4 = DBQuery.SEND_EMAIL_TO_CREATOR(ReqId, SwgId);
            DataSet dsResult4 = GetData(query4);
            string from = dsResult3.Tables[0].Rows[0][0].ToString();
            string to = dsResult3.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);
        }

        // Director Approval
        public void DirectorApprovalRequest(int Reqid, string Comments, string SwgId)
        {
            string query = DBQuery.GET_DIRECTOR_APPROVAL_QUERY(Reqid);
            DataSet dsResult = GetData(query);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(Reqid, SwgId, Comments, 2);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(Reqid);
            DataSet dsResult2 = GetData(query2);


            //Mail Portion
            //To Creator
            string body = EAFConstants.BODY_REQUEST_APPROVED(Reqid);
            string subject = EAFConstants.SUBJECT_REQUEST_APPROVED();
            string query3 = DBQuery.SEND_EMAIL_TO_CREATOR(Reqid, SwgId);
            DataSet dsResult3 = GetData(query3);
            string from = dsResult3.Tables[0].Rows[0][0].ToString();
            string to = dsResult3.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);

            //To Senior
            string subject01 = EAFConstants.SUBJECT_REQUEST_APPROVAL();
            string body01 = EAFConstants.BODY_REQUEST_APPROVAL(Reqid);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(SwgId);
            DataSet dsResult4 = GetData(query4);
            string from01 = dsResult4.Tables[0].Rows[0][0].ToString();
            string to01 = dsResult.Tables[0].Rows[0][1].ToString();
            if (!String.IsNullOrEmpty(to01))
            {
                EMail.sendmail(from01, to01, subject, body);
            }
            else
            {
                //ToHR
                string subject02 = EAFConstants.SUBJECT_REQUEST_NEED_PROCESS();
                string body02 = EAFConstants.BODY_REQUEST_NEED_PROCESS(Reqid);
                string to02 = ConfigurationManager.AppSettings["HREmailIds"].ToString();
                EMail.sendmail(from01, to01, subject01, body01);
            }
        }


        //On Hold Request
        public void OnHoldRequest(int ReqId, string Comments, string SwgId)
        {
            string Query = DBQuery.GET_ONHOLD_QUERY(ReqId);
            DataSet dsresult = GetData(Query);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, SwgId, Comments, 5);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(ReqId);
            DataSet dsResult2 = GetData(query2);

            //Mail Portion
            //To Creator
            string body = EAFConstants.BODY_REQUEST_ONHOLD(ReqId);
            string subject = EAFConstants.SUBJECT_REQUEST_ONHOLD();
            string query3 = DBQuery.SEND_EMAIL_TO_CREATOR(ReqId, SwgId);
            DataSet dsResult3 = GetData(query3);
            string from = dsResult3.Tables[0].Rows[0][0].ToString();
            string to = dsResult3.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);



        }

        public string CreatedManagerID(int ReqId)
        {
            string Query = DBQuery.GET_CREATED_BY_EMPLOYEEID_QUERY(ReqId);
            DataSet dsresult = GetData(Query);
            string ManagerId = dsresult.Tables[0].Rows[0][0].ToString();
            return ManagerId;
        }



        //For Displaying List of Team
        public List<EmployeeClass> GetMyTeam(string swg)
        {
            List<EmployeeClass> myTeamList = new List<EmployeeClass>();
            string query = DBQuery.MyTeam_Query(swg);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                EmployeeClass myTeam = new EmployeeClass();
                myTeam.swg = dr[0].ToString();
                myTeam.name = dr[1].ToString();
                myTeam.jobTitle = dr[2].ToString();
                myTeam.deptName = dr[3].ToString();
                myTeam.product = dr[4].ToString();
                myTeam.manager = dr[5].ToString();
                myTeam.dateOfJoining = Convert.ToDateTime(dr[6].ToString());
                myTeam.email = dr[7].ToString();
                if (dr[8] != DBNull.Value)
                {
                    myTeam.dateOfResignation = Convert.ToDateTime(dr[8].ToString());
                }
                if (dr[8] != DBNull.Value)
                {
                    myTeam.lastDay = Convert.ToDateTime(dr[9].ToString());
                }
                if (dr[8] != DBNull.Value)
                {
                    myTeam.dateOfOffer = Convert.ToDateTime(dr[10].ToString());
                }
                myTeamList.Add(myTeam);
            }
            return myTeamList;
        }

        //For Displaying TeamDetails
        public EmployeeClass GetMyTeamDetails(string s)
        {
            EmployeeClass myTeam = new EmployeeClass();
            string query = DBQuery.MyTeamDtails_Query(s);
            DataSet dsResult = GetData(query);
            myTeam.swg = dsResult.Tables[0].Rows[0][0].ToString();
            myTeam.name = dsResult.Tables[0].Rows[0][1].ToString();
            myTeam.jobTitle = dsResult.Tables[0].Rows[0][2].ToString();
            myTeam.deptName = dsResult.Tables[0].Rows[0][3].ToString();
            myTeam.product = dsResult.Tables[0].Rows[0][4].ToString();
            myTeam.manager = dsResult.Tables[0].Rows[0][5].ToString();
            myTeam.dateOfJoining = Convert.ToDateTime(dsResult.Tables[0].Rows[0][6].ToString());
            myTeam.email = dsResult.Tables[0].Rows[0][7].ToString();
            if ((dsResult.Tables[0].Rows[0][8]).ToString().Length != 0)
                if ((dsResult.Tables[0].Rows[0][8]) != DBNull.Value && (dsResult.Tables[0].Rows[0][8]).ToString() != "1/1/1900 12:00:00 AM")
                {
                    myTeam.dateOfResignation = Convert.ToDateTime(dsResult.Tables[0].Rows[0][8].ToString());
                }
            if ((dsResult.Tables[0].Rows[0][9]).ToString().Length != 0)
                if ((dsResult.Tables[0].Rows[0][9]) != DBNull.Value && (dsResult.Tables[0].Rows[0][9]).ToString() != "1/1/1900 12:00:00 AM")
                {
                    myTeam.lastDay = Convert.ToDateTime(dsResult.Tables[0].Rows[0][9].ToString());
                }
            if ((dsResult.Tables[0].Rows[0][10]).ToString().Length != 0)
                if ((dsResult.Tables[0].Rows[0][10]) != DBNull.Value && (dsResult.Tables[0].Rows[0][10]).ToString() != "1/1/1900 12:00:00 AM")
                {
                    myTeam.dateOfOffer = Convert.ToDateTime(dsResult.Tables[0].Rows[0][10].ToString());
                }
            myTeam.jobId = int.Parse(dsResult.Tables[0].Rows[0][11].ToString());
            myTeam.ProductId = int.Parse(dsResult.Tables[0].Rows[0][12].ToString());
            myTeam.DeptId = int.Parse(dsResult.Tables[0].Rows[0][13].ToString());

            return myTeam;

        }

        //Populating Job Titles
        public List<ViewModels.JobTitle> JobTitles()
        {
            List<ViewModels.JobTitle> theJobList = new List<ViewModels.JobTitle>();
            string query = DBQuery.GetJobTitle();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                ViewModels.JobTitle job = new ViewModels.JobTitle();
                job.JobID = int.Parse(dr[0].ToString());
                job.JobName = dr[1].ToString();
                theJobList.Add(job);
            }
            return theJobList;
        }

        //Populating Product Names
        public List<ProductName> ProductNames()
        {
            List<ProductName> theProductList = new List<ProductName>();
            string query = DBQuery.ProductNames();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                ProductName p = new ProductName();
                p.ProductId = int.Parse(dr[0].ToString());
                p.ProductTitle = dr[1].ToString();
                theProductList.Add(p);
            }
            return theProductList;
        }

        //Populating Department Names
        public List<DepartmentName> DeptNames()
        {
            List<DepartmentName> deptList = new List<DepartmentName>();
            string query = DBQuery.DeptNames();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                DepartmentName dept = new DepartmentName();
                dept.DeptId = int.Parse(dr[0].ToString());
                dept.DeptName = dr[1].ToString();
                deptList.Add(dept);
            }
            return deptList;
        }

        //For Adding a member in Team
        public int AddTeam(EmployeeClass e)
        {
            string sql = DBQuery.AddTeam_Query(e);
            setData(sql);
            return i;
        }

        //For Updating Team
        public int UpdateTeam(EmployeeClass e, string MgrId)
        {
            string sql = DBQuery.UpdateTeam_Query(e, MgrId);
            i = setData(sql);
            return i;
        }

        //For Resubmitting Request
        public void ResubmitRequest(int ReqId, string Comments, string swg)
        {
            string query3 = DBQuery.RESUBMIT_REQUEST(ReqId);
            DataSet dsResult3 = GetData(query3);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, swg, Comments, 8);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(ReqId);
            DataSet dsResult2 = GetData(query2);

            //Mailing Part
            //To Manager
            string subject = EAFConstants.SUBJECT_REQUEST_RESUBMIT();
            string body = EAFConstants.BODY_REQUEST_RESUBMIT(ReqId);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(swg);
            DataSet dsResult = GetData(query4);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string To = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, To, subject, body);

        }



        //Create New Request
        public int CreateNewRequest(RequestData rd)
        {
            string query = DBQuery.CREATE_NEW_REQUEST_QUERY(rd.ProductId, rd.JobId, rd.DeptId, rd.criticality, rd.EstimatedCTC, rd.ReqType, rd.CreatedByEmpId, rd.CreatedDate, rd.LastModifiedDate, rd.vacancies, rd.mineducation, rd.minexperience, rd.minEstimatedCTC);

            int ReqId = setDataScalar(query);
            //For Comments
            string query2 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, rd.CreatedByEmpId, rd.InputComments, 1);
            setData(query2);
            //For Skills
            foreach (var skillid_e in rd.EssentialSkillIds)
            {
                string query3 = DBQuery.STORE_ESSENTIAL_SKILLS(ReqId, skillid_e);
                setData(query3);
            }
            foreach (var skillid_d in rd.DesiredSkillIds)
            {
                string query3 = DBQuery.STORE_DESIRED_SKILLS(ReqId, skillid_d);
                setData(query3);
            }
            foreach (var skillid_o in rd.OptionalSkillIds)
            {
                string query3 = DBQuery.STORE_OPTIONAL_SKILLS(ReqId, skillid_o);
                setData(query3);
            }
            //Mailing Part
            //To Manager
            string subject = EAFConstants.SUBJECT_REQUEST_APPROVAL();
            string body = EAFConstants.BODY_REQUEST_APPROVAL(ReqId);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(rd.CreatedByEmpId);
            DataSet dsResult = GetData(query4);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string to = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);

            //ToHR
            string subject01 = EAFConstants.SUBJECT_REQUEST_CREATED();
            string body01 = EAFConstants.BODY_REQUEST_CREATED(ReqId);
            string to01 = ConfigurationManager.AppSettings["HREmailIds"].ToString();
            EMail.sendmail(from, to01, subject01, body01);



            return ReqId;

        }

        //Create Replacement Request
        public int CreateReplacementRequest(RequestData rd)
        {
            string query = DBQuery.CREATE_REPLACEMENT_REQUEST_QUERY(rd.ProductId, rd.JobId, rd.DeptId, rd.criticality, rd.EstimatedCTC, rd.mineducation, rd.minexperience, rd.ReqType, rd.CreatedByEmpId, rd.CreatedDate, rd.LastModifiedDate, rd.ReplacementFor, rd.minEstimatedCTC);

            int ReqId = setDataScalar(query);
            //For Comments
            string query2 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, rd.CreatedByEmpId, rd.InputComments, 1);
            setData(query2);
            //For Skills
            foreach (var skillid_e in rd.EssentialSkillIds)
            {
                string query3 = DBQuery.STORE_ESSENTIAL_SKILLS(ReqId, skillid_e);
                setData(query3);
            }
            foreach (var skillid_d in rd.DesiredSkillIds)
            {
                string query3 = DBQuery.STORE_DESIRED_SKILLS(ReqId, skillid_d);
                setData(query3);
            }
            foreach (var skillid_o in rd.OptionalSkillIds)
            {
                string query3 = DBQuery.STORE_OPTIONAL_SKILLS(ReqId, skillid_o);
                setData(query3);
            }
            //Mailing Part
            //To Manager
            string subject = EAFConstants.SUBJECT_REQUEST_APPROVAL();
            string body = EAFConstants.BODY_REQUEST_APPROVAL(ReqId);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(rd.CreatedByEmpId);
            DataSet dsResult = GetData(query4);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string To = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, To, subject, body);

            //ToHR
            string subject01 = EAFConstants.SUBJECT_REQUEST_CREATED();
            string body01 = EAFConstants.BODY_REQUEST_CREATED(ReqId);
            string to01 = ConfigurationManager.AppSettings["HREmailIds"].ToString();
            EMail.sendmail(from, to01, subject01, body01);
            return ReqId;
        }

        //Create Internal Hire
        public int CreateInternalHire(RequestData rd)
        {
            string query = DBQuery.INTERNAL_HIRE(rd.ProductId, rd.JobId, rd.DeptId, rd.criticality, rd.EstimatedCTC, rd.FromManagerId, rd.FromDeptId, rd.ReqType, rd.FromProductId, rd.CreatedByEmpId, rd.CreatedDate, rd.LastModifiedDate, rd.minEstimatedCTC);
            int ReqId = setDataScalar(query);
            //For Comments
            string query2 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, rd.CreatedByEmpId, rd.InputComments, 1);
            setData(query2);
            //For Skills
            foreach (var skillid_e in rd.EssentialSkillIds)
            {
                string query3 = DBQuery.STORE_ESSENTIAL_SKILLS(ReqId, skillid_e);
                setData(query3);
            }
            foreach (var skillid_d in rd.DesiredSkillIds)
            {
                string query3 = DBQuery.STORE_DESIRED_SKILLS(ReqId, skillid_d);
                setData(query3);
            }
            foreach (var skillid_o in rd.OptionalSkillIds)
            {
                string query3 = DBQuery.STORE_OPTIONAL_SKILLS(ReqId, skillid_o);
                setData(query3);
            }
            //Mailing Part
            //To Manager
            string subject = EAFConstants.SUBJECT_REQUEST_APPROVAL();
            string body = EAFConstants.BODY_REQUEST_APPROVAL(ReqId);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(rd.CreatedByEmpId);
            DataSet dsResult = GetData(query4);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string To = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, To, subject, body);

            //ToHR
            string subject01 = EAFConstants.SUBJECT_REQUEST_CREATED();
            string body01 = EAFConstants.BODY_REQUEST_CREATED(ReqId);
            string to01 = ConfigurationManager.AppSettings["HREmailIds"].ToString();
            EMail.sendmail(from, to01, subject01, body01);
            return ReqId;

        }

        //Populating List of Employees for a Manager
        public List<EmployeeName> EmployeeNames(string swgid)
        {
            List<EmployeeName> theEmployeeList = new List<EmployeeName>();
            string query = DBQuery.EmployeeNames(swgid);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                EmployeeName E = new EmployeeName();
                E.EmpId = dr[0].ToString();
                E.EmpName = dr[1].ToString();
                theEmployeeList.Add(E);
            }
            return theEmployeeList;
        }

        //Populating Managers List
        public List<EmployeeName> ManagerLists()
        {
            List<EmployeeName> theEmployeeList = new List<EmployeeName>();
            string query = DBQuery.GET_LIST_MANAGERS();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                EmployeeName E = new EmployeeName();
                E.EmpId = dr[0].ToString();
                E.EmpName = dr[1].ToString();
                theEmployeeList.Add(E);
            }
            return theEmployeeList;
        }


        //Populating Getting Skills Name
        public List<ViewModels.Interview> GetSkills(int ReqId, int Id_Interviewed)
        {

            string query = DBQuery.GetSkills(ReqId);
            List<ViewModels.Interview> skillsList = new List<ViewModels.Interview>();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                ViewModels.Interview skill = new ViewModels.Interview();
                skill.SkillType = dr[0].ToString();
                skill.SkillId = int.Parse(dr[1].ToString());
                if (Id_Interviewed != 0)
                {
                    string sql = "select ratings from ratings where (interviewid=" + Id_Interviewed + " and skillid=" + skill.SkillId + ")";
                    DataSet dsResult1 = GetData(sql);
                    skill.Rating = int.Parse(dsResult1.Tables[0].Rows[0][0].ToString());
                }
                skill.SkillName = dr[2].ToString();
                skillsList.Add(skill);
            }
            return skillsList;
        }
        //Get InterviewedId if already iterviewed
        public int GetInterviewedId(int TempId, int ReqId)
        {
            int interId;
            string query = DBQuery.GetInterviewedIdQuery(TempId, ReqId);
            DataSet dsResult = GetData(query);
            if (dsResult.Tables["dsResultSet"].Rows.Count == 0)
            {
                interId = 0;
            }
            else
            {
                interId = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
            }
            return interId;
        }
        //returns the list of skills
        public List<SkillsTitle> Skills()
        {
            List<SkillsTitle> theSkillsTitle = new List<SkillsTitle>();
            string query = DBQuery.GET_SKILL_LISTS();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                SkillsTitle S = new SkillsTitle();
                S.SkillId = int.Parse(dr[0].ToString());
                S.SkillName = dr[1].ToString();
                theSkillsTitle.Add(S);
            }
            return theSkillsTitle;

        }
        //Adding candidate in my Team
        public int AddCandidate(CandidateClass c)
        {
            string query = DBQuery.AddCandidate_Query(c);
            int i = setDataScalar(query);
            return i;
        }
        //get candidate details
        public List<CandidateClass> GetCandidates(int ReqId)
        {

            string query = DBQuery.Candidate_Query(ReqId);
            List<CandidateClass> candidateList = new List<CandidateClass>();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                CandidateClass candidate = new CandidateClass();
                candidate.name = dr[0].ToString();
                candidate.phoneNumber = long.Parse(dr[1].ToString());
                candidate.email = dr[2].ToString();
                candidate.candidateStatus = dr[3].ToString();
                candidate.FileName = dr[4].ToString();
                candidate.TempId = int.Parse(dr[5].ToString());
                candidate.CreatedDate = Convert.ToDateTime(dr[6].ToString());
                candidateList.Add(candidate);
            }
            return candidateList;
        }
        //
        public int RequestedCandidates(int ReqId, int TempId)
        {
            int i;
            string query = DBQuery.RequestedCandidatesQuery(ReqId, TempId);
            i = setData(query);
            string swg = System.Web.HttpContext.Current.Session["swgId"].ToString();
            string subject = EAFConstants.SUBJECT_CANDIDATE_ADD();
            string body = EAFConstants.BODY_CANDIDATE_ADD(TempId, ReqId);
            string query1 = DBQuery.SEND_EMAIL_TO_CREATOR(ReqId, swg);
            DataSet dsResult = GetData(query1);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string to = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);
            return i;
        }
        //public int ReturnReqId(int TempId)
        //{
        //    int ReqId;
        //    string sql;
        //    sql = DBQuery.ReqIdQuery(TempId);
        //    DataSet dsResult = GetData(sql);
        //    ReqId = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
        //    return ReqId;
        //}

        //For Getting Pending Requests for Senior Managers
        public List<RequestData> GetPendingRequests(LoginCredentials lg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_PENDING_REQUESTS(lg);

            DataSet dsResult = GetData(query);

            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = lg.swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }

            return myRequests;
        }
        //set interview first time
        public int Interviews(string swg, ViewModels.Interview obj)
        {
            int interId;
            string query = DBQuery.InterviewsQuery(obj, swg);
            interId = setDataScalar(query);
            return interId;
        }
        //set Interviewcomments
        public int Interviewcomments(int InterviewerId, string swg, InterviewComments interview, int TempId, int ReqId)
        {
            string query = DBQuery.InterviewcommentsQuery(InterviewerId, swg, interview);
            i = setData(query);
            if (interview.Status == "Hire")
            {
                string subject = EAFConstants.SUBJECT_CANDIDATE_HIRE();
                string body = EAFConstants.BODY_CANDIDATE_HIRE(TempId, ReqId);
                string query1 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(swg);
                DataSet dsresult = GetData(query1);
                string from = dsresult.Tables[0].Rows[0][0].ToString();
                string to = dsresult.Tables[0].Rows[0][1].ToString();
                if (!String.IsNullOrEmpty(to))
                {
                    EMail.sendmail(from, to, subject, body);
                }
                else
                {

                }
            }
            return i;
        }
        //Storing rating of skills
        public int SetRating(int interviewId, int rating, int skill)
        {
            string sql = DBQuery.GetRatingQuery(interviewId, rating, skill);
            i = setData(sql);
            return i;
        }
        //stroing CandidateMapping
        public int SetCandidateMapping(int TempId, int ReqId, int interviewId)
        {
            string query = DBQuery.CandidateMappingQuery(TempId, ReqId, interviewId);
            int i = setData(query);
            return i;
        }
        //geting name of manager 
        public string ManagerName(int Id_Interviewed)
        {
            string sql = "select emp.Name from emp inner join Interviews on emp.swg=Interviews.interviewerid where Interviews.interviewid=" + Id_Interviewed;
            string name;
            DataSet dsResult1 = GetData(sql);
            name = (dsResult1.Tables[0].Rows[0][0].ToString());
            return name;
        }
        //get all Comments  from Interviewcomments
        public List<InterviewComments> GetInterViewComments(int Id_Interviewed)
        {
            string sql = "select Empid,comments,datetime,interviewstatus from Interviewcomments where interviewid=" + Id_Interviewed;
            List<InterviewComments> interviewList = new List<InterviewComments>();
            DataSet dsResult = GetData(sql);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                InterviewComments interview = new InterviewComments();
                interview.InterviewerId = dr[0].ToString();
                string sql1 = "select name from emp where swg=" + "'" + dr[0].ToString() + "'";
                DataSet dsResult1 = GetData(sql1);
                interview.InterviewerName = (dsResult1.Tables[0].Rows[0][0].ToString());
                interview.Comments = dr[1].ToString();
                interview.Datetime = Convert.ToDateTime(dr[2].ToString());
                interview.Status = dr[3].ToString();
                interviewList.Add(interview);
            }
            return interviewList;
        }
        //get the name of candiate 
        public string CandidateName(int TempId)
        {
            string sql = "select name from Candidate where TempId=" + TempId;
            string name;
            DataSet dsResult1 = GetData(sql);
            name = (dsResult1.Tables[0].Rows[0][0].ToString());
            return name;
        }
        //update interview tabl after interview
        public int UPDATEInterviews(int interviewID, string swg, string status)
        {
            string query = DBQuery.UPDATEInterviewsQuery(interviewID, swg, status);
            int i = setData(query);
            return i;
        }
        //UpdateCommentCandidate
        public int UpdateCommentCandidate(string Comment, int Tempid)
        {
            string query = "UPDATE Candidate SET CandidateStatus='" + Comment + "' where tempid=" + Tempid;
            int i = setData(query);
            return i;
        }
        //Check Team member already exists
        public string CheckTeamMember(string swgTeam)
        {
            string MgrId;
            string query = DBQuery.CheckTeamMemberQuery(swgTeam);
            DataSet dsResult = GetData(query);
            if (dsResult.Tables["dsResultSet"].Rows.Count == 0)
            {
                MgrId = null;
            }
            else
            {
                MgrId = dsResult.Tables[0].Rows[0][0].ToString();
            }
            return MgrId;
        }
        //getting name of candidate
        public string GetName(string swg)
        {
            string sql1 = "select name from emp where swg=" + "'" + swg + "'";
            DataSet dsResult1 = GetData(sql1);
            string name = (dsResult1.Tables[0].Rows[0][0].ToString());
            return name;
        }
        //For Login Controller used to distinguish between HR and Manager
        public string DepartmentId(string swg)
        {
            string query = DBQuery.GET_ROLE_MANAGER(swg);
            DataSet dsResult = GetData(query);
            if (dsResult.Tables[0].Rows.Count == 0)
            {
                query = DBQuery.GET_ROLE_HR(swg);
                dsResult = GetData(query);
                if (dsResult.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                return dsResult.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return dsResult.Tables[0].Rows[0][0].ToString();
            }

        }

        //For HR Requests
        public List<RequestData> GetHRRequest(LoginCredentials LG)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_HR_REQUEST_QUERY(LG);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = LG.swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                myRequests.Add(RDD);
            }
            return myRequests;
        }

        //Populating names of all HRs
        public List<EmployeeName> GetHRList()
        {
            List<EmployeeName> theEmployeeList = new List<EmployeeName>();
            string query = DBQuery.GET_HR_NAMES();
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                EmployeeName E = new EmployeeName();
                E.EmpId = dr[0].ToString();
                E.EmpName = dr[1].ToString();
                theEmployeeList.Add(E);
            }
            return theEmployeeList;
        }

        //For Processing Requests
        public void Processing(int ReqId, string Comments, string swg, string AssignHR)
        {
            string query = DBQuery.GET_HR_PROCESSING_QUERY(ReqId);
            DataSet dsResult = GetData(query);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, swg, Comments, 4);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(ReqId);
            DataSet dsResult2 = GetData(query2);
            string query3 = DBQuery.UPDATE_ASSIGNED_HR_QUERY(ReqId, AssignHR);
            DataSet dsResult3 = GetData(query3);
            //Mail Portion
            //To Creator
            string body = EAFConstants.BODY_REQUEST_PROCESSING(ReqId);
            string subject = EAFConstants.SUBJECT_REQUEST_PROCESSING();
            string query4 = DBQuery.SEND_EMAIL_TO_CREATOR(ReqId, AssignHR);
            DataSet dsResult4 = GetData(query4);
            string from = dsResult4.Tables[0].Rows[0][0].ToString();
            string to = dsResult4.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, to, subject, body);
        }



        public List<RequestData> Search(string SearchBy, string SearchFor, string swg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            if (SearchBy == "status")
            {
                SearchBy = "statusid";
            }
            string query = DBQuery.GET_SEARCH(SearchBy, SearchFor, swg);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }
            return myRequests;
        }

        public List<RequestData> SearchApprove(string SearchBy, string SearchFor, string SWG)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_SEARCH_APPROVE(SearchBy, SearchFor, SWG);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = SWG;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }
            return myRequests;
        }

        public List<RequestData> SearchPending(string SearchBy, string SearchFor, string swg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            if (SearchBy == "status")
            {
                SearchBy = "statusid";
            }
            string query = DBQuery.GET_SEARCH_PENDING(SearchBy, SearchFor, swg);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }
            return myRequests;
        }
        public List<RequestData> SearchApprovePending(string SearchBy, string SearchFor, string SWG)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_SEARCH_APPROVE_PENDING(SearchBy, SearchFor, SWG);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = SWG;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                RDD.LastReviewedBy = dr[7].ToString();
                myRequests.Add(RDD);
            }
            return myRequests;
        }

        public List<RequestData> SearchApproveHR(string SearchBy, string SearchFor, string swg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            if (SearchBy == "status")
            {
                SearchBy = "statusid";
            }
            string query = DBQuery.GET_SEARCH_APPROVE_HR(SearchBy, SearchFor, swg);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());
                myRequests.Add(RDD);
            }
            return myRequests;
        }

        public List<RequestData> SearchHR(string SearchBy, string SearchFor, string swg)
        {
            List<RequestData> myRequests = new List<RequestData>();
            string query = DBQuery.GET_SEARCH_HR(SearchBy, SearchFor, swg);
            DataSet dsResult = GetData(query);
            foreach (DataRow dr in dsResult.Tables["dsResultSet"].Rows)
            {
                RequestData RDD = new RequestData();
                RDD.SWGId = swg;
                RDD.ReqId = int.Parse(dr[0].ToString());
                RDD.JobTitle = dr[1].ToString();
                RDD.ProductName = dr[2].ToString();
                RDD.vacancies = int.Parse(dr[3].ToString());
                RDD.StatusValue = dr[4].ToString();
                RDD.criticality = int.Parse(dr[5].ToString());
                if (RDD.criticality == 1)
                {
                    RDD.CriticalityName = "Normal";
                }
                else if (RDD.criticality == 2)
                {
                    RDD.CriticalityName = "Urgent";
                }
                RDD.Status = int.Parse(dr[6].ToString());

                myRequests.Add(RDD);
            }
            return myRequests;
        }
        public int UpdateMgrSwg(string MgrId, string swg, int jobId, int depId, int productId)
        {
            string query = "UPDATE emp SET managerid='" + MgrId + "', jobId = " + jobId + ", DeptId = " + depId + ", ProductId = " + productId + " where swg='" + swg + "'";
            int i = setData(query);
            return i;
        }

        public void Complete(int ReqId, string Comments, string swgId)
        {
            string Query = DBQuery.GET_COMPLETE(ReqId);
            DataSet dsresult = GetData(Query);
            string query1 = DBQuery.GET_COMMENT_ADD_QUERY(ReqId, swgId, Comments, 6);
            DataSet dsResult1 = GetData(query1);
            string query2 = DBQuery.UPDATE_LASTMODIFIEDDATE_REQUESTS(ReqId);
            DataSet dsResult2 = GetData(query2);
            //Mailing Part
            //To Manager
            string subject = EAFConstants.SUBJECT_REQUEST_COMPLETED();
            string body = EAFConstants.BODY_REQUEST_COMPLETED(ReqId);
            string query4 = DBQuery.GET_EMAIL_APPROVAL_BY_MANAGER(swgId);
            DataSet dsResult = GetData(query4);
            string from = dsResult.Tables[0].Rows[0][0].ToString();
            string To = dsResult.Tables[0].Rows[0][1].ToString();
            EMail.sendmail(from, To, subject, body);
            //ToHR
            string to01 = ConfigurationManager.AppSettings["HREmailIds"].ToString();
            EMail.sendmail(from, to01, subject, body);
        }
    }
}