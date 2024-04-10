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
            var colorTypes = _intexRepo.Products
                .SelectMany(x => new List<string> { x.PrimaryColor, x.SecondaryColor })
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .OrderBy(x => x)
                .ToList(); // Materialize the query to execute it

            return View(colorTypes);
        }
    }
}
