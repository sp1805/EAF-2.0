using System.ComponentModel.DataAnnotations;

namespace EAFProject.ViewModels
{
    public class Interview
    {
        public string SkillType { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public override string ToString()
        {
            return SkillName;
        }
        [Required(ErrorMessage = "Please rate the Skill")]
        public int Rating { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public string comments { get; set; }
    }
}