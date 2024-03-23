namespace Mission11.Models
{
    public interface IBookstoreRepository
    {
        IQueryable<Book> Books { get; }
    }
}