using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;
using System.Collections.Generic;
using System.Linq;
using BrickHouse.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BrickHouse.Components
{
    public class ProductRecommendationViewComponent : ViewComponent
    {
        private readonly IIntexRepository _intexRepo;

        public ProductRecommendationViewComponent(IIntexRepository temp)
        {
            _intexRepo = temp;
        }

        /*public IViewComponentResult Invoke(int productId)
        {
            // Find the product recommendations for the specified productId
            var productRecommendations = _intexRepo.ProductRecommendations
                .Where(pr => pr.ProductId == productId)
                .ToList(); // Convert to list to fetch all recommendations for the given product

            // Fetch the corresponding product names from the Products table for each recommendation
            var productNames = new List<List<string>>();

            foreach (var recommendation in productRecommendations)
            {
                // Initialize a list to store product names for each recommendation
                var recommendationProductNames = new List<string>();

                // Fetch the product name for Rec1
                var productName1 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec1)
                    .Select(p => p.Name)
                    .FirstOrDefault();
                recommendationProductNames.Add(productName1);

                // Fetch the product name for Rec2
                var productName2 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec2)
                    .Select(p => p.Name)
                    .FirstOrDefault();
                recommendationProductNames.Add(productName2);

                // Fetch the product name for Rec3
                var productName3 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec3)
                    .Select(p => p.Name)
                    .FirstOrDefault();
                recommendationProductNames.Add(productName3);

                // Fetch the product name for Rec4
                var productName4 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec4)
                    .Select(p => p.Name)
                    .FirstOrDefault();
                recommendationProductNames.Add(productName4);

                // Fetch the product name for Rec5
                var productName5 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec5)
                    .Select(p => p.Name)
                    .FirstOrDefault();
                recommendationProductNames.Add(productName5);

                // Add the list of product names to the main list
                productNames.Add(recommendationProductNames);
            }

            // Create a view model to pass both product recommendations and product names to the view
            var viewModel = new ProductRecommendationViewModel
            {
                ProductRecommendations = productRecommendations.AsQueryable(), // Convert list to IQueryable
                ProductNames = productNames // Set the product names
            };

            // Pass the view model to the view
            return View(viewModel);
        }*/

        public IViewComponentResult Invoke(int productId)
        {
            // Find the product recommendations for the specified productId
            var productRecommendations = _intexRepo.ProductRecommendations
                .Where(pr => pr.ProductId == productId)
                .ToList(); // Convert to list to fetch all recommendations for the given product

            // Fetch the corresponding product names from the Products table for each recommendation
            var productNames = new List<List<string>>();
            var productInfos = new List<Product>();

            foreach (var recommendation in productRecommendations)
            {
                // Fetch the product information for Rec1
                var productInfo1 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec1)
                    .Select(p => new Product { Name = p.Name, Price = p.Price, ImgLink = p.ImgLink, ProductId = p.ProductId })
                    .FirstOrDefault();
                productInfos.Add(productInfo1);

                // Fetch the product information for Rec2
                var productInfo2 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec2)
                    .Select(p => new Product { Name = p.Name, Price = p.Price, ImgLink = p.ImgLink, ProductId = p.ProductId })
                    .FirstOrDefault();
                productInfos.Add(productInfo2);

                // Fetch the product information for Rec3
                var productInfo3 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec3)
                    .Select(p => new Product { Name = p.Name, Price = p.Price, ImgLink = p.ImgLink, ProductId = p.ProductId })
                    .FirstOrDefault();
                productInfos.Add(productInfo3);

                // Fetch the product information for Rec4
                var productInfo4 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec4)
                    .Select(p => new Product { Name = p.Name, Price = p.Price, ImgLink = p.ImgLink, ProductId = p.ProductId })
                    .FirstOrDefault();
                productInfos.Add(productInfo4);

                // Fetch the product information for Rec5
                var productInfo5 = _intexRepo.Products
                    .Where(p => p.ProductId == recommendation.Rec5)
                    .Select(p => new Product { Name = p.Name, Price = p.Price, ImgLink = p.ImgLink, ProductId = p.ProductId })
                    .FirstOrDefault();
                productInfos.Add(productInfo5);
            }

            // Create a view model to pass both product recommendations and product names to the view
            var viewModel = new ProductRecommendationViewModel
            {
                ProductRecommendations = productRecommendations.AsQueryable(), // Convert list to IQueryable
                ProductNames = productNames, // Set the product names
                ProductInfos = productInfos // Set the product information
            };

            // Pass the view model to the view
            return View(viewModel);
        }



    }
}
