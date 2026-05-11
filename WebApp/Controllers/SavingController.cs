using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Mappers;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("savings")]
public class SavingController(
    ISavingService savingService,
    UserManager<UserEntity> userManager) : Controller
{
    private readonly ISavingService _savingService = savingService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("SignIn", "Auth");

        var savings = await _savingService.GetAllSavingsAsync(user.Id);

        var model = savings
            .OrderByDescending(s => s.TargetAmount > 0
                ? s.CurrentAmount / s.TargetAmount
                : 0)
            .Select(s => new SavingViewModel
            {
                Id = s.Id,
                Title = s.Title,
                CurrentAmount = s.CurrentAmount,
                TargetAmount = s.TargetAmount
            })
            .ToList();

        return View(model);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSavingGoal(SavingViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        var saving = new SavingDto
        {
            Title = model.Title,
            CurrentAmount = model.CurrentAmount,
            TargetAmount = model.TargetAmount
        };

        await _savingService.CreateSavingGoalAsync(saving, userId);

        return RedirectToAction("Index", "Saving");
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToSaving(AddToSavingViewModel model)
    {
        await _savingService.AddToSavingAsync(model.SavingId, model.Amount);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(SavingViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        var saving = SavingMapper.ToDto(model);

        await _savingService.UpdateSavingAsync(model.Id, saving, userId);

        return RedirectToAction("Index");
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> DeleteSaving(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        await _savingService.DeleteSavingAsync(id, userId);

        return RedirectToAction("Index");
    }

    [HttpGet("history")]
    public async Task<IActionResult> History(int savingId)
    {
        var history = await _savingService.GetSavingHistoryBySavingIdAsync(savingId);

        var model = history.Select(h => new SavingHistoryViewModel
        {
            Id = h.Id,
            SavingId = h.SavingId,
            Amount = h.Amount,
            Date = h.Date,
            Type = h.Type
        }).ToList();

        return PartialView("Partials/Lists/_SavingHistoryList", model);
    }
}