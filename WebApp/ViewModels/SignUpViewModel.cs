using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "E-post krävs")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress")]
    [Display(Name = "E-post")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Förnamn krävs")]
    [Display(Name = "Förnamn")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Efternamn krävs")]
    [Display(Name = "Efternamn")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Lösenord krävs")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Bekräfta lösenord")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Lösenorden matchar inte")]
    [Display(Name = "Bekräfta lösenord")]
    public string ConfirmPassword { get; set; } = null!;
}