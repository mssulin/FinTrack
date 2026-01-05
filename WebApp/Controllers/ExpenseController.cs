using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;


[Authorize]
public class ExpenseController(IExpenseService expenseService, UserManager<UserEntity> userManager)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Add(ExpenseViewModel model)
    {
        
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Dashboard");

        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("SignIn", "Auth");

        var expense = new ExpenseEntity
        {
            Date = DateTime.Now,
            Amount = model.Amount,
            Category = model.Category,
            UserId = user!.Id
        };

        await expenseService.AddExpenseAsync(expense);

        return RedirectToAction("Index", "Dashboard");
    }
}