using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EAFProject.ViewModels
{
    public class CandidateClass
    {
        public int TempId { get; set; }
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Please Enter a Valid Name.")]
        public string name { get; set; }
        [DisplayName("Phone Number")]
        [StringLength(10, ErrorMessage = "Enter a 10 Digit Mobile Number", MinimumLength = 10)]
        public Int64 phoneNumber { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter a Valid Email Address")]
        public string email { get; set; }
        [DisplayName("Status")]
        public string candidateStatus { get; set; }
        [DisplayName("Date Time")]
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}