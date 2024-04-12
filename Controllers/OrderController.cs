using Microsoft.AspNetCore.Mvc;
using Intex.Models;
namespace Intex.Controllers
{
    public class OrderController : Controller
    {
        public ViewResult Checkout() => View(new Orders());
    }
}