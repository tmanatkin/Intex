namespace Intex.Models.ViewModels
{
    public class ListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    }
}