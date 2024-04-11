namespace BrickHouse.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; }  
        
        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int) (Math.Ceiling((decimal) TotalItems / ItemsPerPage));

        public string CurrentProductType { get; set; }
      
        public string CurrentColor { get; set; }

    }
}
