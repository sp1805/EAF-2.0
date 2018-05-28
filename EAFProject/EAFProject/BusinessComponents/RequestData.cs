using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EAFProject.BusinessComponents
{
    public class RequestData
    {
        public string SWGId { get; set; }
        public int ReqId { get; set; }
        public string ReqType { get; set; }
        [Required(ErrorMessage = "Job Title required.")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Job Title required.")]
        public int JobId { get; set; }
        [Required(ErrorMessage = "Product Name required.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Department Name required.")]
        public string DeptName { get; set; }
        public string Skills { get; set; }
        [Range(0, 4)]
        [Required(ErrorMessage = "Criticality required.")]
        public int criticality { get; set; }
        [Required(ErrorMessage = "Criticality required.")]
        public string CriticalityName { get; set; }
        [Required(ErrorMessage = "Vacancies required.")]
        [Display(Name = "vacancies")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int vacancies { get; set; }
        public string StatusValue { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmpId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }
        public string HRName { get; set; }

        [Required(ErrorMessage = "Maximum CTC required.")]
        [Range(1, int.MaxValue)]
        public int EstimatedCTC { get; set; }
        [Required(ErrorMessage = "Minimum CTC required.")]
        [Range(1, int.MaxValue)]
        public int minEstimatedCTC { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [Required(ErrorMessage = "Minimum experience required.")]
        public int minexperience { get; set; }
        [Required(ErrorMessage = "Minimum education required.")]
        public string mineducation { get; set; }
        public string LastReviewedBy { get; set; }
        public List<DateTime> LastReviewedDate { get; set; }
        [Required(ErrorMessage = "Product Name required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Department Name required.")]
        public int DeptId { get; set; }
        [Required(ErrorMessage = "One or more values of Essential Skills required")]
        public List<int> EssentialSkillIds;
        public List<int> DesiredSkillIds;
        public List<int> OptionalSkillIds;
        [Required(ErrorMessage = "Choose From Product")]
        public int FromProductId { get; set; }
        [Required(ErrorMessage = "Choose From Department")]
        public int FromDeptId { get; set; }
        [Required(ErrorMessage = "Choose From Manager")]
        public string FromManagerId;
        public string EmpId;
        public string EmpName { get; set; }
        [Required(ErrorMessage = "Choose Replacement For Employee Name")]
        public string ReplacementFor;
        [Required(ErrorMessage = "Please select Manager ID.")]
        public string FromManagerName { get; set; }
        [Required(ErrorMessage = "Please enter From Product name.")]
        public string FromProductName { get; set; }
        [Required(ErrorMessage = "Please enter From Department name.")]
        public string FromDeptName { get; set; }
        public List<string> EssentialSkills { get; set; }
        public List<string> DesiredSkills { get; set; }
        public List<string> OptionalSkills { get; set; }
        public List<string> ReviewedBy { get; set; }
        public List<string> ReviewedStatus { get; set; }
        public List<string> ReviewedComments { get; set; }
        public List<string> ReviewedEmpId { get; set; }
        [Required(ErrorMessage = "Comments required.")]
        public string InputComments { get; set; }
        public int Status { get; set; }
        public int hiredNumberCandidate { get; set; }
    }
}