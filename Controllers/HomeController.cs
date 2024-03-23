using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission11.Models;
using Mission11.Models.ViewModels;

namespace Mission11.Controllers;

public class HomeController : Controller
{
   private IBookstoreRepository _repo;
    public HomeController(IBookstoreRepository temp)
    {
        _repo = temp;
    }

    public IActionResult Index(int pageNum)
    {

        int pageSize = 10;

        var data = new BookListViewModel
        {
            Books = _repo.Books
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                TotalNumItems = _repo.Books.Count(),
                NumItemsPerPage = pageSize,
                CurrentPageNum = pageNum
            }
        };

        return View(data);
    }
}