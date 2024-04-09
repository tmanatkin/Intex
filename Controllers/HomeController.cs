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

    public IActionResult AboutUs()
    {
        return View();
    }
    public IActionResult Products()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Index(int pageNum)
    {

        int pageSize = 10;

        var data = new ListViewModel
        {
            Products = _repo.Products
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                TotalNumItems = _repo.Products.Count(),
                NumItemsPerPage = pageSize,
                CurrentPageNum = pageNum
            }
        };

        return View(data);
    }
}