namespace BrickHouse.Models.ViewModels
{
    public class AdminOrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
