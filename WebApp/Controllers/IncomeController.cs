using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Mappers;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class IncomeController(IIncomeService incomeService, UserManager<UserEntity> userManager) : Controller
{
    private readonly IIncomeService _incomeService = incomeService;
    private readonly UserManager<UserEntity> _userManager = userManager;


    [HttpPost]
    public async Task<IActionResult> AddIncome(DashboardViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Dashboard");

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("SignIn", "Auth");
        
        var entity = IncomeMapper.ToEntity(model, user.Id);

        await _incomeService.AddIncomeAsync(entity);

        return RedirectToAction("Index", "Dashboard");
    }
    
}