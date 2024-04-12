namespace BrickHouse.Models.ViewModels
{

    public class ProductRecommendationViewModel
    {
        public IQueryable<ProductRecommendation> ProductRecommendations { get; set; }
        public List<List<string>> ProductNames { get; set; } // Change to List<List<string>>
        public List<Product> ProductInfos { get; set; }
    }
}

