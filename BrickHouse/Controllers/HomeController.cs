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
        public IActionResult ProductPage(int pageNum, string primaryCategory, string secondaryCategory)
        {
            int pageSize = 5;
            int adjustedPageNum = pageNum <= 0 ? 1 : pageNum;
            ViewBag.SelectedProductType = primaryCategory;

            var productsViewModel = new ProductsListViewModel
            {
                Products = _repo.Products
                    .Where(x => (string.IsNullOrEmpty(primaryCategory) || x.PrimaryCategory == primaryCategory) &&
                                (string.IsNullOrEmpty(secondaryCategory) || x.SecondaryCategory == secondaryCategory))
                    .OrderBy(x => x.Name)
                    .Skip((adjustedPageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = primaryCategory == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryCategory == primaryCategory).Count()
                },
                CurrentProductType = primaryCategory
            };

            // Return the regular view along with the view model
            return View("ProductPage", productsViewModel);
        }

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
