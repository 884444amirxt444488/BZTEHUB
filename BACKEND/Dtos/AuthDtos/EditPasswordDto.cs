namespace BZTEHUB.Backend.Dtos.AuthDtos;



using System.ComponentModel.DataAnnotations;

public class EditPasswordDto
{
    [Required(ErrorMessage ="رمز عبور قبلی ضروری است")]
    [StringLength(16, MinimumLength =8, ErrorMessage ="رمز عبور قبلی باید بین 8و 16 کاراکتری باشد")]
    public string OldPassword {get; set;} = string.Empty;
    [Required(ErrorMessage ="رمز عبور جدیدی ضروری است")]
    [StringLength(16, MinimumLength =8, ErrorMessage ="رمز عبور جدید باید بین 8و 16 کاراکتر باشد")]
    public string NewPassword {get; set;} = string.Empty;
    [Required(ErrorMessage ="تاییدیه رمز عبور ضروری است")]
    [StringLength(16, MinimumLength =8, ErrorMessage ="تاییدیه رمز عبور باید بین 8و 16 کاراکتر باشد")]
    public string ConfirmPassword {get; set;} = string.Empty;
}















