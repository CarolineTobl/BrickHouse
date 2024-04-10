using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using BrickHouse.Models;
using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace BrickHouse.Controllers;

public class RoleController : Controller
{
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<IdentityUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public ViewResult Roles() => View(_roleManager.Roles);

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
    
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
                return RedirectToAction("Roles");
            else
                Errors(result);
        }
        return View(name);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Roles");
            else
                Errors(result);
        }
        else
            ModelState.AddModelError("", "No role found");
        return View("Roles", _roleManager.Roles);
    }
    
    public async Task<IActionResult> Update(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        
        // Materialize the user list
        List<IdentityUser> users = await _userManager.Users.ToListAsync();
        
        List<IdentityUser> members = new List<IdentityUser>();
        List<IdentityUser> nonMembers = new List<IdentityUser>();
        foreach (IdentityUser user in users)
        {
            var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
            list.Add(user);
        }
        return View(new RoleEdit
        {
            Role = role,
            Members = members,
            NonMembers = nonMembers
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleModification model)
    {
        IdentityResult result;
        if (ModelState.IsValid)
        {
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
            return RedirectToAction(nameof(Roles));
        else
            return await Update(model.RoleId);
    }
}