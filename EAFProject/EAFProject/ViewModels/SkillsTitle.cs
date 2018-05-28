namespace EAFProject.ViewModels
{
    public class SkillsTitle
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public override string ToString()
        {
            return SkillName;
        }
    }
}