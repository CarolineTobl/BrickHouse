namespace BrickHouse.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Product> Products { get; }

        public IQueryable<Order> Orders { get; }

        public IQueryable<Customer> Customers { get; }

        public IQueryable<ProdRec> ProdRecs { get; }
        
        public IQueryable<ProductRecommendation> ProductRecommendations { get; }

        public IQueryable<CustomerRecommendation> CustomerRecommendations { get; }

        // New method to add a Customer
        public Task AddCustomerAsync(Customer customer);

        public void AddLineItem(LineItem li);
        public void AddOrder(Order o);

    }
}
