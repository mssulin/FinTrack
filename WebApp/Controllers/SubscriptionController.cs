using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("subscriptions")]
public class SubscriptionController(IWebHostEnvironment hostingEnvironment, ISubService subService, UserManager<UserEntity> userManager) : Controller
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
      
      foreach (var sub in subs)
      {
         if (DateTime.Now > sub.NextPaymentDate)
         {
            while (sub.NextPaymentDate <= DateTime.Now)
            {
               sub.NextPaymentDate = sub.Frequency switch
               {
                  "Månad"   => sub.NextPaymentDate.AddMonths(1),
                  "Kvartal" => sub.NextPaymentDate.AddMonths(3),
                  "År"      => sub.NextPaymentDate.AddYears(1),
                  _         => sub.NextPaymentDate
               };
            }

            await _subService.UpdateSubAsync(sub);
         }
      }

      var vm = subs.Select(s => new SubscriptionViewModel
         {
            Id = s.Id,
            Name = s.Name,
            Amount = s.Amount,
            LastPaymentDate = s.LastPaymentDate,
            NextPaymentDate = s.NextPaymentDate,
            Category = s.Category,
            Frequency = s.Frequency,
            ImageUrl = s.ImageUrl,
            IsPaid = s.IsPaid,
         })
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

      string? fileName = null;

      if (model.Image != null)
      {
         var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
         Directory.CreateDirectory(uploads);

         fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
         var filePath = Path.Combine(uploads, fileName);

         using (var stream = new FileStream(filePath, FileMode.Create))
         {
            await model.Image.CopyToAsync(stream);
         }
         
         model.ImageUrl = "/uploads/" + fileName;
      }
      
      var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
      
      var subscription = new SubscriptionEntity
      {
         Name = model.Name,
         Amount = model.Amount,
         LastPaymentDate = model.LastPaymentDate,
         Category = model.Category,
         Frequency = model.Frequency,
         ImageUrl = model.ImageUrl,
         UserId = userId!
      };
      
      subscription.NextPaymentDate = subscription.Frequency switch
      {
         "Månad"   => subscription.LastPaymentDate.AddMonths(1),
         "Kvartal" => subscription.LastPaymentDate.AddMonths(3),
         "År"      => subscription.LastPaymentDate.AddYears(1),
         _         => subscription.LastPaymentDate
      };
      
      await _subService.AddSubAsync(subscription);

      return RedirectToAction("Index", "Dashboard");
   }
   
   [HttpPost("delete/{id:int}")]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Delete(int id)
   {
      var sub = await _subService.GetSubByIdAsync(id);
      if (sub == null)
      {
         return NotFound();
      }

      await _subService.DeleteSubAsync(sub);

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