namespace EAFProject.ViewModels
{
    public class ProductName
    {
        public string ProductTitle { get; set; }
        public int ProductId { get; set; }

        public override string ToString()
        {
            return ProductTitle;
        }
    }
}