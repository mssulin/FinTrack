using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("fintrack")]
public class DashboardController(IDashboardService dashboardService, UserManager<UserEntity> _userManager)
    : Controller
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("dashboard")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("SignIn", "Auth");
        
        var dto = await _dashboardService.GetDashboardAsync(user!.Id);

        var viewModel = new DashboardViewModel
        {
            TotalIncome = dto.TotalIncome,
            TotalExpenses = dto.TotalExpenses,
            AvailableMoney = dto.AvailableMoney,
            Expenses = dto.Expenses.Select(e => new ExpenseViewModel
            {
                Id = e.Id,
                Category = e.Category,
                Amount = e.Amount,
            }).ToList(),
            Subscriptions = dto.Subscriptions
                .Select(s => new SubscriptionViewModel
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Amount = s.Amount,
                Category = s.Category,
                Frequency = s.Frequency,
                LastPaymentDate = s.LastPaymentDate,
                NextPaymentDate = s.NextPaymentDate,
                IsPaid = s.IsPaid
                
            }).ToList(),
            Savings = dto.Savings
                .OrderByDescending(s => s.TargetAmount > 0
                ? (decimal)s.CurrentAmount / s.TargetAmount : 0)
                .Select(s => new SavingViewModel
            {
                Id = s.Id,
                Title = s.Title,
                CurrentAmount = s.CurrentAmount,
                TargetAmount = s.TargetAmount
            }).ToList(),
            CurrentMonth = dto.CurrentMonth,
            SavingPercent = dto.SavingPercent,
            ExpenseCategories = dto.ExpenseCategories,
            IncomeSources = dto.IncomeSources,
        };
        
        return View(viewModel);
    }
}