namespace BrickHouse.Models.ViewModels
{
    public class AdminProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
