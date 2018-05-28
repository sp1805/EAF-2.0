using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EAFProject.ViewModels
{
    public class JobTitle
    {
        public string JobName { get; set; }
        [DisplayName("Job Title")]
        [Required(ErrorMessage = "Job Title is Required")]
        public int JobID { get; set; }
        public override string ToString()
        {
            return JobName;
        }
    }
}