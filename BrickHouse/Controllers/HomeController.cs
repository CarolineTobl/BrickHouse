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

        public IActionResult ProductPage(int pageNum, string primaryCategory)
        {

            int pageSize = 5;

            //Handles a pagenumber less than 0 and defaults it to 2 (so it can be subtracted by 1 below)
            int adjustedPageNum = pageNum <= 0 ? 2 : pageNum;

            var blah = new ProductsListViewModel
            {


                Products = _repo.Products
                .Where(x => x.PrimaryCategory == primaryCategory || primaryCategory == null)
                .OrderBy(x => x.Name)
                .Skip((adjustedPageNum - 1) * pageSize)
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    // if Product type is null, get a count of all Products, if filtering then only get the count of the filtered Products
                    TotalItems = primaryCategory == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryCategory == primaryCategory).Count()
                },

                CurrentProductType = primaryCategory
            };

            /*            var ProductData = _repo.Products
                            .OrderBy(x => x.Name)
                            .Skip(pageSize * (pageNum -1))
                            .Take(pageSize);*/

            return View(blah);
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

        public IActionResult Checkout()
        {
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
