using Microsoft.AspNetCore.Mvc;
using BrickHouse.Models;

namespace BrickHouse.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;
        
        public CartSummaryViewComponent(Cart cartService)
        {
            this.cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
