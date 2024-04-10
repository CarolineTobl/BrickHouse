using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BrickHouse.Controllers
{
    public class HomeController : Controller
    {
        private IIntexRepository _repo;

        public HomeController(IIntexRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*public IActionResult ProductPage(int pageNum, string[] category, string color)
        {

            int pageSize = 5;

            var selectedCategories = category ?? new string[] { }; // Handle null case

            var productsViewModel = new ProductsListViewModel
            {


                Products = _repo.Products
                    .Where(x => string.IsNullOrEmpty(category) ||
                                x.PrimaryCategory == category ||
                                x.SecondaryCategory == category ||
                                x.TertiaryCategory == category)
                    .OrderBy(x => x.Name)
                    .Skip((adjustedPageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    CurrentProductType = category,
                    TotalItems = string.IsNullOrEmpty(category) ? _repo.Products.Count() :
                                 _repo.Products.Where(x => x.PrimaryCategory == category || x.SecondaryCategory == category).Count()
                },
                
            };

            // Return the regular view along with the view model
            return View("ProductPage", productsViewModel);
        }

        /*        public IActionResult Index()
                {
                    return View();
                }*/







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
