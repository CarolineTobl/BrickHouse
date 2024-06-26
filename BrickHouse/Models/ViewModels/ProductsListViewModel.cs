﻿namespace BrickHouse.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductType { get; set; }

        public string[]? CurrentColor { get; set; }

        public List<string> AvailableColors { get; set; }

        public int ItemsPerPage { get; set; }

        // Property to store selected category
        public string[] SelectedCategory { get; set; }

        // Property to store selected color
        public string[] SelectedColor { get; set; }
    }
}
