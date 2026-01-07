using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers;

[AllowAnonymous]
[Route("")]
public class AuthController : Controller
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;

    public AuthController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        return View(new SignInViewModel());
    }

    [HttpPost("signin")]
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

        ModelState.AddModelError("", "Fel e-postadress eller lösenord");
        return View(model);
    }
    

    [HttpGet("signup")]
    public IActionResult SignUp()
    {
        return View(new SignUpViewModel());
    }

    [HttpPost("signup")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new UserEntity
        {
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
           
            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, err.Description);

            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        TempData["SuccessMessage"] = "Ditt konto har skapats! Välkommen till FinTrack.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}