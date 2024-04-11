using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BrickHouse.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        // "Shop" page for all products
        public IActionResult ProductPage(int pageNum, string category)
        {
            // Set default page size
            int pageSize = 5;
            int adjustedPageNum = pageNum <= 0 ? 1 : pageNum;

            // Build correct view model
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

            // Return the view and view model
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
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart()
                
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            // Create new transaction ID
            int newId = 0;
            
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
            // Add customer ID
            
            // Run fraud check
            model.Order.Fraud = 0;
            
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
            
            // Send to order confirmation or fraud review confirmation
            if (model.Order.Fraud == 1)
            {
                return View("CheckoutFraud");
            }

            return View("CheckoutConfirmed");
        }
    }
}
