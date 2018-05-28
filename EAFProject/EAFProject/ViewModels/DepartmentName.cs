namespace EAFProject.ViewModels
{
    public class DepartmentName
    {
        public string DeptName { get; set; }
        public int DeptId { get; set; }

        public override string ToString()
        {
            return DeptName;
        }
    }
}