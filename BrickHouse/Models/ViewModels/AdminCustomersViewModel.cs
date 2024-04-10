namespace BrickHouse.Models.ViewModels
{
    public class AdminCustomersViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
