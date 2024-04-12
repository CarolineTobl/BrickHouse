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

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
    }
}
