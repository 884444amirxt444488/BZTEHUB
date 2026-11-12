namespace BZTEHUB.Backend.Dtos.AuthDtos;


using System.ComponentModel.DataAnnotations;

public class SignupDto
{
    private string _email = string.Empty;

    [Required(ErrorMessage ="ایمیل ضروری است")]
    [StringLength(60, MinimumLength =4, ErrorMessage ="ایمیل باید بین 4 یا 60 کاراکتر باشد")]
    [EmailAddress(ErrorMessage ="شکل ایمیل نامعتبر است")]
    public string Email
    {
        get => _email;
        set => _email = value.Trim().ToLowerInvariant() ?? string.Empty;
    }

    [Required(ErrorMessage ="رمز عبور ضروری است")]
    [StringLength(16, MinimumLength =8, ErrorMessage ="رمز عبور بای بین 8 یا 16 کاراکتر باشد")]
    public string Password {get; set;} = string.Empty;
}











