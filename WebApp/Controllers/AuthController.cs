using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;

    public AuthController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View(new SignInViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,               
            model.Password,             
            isPersistent: false,         
            lockoutOnFailure: false    
        );

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Dashboard"); 
        }

        ModelState.AddModelError("", "Fel email eller lösenord");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}