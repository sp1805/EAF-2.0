namespace EAFProject.ViewModels
{
    public abstract class EAFConstants
    {
        public static string BODY_REQUEST_CREATED(int reqId)
        {
            return "A request with Requisition Id: " + reqId + " has been created";
        }
        public static string SUBJECT_REQUEST_CREATED()
        {
            return "Request created";
        }

        public static string BODY_REQUEST_CANCEL(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " has been cancelled";
        }
        public static string SUBJECT_REQUEST_CANCEL()
        {
            return "Request Cancelled";
        }

        public static string BODY_REQUEST_APPROVAL(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " needs your approval";
        }
        public static string SUBJECT_REQUEST_APPROVAL()
        {
            return "Request Approval";
        }

        public static string BODY_REQUEST_APPROVED(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " has been approved";
        }
        public static string SUBJECT_REQUEST_APPROVED()
        {
            return "Request Approval";
        }

        public static string BODY_REQUEST_NEED_PROCESS(int reqId)
        {
            return "The Requisition Id: " + reqId + " has been approved and now needs to be processed by HR";
        }
        public static string SUBJECT_REQUEST_NEED_PROCESS()
        {
            return "Request needs HR Processing";
        }


        public static string BODY_REQUEST_ONHOLD(int reqId)
        {
            return "The Requisition Id: " + reqId + " has been kept on hold";
        }
        public static string SUBJECT_REQUEST_ONHOLD()
        {
            return "Request on hold";
        }

        public static string BODY_REQUEST_RESUBMIT(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " has been resubmitted for your approval";
        }
        public static string SUBJECT_REQUEST_RESUBMIT()
        {
            return "Request Resubmitted";
        }

        public static string BODY_REQUEST_PROCESSING(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " is under HR Process";
        }
        public static string SUBJECT_REQUEST_PROCESSING()
        {
            return "Request under HR Processing";
        }

        public static string BODY_REQUEST_COMPLETED(int reqId)
        {
            return "A request with Requisition Id : " + reqId + " has been completed and closed";
        }
        public static string SUBJECT_REQUEST_COMPLETED()
        {
            return "Request completed";
        }

        public static string BODY_CANDIDATE_ADD(int tempId, int reqId)
        {
            return "A candidate with id: " + tempId + " has been added for Request Id: " + reqId;
        }
        public static string SUBJECT_CANDIDATE_ADD()
        {
            return "Candidate added";
        }

        public static string BODY_SUBMITFEEDBACK(int tempId, int reqId)
        {
            return "Feedback form has been completed for Candidate Id: " + tempId + " for Request Id:" + reqId;
        }
        public static string SUBJECT_SUBMITFEEDBACK()
        {
            return "Feedback submitted";
        }

        public static string BODY_CANDIDATE_HIRE(int tempId, int reqId)
        {
            return "A candidate with id: " + tempId + " has been hired for Request Id: " + reqId;
        }
        public static string SUBJECT_CANDIDATE_HIRE()
        {
            return "Candidate hired";
        }
    }
}