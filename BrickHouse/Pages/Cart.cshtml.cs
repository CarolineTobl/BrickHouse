using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BrickHouse.Infrastructure;
using BrickHouse.Models;

namespace BrickHouse.Pages
{
    public class CartModel : PageModel
    {
        private IIntexRepository _repo;

        public CartModel(IIntexRepository temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }

        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int ProductId, string returnUrl) 
        {
            Product proj = _repo.Products
                .FirstOrDefault(x => x.ProductId == ProductId);

            if (proj != null) 
            {
                //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(proj, 1);
                //HttpContext.Session.SetJson("cart", Cart);
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove (int ProductId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == ProductId).Product);

            return RedirectToPage(new {returnUrl = returnUrl});
        }
    }
}
