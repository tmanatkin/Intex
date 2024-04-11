namespace Intex.Models
{
    public class EFIntexRepository : IIntexRepository
    {
      private IntexContext _context;
      public EFIntexRepository(IntexContext temp)
      {
        _context = temp;
      }
        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Orders> Orderss => _context.Orderss;
        public IQueryable<Customer> Customers => _context.Customers;
        public IQueryable<LineItem> LineItems => _context.LineItems;
        public IQueryable<ItemRecommendation> ItemRecommendations => _context.ItemRecommendations;
        public IQueryable<UserRecommendation> UserRecommendations => _context.UserRecommendations;
    }
}