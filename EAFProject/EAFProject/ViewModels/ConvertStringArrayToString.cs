namespace EAFProject.ViewModels
{
    public class ConvertStringArrayToString
    {
        public static string ConvertArraytoString(string[] array)
        {
            string result = string.Join(",", array);
            return result;
        }
    }
}