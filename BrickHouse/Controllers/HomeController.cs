using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static BrickHouse.Models.Product;

namespace BrickHouse.Controllers
{
    public class HomeController : Controller
    {
        private IIntexRepository _repo;

        public HomeController(IIntexRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(int pageNum, string category)
        {

            int pageSize = 5;

            var blah = new ProductsListViewModel
            {


                Products = _repo.Products
                .Where(x => x.PrimaryCategory == category || category == null)
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    // if Product type is null, get a count of all Products, if filtering then only get the count of the filtered Products
                    TotalItems = category == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryCategory == category).Count()
                },

                CurrentProductType = category
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
    }
}
