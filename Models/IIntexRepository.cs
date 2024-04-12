namespace Intex.Models
{
    public interface IIntexRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Orders> Orderss { get; }
        IQueryable<Customer> Customers { get; }
        IQueryable<LineItem> LineItems { get; }
        IQueryable<ItemRecommendation> ItemRecommendations { get; }
        IQueryable<UserRecommendation> UserRecommendations { get; }
        public void AddProduct(Product ProductId);
        public void EditProduct(Product ProductId);
        public void DeleteProduct(Product ProductId);
    }
}