using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

// INTEX II
// Group 2-2
// Garrett Ashcroft, Jared Rosenlund, Vivian Solgere, and Caroline Tobler

namespace BrickHouse.Controllers
{
    public class HomeController : Controller
    {
        // Initialize private repository instance
        private IIntexRepository _repo;

        public HomeController(IIntexRepository temp)
        {
            // Assign temporary public repo resource to private var
            _repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }


        /*public IActionResult ProductPage(int pageNum, string[] category, string color)


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

        // Honestly idek what this attribute does
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
            // Find clicked product by productID
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);
            
            // Just in case product doesn't exist
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        
        // Must be logged in to see this page; unauthenticated users redirected to login
        [Authorize]
        public IActionResult Checkout()
        {
            // Build view model
            var viewModel = new CheckoutViewModel
            {
                UniqueBanks = _repo.Orders.Select(o => o.Bank).Distinct().ToList(),
                UniqueCardTypes = _repo.Orders.Select(o => o.TypeOfCard).Distinct().ToList(),
                UniqueCountriesOfTransaction = _repo.Orders.Select(o => o.CountryOfTransaction).Distinct().ToList(),
                UniqueShippingAddresses = _repo.Orders.Select(o => o.ShippingAddress).Distinct().ToList(),
            };

            return View(viewModel);
        }


    }
}
