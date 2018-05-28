using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EAFProject.ViewModels
{
    public class EmployeeClass
    {
        [DisplayName("SWG ID")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter a Valid SWG ID.")]
        [Required]
        public string swg { get; set; }
       
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Please Enter a Valid Name.")]
        [Required]
        [StringLength(50, ErrorMessage = "The Name must be less than 50", MinimumLength = 3)]
        [Editable(true)]
        public string name { get; set; }

        [DisplayName("Job Title")]
        [Required(ErrorMessage = "Job Title is Required")]
        public string jobTitle { get; set; }

        [DisplayName("Department Name")]
        [Required]
        public string deptName { get; set; }
        [DisplayName("Product")]
        [Required]
        public string product { get; set; }
        [DisplayName("Manager")]
        [Required]
        public string manager { get; set; }
        [DisplayName("Date Of Joining")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfJoining { get; set; }
        public string email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfResignation { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? lastDay { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfOffer { get; set; }
       
        [Required(ErrorMessage = "Job Title is Required")]
        public int jobId{ get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        public int ProductId{ get; set; }
        [Required(ErrorMessage = "Department Name is Required")]
        public int DeptId{ get; set; }
    }
}