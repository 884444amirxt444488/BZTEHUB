using System.ComponentModel.DataAnnotations;
namespace BZTEHUB.Backend.Dtos.AuthDtos;

public class EditEmailDto
{
    private string _email = string.Empty;
    [Required(ErrorMessage ="ایمیل ضروری است")]
    [StringLength(60, MinimumLength =4, ErrorMessage ="ایمیل باید بین 4و 60 کاراکتر باشد")]
    [EmailAddress(ErrorMessage ="شکل ایمیل نامعتبر است")]
    public string Email
    {
        get => _email;
        set => _email = value.Trim().ToLowerInvariant() ?? string.Empty;
    }
}