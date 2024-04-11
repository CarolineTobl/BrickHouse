using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrickHouse.Models;
using BrickHouse.Models.ViewModels;

namespace BrickHouse.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIntexRepository _repo; // Changed from ApplicationDbContext to IIntexRepository

        public CustomerController(UserManager<IdentityUser> userManager, IIntexRepository repo)
        {
            _userManager = userManager;
            _repo = repo; // Changed from _context to _repo
        }
        // GET: /AccountDetails
        public IActionResult AccountDetails()
        {
            return View(new AccountDetailsModel());
        }

        // POST: /AccountDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountDetails(AccountDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                // Create a new Customer object from the model
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    CountryOfResidence = model.Country,
                    AspNetUserId = userId // Link to the registered user
                };

                await _repo.AddCustomerAsync(customer); // Changed from _context to _repo

                return RedirectToAction("ConfirmationPage");
            }

            return View(model);
        }
    }
}

