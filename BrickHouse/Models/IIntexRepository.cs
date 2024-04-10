namespace BrickHouse.Models
{
    public interface IIntexRepository
    {
        public IQueryable<Product> Products { get; }

    }
}
