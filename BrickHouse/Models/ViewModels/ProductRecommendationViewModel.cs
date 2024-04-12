/*namespace BrickHouse.Models.ViewModels
{
    public class ProductRecommendationViewModel
    {
        public IQueryable<ProductRecommendation> ProductRecommendations { get; set; }
        public List<string> ProductNames { get; set; } // Change to a list of strings to store multiple product names
    }
}*/

using BrickHouse.Models;

namespace BrickHouse.Models.ViewModels
{

    public class ProductRecommendationViewModel
    {
        public IQueryable<ProductRecommendation> ProductRecommendations { get; set; }
        public List<List<string>> ProductNames { get; set; } // Change to List<List<string>>
    }
}

