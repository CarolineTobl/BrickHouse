namespace BrickHouse.Models
{
    public class EFIntexRepository : IIntexRepository
    {
        private ScaffoldedDbContext _context;
        public EFIntexRepository(ScaffoldedDbContext temp) 
        {
            _context = temp;
        }

        public IQueryable<Product> Products => _context.Products;

        public IQueryable<Order> Orders => _context.Orders;
        
        public IQueryable<Customer> Customers => _context.Customers;

        public IQueryable<ProdRec> ProdRecs => _context.ProdRecs;

        public IQueryable<ProductRecommendation> ProductRecommendations => _context.ProductRecommendations;

        public IQueryable<CustomerRecommendation> CustomerRecommendations => _context.CustomerRecommendations;

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
        public void AddLineItem(LineItem li)
        {
            _context.LineItems.Add(li);
            _context.SaveChanges();
        }
        
        public void AddOrder(Order o)
        {
            _context.Orders.Add(o);
            _context.SaveChanges();
        }

    }
}
