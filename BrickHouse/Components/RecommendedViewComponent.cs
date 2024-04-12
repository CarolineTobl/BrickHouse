using System.Security.Claims;
using BrickHouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;

namespace BrickHouse.Components;

public class RecommendedViewComponent : ViewComponent
{
    private IIntexRepository _repo;
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;

    public RecommendedViewComponent(IIntexRepository temp, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        ViewBag.signedIn = false;
        
        _repo = temp;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IViewComponentResult Invoke()
    {
        List<Product> Products = null;
        int custId = 0;
        
        if (_signInManager.IsSignedIn((ClaimsPrincipal)User))
        {
            ViewBag.signedIn = true;
            
            // Find Customer ID associated with session user
            var userId = _userManager.GetUserId((ClaimsPrincipal)User);
            var customer= _repo.Customers
                .Where(c => c.AspNetUserId == userId)
                .FirstOrDefault();

            if (customer != null)
            {
               custId = customer.CustomerId; 
            }
            else
            {
                ViewBag.signedIn = false;
            }
            
            
            // Pass five recommendations based on customer previous purchases
            var Recommendations = _repo.CustomerRecommendations
                .Where(cr => cr.CustomerId == custId)
                .Take(5).ToList();

            foreach (CustomerRecommendation cr in Recommendations)
            {
                Products.Add(_repo.Products
                    .Where(p => p.Name == cr.Recommendation)
                    .FirstOrDefault());
            }
            
            // If Products is null, that means user has no rating history and should be given default products
            if (Products == null)
            {
                ViewBag.signedIn = false;
            }
            else
            {
                ViewBag.Recommendations = Products;
            }
        }
        
        if (ViewBag.signedIn == false || !_signInManager.IsSignedIn((ClaimsPrincipal)User))
        {
            Products = [];
            
            // User is not logged in or has no rating history
            var ProdRecs = _repo.ProdRecs
                .OrderByDescending(p => p.MeanRating)
                .Take(5)
                .ToList();
            
            foreach (ProdRec pr in ProdRecs)
            {
                Products.Add(_repo.Products
                    .Where(p => p.ProductId == pr.ProductId)
                    .FirstOrDefault());
            }

            ViewBag.signedIn = false;
            ViewBag.Recommendations = Products;
        }
        return View();
    }
    
    
}