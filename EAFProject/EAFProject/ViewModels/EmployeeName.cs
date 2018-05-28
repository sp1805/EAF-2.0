namespace EAFProject.ViewModels
{
    public class EmployeeName
    {
        public string EmpName { get; set; }
        public string EmpId { get; set; }

        public override string ToString()
        {
            return EmpName;
        }
    }
}