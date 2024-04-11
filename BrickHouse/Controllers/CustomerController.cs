using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            return View(new AccountDetailsViewModel());
        }

        // POST: /AccountDetails
        [HttpPost]
        public async Task<IActionResult> AccountDetails(AccountDetailsViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            int newId;
            if (_repo.Customers.Any()) // Check if there are any customers in the database
            {
                int maxId = await _repo.Customers.MaxAsync(c => (int?)c.CustomerId) ?? 0; // Get the max CustomerId
                newId = maxId + 1; // Increment by one
            }
            else
            {
                newId = 30000; // Start from 30000 if there are no customers
            }

            // Create a new Customer object from the model
            var customer = new Customer
            {
                CustomerId = newId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                CountryOfResidence = model.Country,
                AspNetUserId = userId // Link to the registered user
            };

            await _repo.AddCustomerAsync(customer); // Add the new customer

            return RedirectToAction("ConfirmationPage");
        }

        public IActionResult ConfirmationPage()
        {
            return View();
        }
    }
}

