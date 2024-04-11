namespace BrickHouse.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Product> Products { get; }

        public IQueryable<Order> Orders { get; }

        public IQueryable<Customer> Customers { get; }

        // New method to add a Customer
        public Task AddCustomerAsync(Customer customer);

    }
}
