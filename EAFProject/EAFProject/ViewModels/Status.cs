namespace EAFProject.ViewModels
{
    public class Status
    {
        public string StatusValue { get; set; }
        public int StatusID { get; set; }
        public override string ToString()
        {
            return StatusValue;
        }
    }
}