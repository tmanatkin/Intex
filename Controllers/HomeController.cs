using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Buffers.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace Intex.Controllers;

public class HomeController : Controller
{
    private IIntexRepository _repo;
    private readonly InferenceSession _session;
    private readonly string _onnxModelPath;
    //private readonly ItemRecommendation.ProductService _recommendationService;

    public HomeController(IIntexRepository temp, IHostEnvironment hostEnvironment)
    {
        _repo = temp;

        // These are for ReviewOrders
        _onnxModelPath = System.IO.Path.Combine(hostEnvironment.ContentRootPath, "fraudModel.onnx");
        //initialize the InferenceSession
        _session = new InferenceSession(_onnxModelPath);

        // This is for Item Recommanmdations
        //_recommendationService = new ItemRecommendation.ProductService();
    }

    public IActionResult Index()
    {
        List<int> productIds = new List<int> { 23, 21, 22, 20, 13, 24, 30, 28, 10, 12};

        var data = new ListViewModel
        {
            Products = _repo.Products
                .Where(p => productIds.Contains(p.ProductId))
        };
        return View(data);
    }

    public IActionResult Index()
    {
        List<int> productIds;

        // Check if the user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            // Generate a random customer ID within the range of existing IDs in your database
            Random random = new Random();
            int userId = random.Next(1, 29100);

            // Get the user recommendation based on the generated customer ID
            var userRecommendation = _repo.UserRecommendations.FirstOrDefault(u => u.CustomerId == userId);

            // Check if userRecommendation is null (no recommendation found for the user)
            if (userRecommendation != null)
            {
                // Get the recommended product IDs
                productIds = new List<int> {
                (int)userRecommendation.RecId1,
                (int)userRecommendation.RecId2,
                (int)userRecommendation.RecId3,
                (int)userRecommendation.RecId4,
                (int)userRecommendation.RecId5
            };
            }
            else
            {
                // If the user has no recommendations, show default products
                productIds = new List<int> { 23, 21, 22, 20, 13 };
            }
        }
        else
        {
            // If the user is not authenticated, show default products
            productIds = new List<int> { 23, 21, 22, 20, 13 };
        }



        var data = new ListViewModel
        {
            Products = _repo.Products
                .Where(p => productIds.Contains(p.ProductId))
        };
        return View(data);
    }


    public IActionResult Products(int pageNum = 1)
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

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult ProductDetail(int id)
    {
        // Retrieve the product details from the database based on the ID
        var product = _repo.Products.FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            // Handle the case where the product is not found
            return NotFound();
        }

        // Retrieve recommendations for the product from the database
        var recommendations = _repo.ItemRecommendations.FirstOrDefault(r => r.ProductId == id);

        // Create a ProductDetailViewModel object and pass product details and recommendations to it
        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            Recommendations = recommendations
        };

        // Pass the ProductDetailViewModel object to the ProductDetail view
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminHome()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult EditProducts()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult EditUsers()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ReviewOrders()
    {
        var records = _repo.Orderss
            .OrderByDescending(o => o.Date)
            .Take(20)
            .ToList();  // Fetch the 20 most recent records
        var predictions = new List<OrderPrediction>();  // Your ViewModel for the view

        // Dictionary mapping the numeric prediction to an animal type
        var class_type_dict = new Dictionary<int, string>
            {
                { 0, "Not Fraud" },
                { 1, "Fraud" }
            };

        foreach (var record in records)
        {
            // Calculate days since January 1, 2022
            var january1_2022 = new DateTime(2022, 1, 1);
            var daysSinceJan12022 = record.Date.HasValue ? Math.Abs((record.Date.Value - january1_2022).Days) : 0;

            // Preprocess features to make them compatible with the model
            var input = new List<float>
                {
                    (float)record.CustomerId, 
                    (float)record.Time,    
                    // fix amount if it's null
                    (float)(record.Amount ?? 0),

                    // fix date
                    daysSinceJan12022,

                    // Check the Dummy Coded Data
                    record.DayOfWeek == "Mon" ? 1 : 0,
                    record.DayOfWeek == "Sat" ? 1 : 0,
                    record.DayOfWeek == "Sun" ? 1 : 0,
                    record.DayOfWeek == "Thu" ? 1 : 0,
                    record.DayOfWeek == "Tue" ? 1 : 0,
                    record.DayOfWeek == "Wed" ? 1 : 0,

                    record.EntryMode == "Pin" ? 1 : 0,
                    record.EntryMode == "Tap" ? 1 : 0,

                    record.TypeOfTransaction == "Online" ? 1 : 0,
                    record.TypeOfTransaction == "POS" ? 1 : 0,

                    record.CountryOfTransaction == "India" ? 1 : 0,
                    record.CountryOfTransaction == "Russia" ? 1 : 0,
                    record.CountryOfTransaction == "USA" ? 1 : 0,
                    record.CountryOfTransaction == "UnitedKingdom" ? 1 : 0,

                    // Use CountryOfTransaction if ShippingAddress is null
                    (record.ShippingAddress ?? record.CountryOfTransaction) == "India" ? 1 : 0,
                    (record.ShippingAddress ?? record.CountryOfTransaction) == "Russia" ? 1 : 0,
                    (record.ShippingAddress ?? record.CountryOfTransaction) == "USA" ? 1 : 0,
                    (record.ShippingAddress ?? record.CountryOfTransaction) == "UnitedKingdom" ? 1 : 0,

                    record.Bank == "HSBC" ? 1 : 0,
                    record.Bank == "Halifax" ? 1 : 0,
                    record.Bank == "Lloyds" ? 1 : 0,
                    record.Bank == "Metro" ? 1 : 0,
                    record.Bank == "Monzo" ? 1 : 0,
                    record.Bank == "RBS" ? 1 : 0,
                    
                    record.TypeOfCard == "Visa" ? 1 : 0
                };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

            string predictionResult;
            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                predictionResult = prediction != null && prediction.Length > 0 ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
            }

            predictions.Add(new OrderPrediction { Orderss = record, Prediction = predictionResult }); // Adds the animal information and prediction for that animal to AnimalPrediction viewmodel
        }

        return View(predictions);
    }
}