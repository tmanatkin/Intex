namespace Intex.Models
{
    public interface IIntexRepository
    {
        IQueryable<Book> Books { get; }
    }
}