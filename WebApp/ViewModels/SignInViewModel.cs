using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignInViewModel
{
    [Required(ErrorMessage = "E-post krävs")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress")]
    [Display(Name = "E-post")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Lösenord krävs")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = null!;
}