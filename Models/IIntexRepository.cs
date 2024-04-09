namespace Intex.Models
{
    public interface IIntexRepository
    {
        IQueryable<Product> Products { get; }
    }
}