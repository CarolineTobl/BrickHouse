using BrickHouse.Models;
using BrickHouse.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

//[Authorize(Roles = "Admin")] // Ensure only users in the "Admin" role can access this controller
public class AdminController : Controller
{
    private readonly ScaffoldedDbContext _context;

    public AdminController(ScaffoldedDbContext context)
    {
        _context = context;
    }

    private bool CustomerExists(string customerId)
    {
        return _context.Customers.Any(e => e.CustomerId == customerId);
    }

    // CRUDUSERS
    //
    //
    //
    //

    public async Task<IActionResult> CRUDUsers(int pageNum = 1, int pageSize = 25)
    {
        var customersQuery = _context.Customers.AsNoTracking(); // Use AsNoTracking for read-only scenarios
        var totalItems = await customersQuery.CountAsync();

        var customers = await customersQuery
                            .OrderBy(c => c.LastName)
                            .Skip((pageNum - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var model = new AdminCustomersViewModel
        {
            Customers = customers,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            }
        };

        return View(model);
    }


    // GET: Admin/AddUser
    public IActionResult AddUser()
    {
        return View(new Customer()); // Pass a new Customer object to the view if needed for a form
    }

    // POST: Admin/AddUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUser(Customer customer)
    {
        if (ModelState.IsValid)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CRUDUsers)); // Redirect back to the listing
        }
        // If we got this far, something failed; redisplay form
        return View(customer);
    }

    // GET: Admin/EditUser/5
    public async Task<IActionResult> EditUser(string customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    // POST: Admin/EditUser/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(string customerId, Customer customer)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CRUDUsers));
        }
        return View(customer);
    }

    // GET: Admin/DeleteUserConfirmation/5
    public async Task<IActionResult> DeleteUserConfirmation(string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return NotFound();
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(m => m.CustomerId == customerId);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: Admin/DeleteUser/5
    // POST: Admin/DeleteUser
    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserConfirmed(string customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(CRUDUsers));
    }


    // CRUDPRODUCTS
    //
    //
    //
    //


    // GET: Admin/CRUDProducts
    public async Task<IActionResult> CRUDProducts(int pageNum = 1, int pageSize = 10)
    {
        var productsQuery = _context.Products.AsNoTracking(); // Adjust according to your context
        var totalItems = await productsQuery.CountAsync();

        var products = await productsQuery
                            .OrderBy(p => p.Name)
                            .Skip((pageNum - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var model = new AdminProductsViewModel
        {
            Products = products,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems,
            }
        };

        return View(model);
    }

    // GET: Admin/AddProduct
    public IActionResult AddProduct()
    {
        return View();
    }

    // POST: Admin/AddProduct
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct([Bind("Name,Year,NumParts,Price,ImgLink,PrimaryColor,SecondaryColor,Description,PrimaryCategory,SecondaryCategory,TertiaryCategory")] Product product)
    {
        if (ModelState.IsValid)
        {
            // Generate a random number for the new product ID
            int maxId = await _context.Products.MaxAsync(p => (int?)p.ProductId) ?? 37;
            int newId;
            do
            {
                newId = new Random().Next(maxId + 1, int.MaxValue);
            } while (_context.Products.Any(p => p.ProductId == newId));

            // Set the new ID to the product
            product.ProductId = newId;

            _context.Add(product);
            await _context.SaveChangesAsync(); // Save the changes asynchronously
            return RedirectToAction(nameof(CRUDProducts)); // Redirect to the product list view
        }
        return View(product); // If ModelState is not valid, return to the form with the entered data
    }

    // GET: Admin/EditProduct/5
    public async Task<IActionResult> EditProduct(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Admin/EditProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, [Bind("ProductId,Name,Year,NumParts,Price,ImgLink,PrimaryColor,SecondaryColor,Description,PrimaryCategory,SecondaryCategory,TertiaryCategory")] Product product)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(CRUDProducts));
        }
        return View(product);
    }

    // Method to show the delete confirmation view
    public async Task<IActionResult> DeleteProductConfirmation(int productId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == productId);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // Method to delete the product
    [HttpPost, ActionName("DeleteProduct")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(CRUDProducts));
    }

    // CRUD ORDERS
    //
    //
    //
    //

    public async Task<IActionResult> CRUDOrders(int pageNum = 1, int pageSize = 40)
    {
        var ordersQuery = _context.Orders.AsNoTracking(); // Use AsNoTracking for read-only scenarios
        var totalItems = await ordersQuery.CountAsync();

        var orders = await ordersQuery
                            .OrderByDescending(o => o.Date) // Assuming you want to sort by the date
                            .Skip((pageNum - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var model = new AdminOrdersViewModel
        {
            Orders = orders,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            }
        };

        return View(model);
    }

}

