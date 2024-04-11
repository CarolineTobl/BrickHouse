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


        public IActionResult ProductPage(int pageNum, string[] category, string[] color, int pageSize = 5)
        {
            int adjustedPageNum = pageNum <= 0 ? 1 : pageNum;

            var selectedCategories = category ?? new string[] { };
            var selectedColors = color ?? new string[] { };

            var productsQuery = _repo.Products
                .Where(x =>
                    (selectedCategories.Length == 0 || selectedCategories.All(c =>
                        x.PrimaryCategory == c || x.SecondaryCategory == c || x.TertiaryCategory == c))
                    &&
                    (selectedColors.Length == 0 || selectedColors.Any(col =>
                        x.PrimaryColor == col || x.SecondaryColor == col)))
                .OrderBy(x => x.Name)
                .Skip((adjustedPageNum - 1) * pageSize)
                .Take(pageSize);

            var products = productsQuery.ToList();

            var productsViewModel = new ProductsListViewModel
            {
                Products = products.AsQueryable(),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    CurrentProductType = selectedCategories.Length > 0 ? string.Join(", ", selectedCategories) : "All",
                    CurrentColor = selectedColors.Length > 0 ? string.Join(", ", selectedColors) : "All",
                    TotalItems = _repo.Products.Count() // Update this to consider filters if needed
                },
                AvailableColors = products
                    .SelectMany(x => new List<string> { x.PrimaryColor, x.SecondaryColor })
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            // Set ItemsPerPage to 5 explicitly
            productsViewModel.ItemsPerPage = pageSize;

            ViewBag.SelectedCategories = selectedCategories.ToList();
            ViewBag.SelectedColors = selectedColors.ToList();
            return View("ProductPage", productsViewModel); // Ensure you are passing the correct model here
        }


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
