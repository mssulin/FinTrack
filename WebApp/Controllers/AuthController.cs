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
    public IActionResult Index()
    {
        return View(new SignInViewModel());
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View("Index", new SignInViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

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

        ModelState.AddModelError("", "Fel e-post eller lösenord");
        return View("Index", model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}