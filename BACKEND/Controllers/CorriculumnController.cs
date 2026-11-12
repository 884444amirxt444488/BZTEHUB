
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BZTEHUB.Backend.Dtos;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]


public class CurriculumnController : ControllerBase
{
    private readonly Curriculumn curriculumn;

    public CurriculumnController(Curriculumn curriculumn)
    {
        this.curriculumn = curriculumn;
    }

    [Authorize]
    [HttpPost("AddCurriculumn")]
    public async Task<IActionResult> AddCurriculumn([FromBody] CurriculumnListDto curriculumnDto)
    {
        if (!int.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out int UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            List<CurriculumnResultDto> Result = await curriculumn.AddCurriculumn(UserId, curriculumnDto);
            return Created("api/Curriculumn/AddCurriculumn", new
            {
                Message = "برنامه درسی ساخته شد",
                Items = Result
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to add curriculumn: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
    [Authorize]
    [HttpPost("DeleteOneCurriculumn")]
    public async Task<IActionResult> DeleteOneUserCurriculumn([FromQuery] CurriculumnDeleteDto  curriculumnDeleteDto)
    {
        if (!int.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out int UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            await curriculumn.DeleteOneCurriculumn(UserId, curriculumnDeleteDto);
            return Ok(new
            {
                Message = "با موفقیت حذف شد"
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to delete curriculumn: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
    [Authorize]
    [HttpPost("DeleteAllCurriculumn")]
    public async Task<IActionResult> DeleteAllUserCurriculumn()
    {
        if (!int.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out int UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            await curriculumn.DeleteAllUserCurriculumn(UserId);
            return Ok(new
            {
                Message = "با موفقیت حذف شد"
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to delete curriculumn: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
    [Authorize]
    [HttpPost("EditCurriculumn")]
    public async Task<IActionResult> EditUserCurriculumn([FromBody] CurriculumnEditDto  curriculumnDeleteDto)
    {
        if (!int.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out int UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            await curriculumn.EditUserCurriculumn(UserId, curriculumnDeleteDto);
            return Ok(new
            {
                Message = "بروزرسانی انجام شد"
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to delete curriculumn: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
    [Authorize]
    [HttpPost("GetUserCurriculumn")]
    public async Task<IActionResult> GetUserCurriculumn()
    {
        if (!int.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out int UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            List<CurriculumnResultDto> Result = await curriculumn.GetUserCurriculumn(UserId);
            return Ok(new
            {
                Message = "ارسال شد",
                Items = Result 
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to delete curriculumn: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
}