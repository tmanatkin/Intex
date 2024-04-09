namespace Intex.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalNumItems { get; set; }
        public int NumItemsPerPage { get; set; }
        public int CurrentPageNum { get; set; }
        public int TotalNumPages => (int) (Math.Ceiling((decimal) TotalNumItems / NumItemsPerPage));
    }
}