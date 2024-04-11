using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;
using System.Collections.Generic;
using System.Linq;

namespace BrickHouse.Components
{
    public class ColorViewComponent : ViewComponent
    {
        private readonly IIntexRepository _intexRepo;

        public ColorViewComponent(IIntexRepository temp)
        {
            _intexRepo = temp;
        }

        public IViewComponentResult Invoke()
        {
            var primaryColors = _intexRepo.Products
                .Select(x => x.PrimaryColor)
                .Where(x => !string.IsNullOrEmpty(x));

            var secondaryColors = _intexRepo.Products
                .Select(x => x.SecondaryColor)
                .Where(x => !string.IsNullOrEmpty(x));

            var colorTypes = primaryColors
                .Union(secondaryColors)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return View(colorTypes);
        }

    }
}
