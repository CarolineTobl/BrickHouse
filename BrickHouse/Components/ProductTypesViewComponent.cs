using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;
using BrickHouse.Models.ViewModels;

namespace BrickHouse.Components
{
    public class ProductTypesViewComponent : ViewComponent
    {
        private IIntexRepository _intexRepo;

        // Constructor
        public ProductTypesViewComponent(IIntexRepository temp)
        {
            _intexRepo = temp;
        }

        public IViewComponentResult Invoke(string primaryCategory, string secondaryCategory, string primaryColor, int pageSize)
        {
            ViewBag.SelectedProductType = primaryCategory;
            ViewBag.SelectedColor = primaryColor;

            pageSize = pageSize;



            var productTypes = _intexRepo.Products
                .Select(x => x.PrimaryCategory)
                .Distinct()
                .OrderBy(x => x);

            return View(productTypes);
        }





    }
}
