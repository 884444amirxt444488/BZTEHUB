
namespace BZTEHUB.Backend.Dtos;

using System.ComponentModel.DataAnnotations;

public class CurriculumnDto
{
    [Required(ErrorMessage ="اسم درس ضروری است")]
    [StringLength(20, MinimumLength =3, ErrorMessage ="درس باید بین 3 و 20 کاراکتر باشد")]
    public string lession_name {get; set;} = string.Empty;
    [Required(ErrorMessage ="روز کلاس ضروری است")]
    [Range(0, 6, ErrorMessage ="روز کلاس باید روز هفته باشد")]
    public int day_of_class {get; set;} = 0;
    [Required(ErrorMessage ="زمان شروع کلاس ضروری است")]
    public TimeOnly start_class {get; set;} = TimeOnly.MinValue;
    [Required(ErrorMessage ="شماره ی کلاس ضروری است")]
    [Range(1, 1000, ErrorMessage ="شماره ی کلاس می تواند بین 1 تا 1000 باشد")]
    public int class_loc {get; set;} = 0;
}

public class CurriculumnListDto
{
    public List<CurriculumnDto> curriculumnDtos {get; set;} = new();
}

public class CurriculumnResultDto
{
    public long Id {get; set;} = 0;
    public string lession_name {get; set;} = string.Empty;
    public int day_of_class {get; set;} = 0;
    public TimeOnly start_class {get; set;} = TimeOnly.MinValue;
    public TimeOnly finish_class {get; set;} = TimeOnly.MinValue;
    public int class_loc {get; set;} = 0;
}

public class CurriculumnDeleteDto
{
    [Required(ErrorMessage ="ایدی ارسال نشده است")]
    [Range(1, 10000, ErrorMessage ="ایدی نا معتبر است")]
    public long Id {get; set;} = 0;
}

public class CurriculumnEditDto : IValidatableObject
{
    [Required(ErrorMessage ="ایدی ضروری است")]
    [Range(1, 1000, ErrorMessage ="ایدی نا معتبر است")]
    public long Id {get; set;} = 0;
    [StringLength(20, MinimumLength =3, ErrorMessage ="اسم درس باید بین 3و 20 کاراکتر باشد")]
    public string? lession_name {get; set;} = string.Empty;
    [Range(0, 6, ErrorMessage ="روز کلاس باید روز هفته باشد")]
    public int? day_of_class {get; set;} = null;
    public TimeOnly? start_class {get; set;} = null;
    [Range(1, 1000, ErrorMessage ="شماره ی کلاس می تواند بین 1 تا 1000 باشد")]
    public int? class_loc {get; set;} = null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(lession_name ) && day_of_class == null && start_class == null && class_loc == null)
        {
            yield return new ValidationResult(
                "چیزی برای بروزرسانی ارسال نشده",
                new[] {nameof(lession_name), nameof(day_of_class), nameof(start_class), nameof(class_loc)}
            );
        }
    }

}






