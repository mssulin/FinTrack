using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Mappers;

namespace WebApp.Controllers;

[Authorize]
[Route("dashboard")]
public class DashboardController(
    IDashboardService dashboardService,
    UserManager<UserEntity> userManager)
    : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("SignIn", "Auth");

        var dto = await dashboardService.GetDashboardAsync(user.Id);
        var viewModel = DashboardMapper.ToViewModel(dto);

        return View(viewModel);
    }
}