namespace BrickHouse.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Product> Products { get; }

        public IQueryable<Order> Orders { get; }

    }
}
