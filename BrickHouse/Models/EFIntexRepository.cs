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
    }
}
