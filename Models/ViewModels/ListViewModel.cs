namespace Intex.Models.ViewModels
{
    public class ListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public IQueryable<Orders> Orderss { get; set; }
        public IQueryable<Customer> Customers { get; set; }
        public IQueryable<LineItem> LineItems { get; set; }
        public IQueryable<ItemRecommendation> ItemRecommendations { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    }
}