using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Models.ViewModels;

namespace Intex.Controllers;

public class HomeController : Controller
{
   private IIntexRepository _repo;
    public HomeController(IIntexRepository temp)
    {
        _repo = temp;
    }

    public IActionResult Index(int pageNum)
    {

        int pageSize = 10;

        var data = new ListViewModel
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