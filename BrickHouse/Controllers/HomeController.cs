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

        public IActionResult ProductPage(int pageNum, string category)
        {
            int pageSize = 5;
            int adjustedPageNum = pageNum <= 0 ? 1 : pageNum;

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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ProductDetails(int productId)
        {
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

/*        public IActionResult Checkout()
        {
 
            var viewModel = new CheckoutViewModel
            {
                UniqueBanks = _repo.Orders.Select(o => o.Bank).Distinct().ToList(),
                UniqueCardTypes = _repo.Orders.Select(o => o.TypeOfCard).Distinct().ToList(),
                UniqueCountriesOfTransaction = _repo.Orders.Select(o => o.CountryOfTransaction).Distinct().ToList(),
                UniqueShippingAddresses = _repo.Orders.Select(o => o.ShippingAddress).Distinct().ToList(),
            };

            return View(viewModel);
        }*/


    }
}
