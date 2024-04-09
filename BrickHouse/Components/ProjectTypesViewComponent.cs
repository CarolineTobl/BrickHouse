using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;

namespace BrickHouse.Components
{
    public class ProductTypesViewComponent :ViewComponent
    {
        private IIntexRepository _waterRepo;

        //Constructor
        public ProductTypesViewComponent(IIntexRepository temp) 
        {
            _waterRepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductType = RouteData?.Values["ProductType"];

            var ProductTypes = _waterRepo.Products
                .Select(x => x.PrimaryCategory)
                .Distinct()
                .OrderBy(x => x);

            return View(ProductTypes);
        }
    }
}
