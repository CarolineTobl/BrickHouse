using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;

namespace BrickHouse.Components
{
    public class ProductTypesViewComponent :ViewComponent
    {
        private IIntexRepository _intexRepo;

        //Constructor
        public ProductTypesViewComponent(IIntexRepository temp) 
        {
            _intexRepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductType = RouteData?.Values["ProductType"];

            var ProductTypes = _intexRepo.Products
                .Select(x => x.PrimaryCategory)
                .Distinct()
                .OrderBy(x => x);

            return View(ProductTypes);
        }
    }
}
