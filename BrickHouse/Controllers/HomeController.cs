using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BrickHouse.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using Newtonsoft.Json;

// INTEX II
// Group 2-2
// Garrett Ashcroft, Jared Rosenlund, Vivian Solgere, and Caroline Tobler

namespace BrickHouse.Controllers
{
    public class HomeController : Controller
    {
        // Initialize private repository instance
        private IIntexRepository _repo;
        private UserManager<IdentityUser> _userManager;
        private PredictionService _predictionService;

        public HomeController(IIntexRepository repo, UserManager<IdentityUser> userManager, PredictionService predictionService)
        {
            // Assign temporary public repo resource to private var
            _repo = repo;
            _userManager = userManager;
            _predictionService = predictionService;
        }

        public IActionResult Index()
        {
            // Get the top 5 rated products
            var topProducts = _repo.ProdRecs
                                   .OrderByDescending(p => p.MeanRating)
                                   .Take(5)
                                   .ToList();

            // Select the details from the Products table based on the product_ID
            var topProductDetails = topProducts.Select(p => _repo.Products.FirstOrDefault(prod => prod.ProductId == p.ProductId)).ToList();

            ViewBag.Products = topProductDetails;

            return View();
        }

        public IActionResult ProductPage(int pageNum, string[] category, string[] color, int pageSize)
        {
            int adjustedPageNum = pageNum <= 0 ? 1 : pageNum;

            if (pageSize == 0)
            {
                pageSize = 5;
            }

            var selectedCategories = category ?? new string[] { };
            var selectedColors = color ?? new string[] { };

            var productsQuery = _repo.Products
                .Where(x =>
                    (selectedCategories.Length == 0 || selectedCategories.All(c =>
                        x.PrimaryCategory == c || x.SecondaryCategory == c || x.TertiaryCategory == c))
                    &&
                    (selectedColors.Length == 0 || selectedColors.Any(col =>
                        x.PrimaryColor == col || x.SecondaryColor == col)))
                .OrderBy(x => x.Name);

            // Count the total filtered products
            int totalFilteredItems = productsQuery.Count();

            var products = productsQuery
                .Skip((adjustedPageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productsViewModel = new ProductsListViewModel
            {
                Products = products.AsQueryable(),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    CurrentProductType = selectedCategories.Length > 0 ? string.Join(", ", selectedCategories) : null,
                    CurrentColor = selectedColors.Length > 0 ? string.Join(", ", selectedColors) : null,
                    TotalItems = totalFilteredItems // Update with total filtered products count
                },

                ItemsPerPage = pageSize,
                SelectedCategory = selectedCategories, // Store selected category
                SelectedColor = selectedColors // Store selected color
            };

            ViewBag.SelectedCategories = selectedCategories.ToList();
            ViewBag.SelectedColors = selectedColors.ToList();
            return View("ProductPage", productsViewModel);
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
         [HttpGet]
         public IActionResult Checkout()
         {
             // Build view model
             var viewModel = new CheckoutViewModel
             {
                 UniqueBanks = _repo.Orders.Select(o => o.Bank).Distinct().ToList(),
                 UniqueCardTypes = _repo.Orders.Select(o => o.TypeOfCard).Distinct().ToList(),
                 UniqueCountriesOfTransaction = _repo.Orders.Select(o => o.CountryOfTransaction).Distinct().ToList(),
                 UniqueShippingAddresses = _repo.Orders.Select(o => o.ShippingAddress).Distinct().ToList(),

                 Order = new Order(),
                 Cart = HttpContext.Session.GetJson<Cart>("Cart") // Get the session cart
             };

             return View(viewModel);
         }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
         {
             // Create new transaction ID
             int newId = 0;
             
             // Reset session cart
             model.Cart = HttpContext.Session.GetJson<Cart>("Cart"); // Get the session cart
             model.Order.Amount = (double)model.Cart.CalculateTotal();
             
             if (_repo.Orders.Any()) // Check if there are any orders in the database
             {
                 int maxId = await _repo.Orders.MaxAsync(o => o.TransactionId); // Get the max TransactionId
                 newId = maxId + 1; // Increment by one
             }
             else
             {
                 newId = 100000; // Start from 100000 if there are no orders
             }

             // Set transaction ID
             model.Order.TransactionId = newId;
             
             // Find Customer ID associated with session user
             var userId = _userManager.GetUserId(User);
             var custId = await _repo.Customers
                 .Where(c => c.AspNetUserId == userId)
                 .Select(c => c.CustomerId)
                 .FirstOrDefaultAsync();
             
             // Set custId attribute in order object
             model.Order.CustomerId = custId;

             //get customer object
             var customer = await _repo.Customers.Where(c => c.CustomerId==custId).FirstOrDefaultAsync();

             // Run fraud check
             model.Order.Fraud = _predictionService.Predict(model.Order,customer);

             foreach (var l in model.Cart.Lines)
             {
                 // Create LineItem and fill with data
                 var li = new LineItem();
                 li.TransactionId = model.Order.TransactionId;
                 li.ProductId = l.Product.ProductId;
                 li.Qty = (byte)l.Quantity;
                 
                 // Add LineItem to database
                 _repo.AddLineItem(li);
             }
             
             // Add Order to the database
             _repo.AddOrder(model.Order);
             
             // Reset the session cart
             HttpContext.Session.Remove("Cart");
             var newCart = new Cart();
             string cartJson = JsonConvert.SerializeObject(newCart);
             HttpContext.Session.SetString("Cart", cartJson);
             
             // Send to order confirmation or fraud review confirmation
             if (model.Order.Fraud == 1)
             {
                 return View("CheckoutFraud");
             }

             return View("CheckoutConfirmed");
         }
        
    }
}
