using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Mappers;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("subscriptions")]
public class SubscriptionController(
    IWebHostEnvironment hostingEnvironment,
    ISubService subService,
    UserManager<UserEntity> userManager) : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
    private readonly ISubService _subService = subService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("SignIn", "Auth");

        var subs = await _subService.GetSubsAsync();

        var vm = subs
            .Select(SubscriptionMapper.ToViewModel)
            .OrderBy(s => s.NextPaymentDate)
            .ToList();

        return View(vm);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSub(SubscriptionViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Dashboard");

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        if (model.Image != null)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(uploads, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.Image.CopyToAsync(stream);

            model.ImageUrl = "/uploads/" + fileName;
        }

        model.NextPaymentDate = model.Frequency switch
        {
            "Månad" => model.LastPaymentDate.AddMonths(1),
            "Kvartal" => model.LastPaymentDate.AddMonths(3),
            "År" => model.LastPaymentDate.AddYears(1),
            _ => model.LastPaymentDate
        };

        var subscription = SubscriptionMapper.ToDto(model);

        await _subService.AddSubAsync(subscription, userId);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost("update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SubscriptionViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        var subscription = SubscriptionMapper.ToDto(model);

        await _subService.UpdateSubAsync(model.Id, subscription, userId);

        return RedirectToAction("Index");
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return RedirectToAction("SignIn", "Auth");

        await _subService.DeleteSubAsync(id, userId);

        return RedirectToAction("Index");
    }

    [HttpPost("paid/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Paid(int id)
    {
        await _subService.MarkPaidAsync(id);

        return RedirectToAction("Index", "Dashboard");
    }
}