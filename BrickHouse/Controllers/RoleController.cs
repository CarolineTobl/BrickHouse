using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using BrickHouse.Models;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// INTEX II
// Group 2-2
// Garrett Ashcroft, Jared Rosenlund, Vivian Solgere, and Caroline Tobler

namespace BrickHouse.Controllers;

// All pages only accessible with admin rights, protects from direct URL navigation
[Authorize (Roles = "admin")]
public class RoleController : Controller
{
    // Create tools from Identity packages
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<IdentityUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // Shows all roles in dbo.AspNetRoles
    public ViewResult Roles() => View(_roleManager.Roles);

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
    
    public IActionResult Create() => View();
    
    // Add role to the database
    [HttpPost]
    public async Task<IActionResult> Create(string name) // Name is the only parameter, .NET generates a role ID
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
                // Take them back to the role summary page
                return RedirectToAction("Roles");
            else
                Errors(result);
        }
        return View(name);
    }
    
    // Remove role from database
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                // Take them back to role summary
                return RedirectToAction("Roles");
            else
                Errors(result);
        }
        else
            ModelState.AddModelError("", "No role found");
        return View("Roles", _roleManager.Roles);
    }
    
    // View to see all users as members and non-members of given role
    public async Task<IActionResult> Update(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        
        // Materialize a user list
        List<IdentityUser> users = await _userManager.Users.ToListAsync();
        
        // Initialize member and non-member list
        List<IdentityUser> members = new List<IdentityUser>();
        List<IdentityUser> nonMembers = new List<IdentityUser>();
        foreach (IdentityUser user in users)
        {
            var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
            list.Add(user);
        }
        
        // Return view with correct lists
        return View(new RoleEdit
        {
            Role = role,
            Members = members,
            NonMembers = nonMembers
        });
    }
    
    // Assign users to given role
    [HttpPost]
    public async Task<IActionResult> Update(RoleModification model)
    {
        IdentityResult result;
        if (ModelState.IsValid)
        {
            // Add selected users to role
            foreach (string userId in model.AddIds ?? new string[] { })
            {
                IdentityUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
            
            // Delete deselected users from role
            foreach (string userId in model.DeleteIds ?? new string[] { })
            {
                IdentityUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
        }

        if (ModelState.IsValid)
            // Send them back to roles summary
            return RedirectToAction(nameof(Roles));
        else
            return await Update(model.RoleId);
    }
}