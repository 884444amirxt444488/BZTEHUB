


using System.IdentityModel.Tokens.Jwt;
using BZTEHUB.Backend.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]


public class AuthController : ControllerBase
{
    private readonly Auth auth;

    public AuthController(Auth auth)
    {
        this.auth = auth;
    }

    [EnableRateLimiting("RateLimiter")]
    [HttpPost("Signup")]
    public async Task<IActionResult> Singup([FromBody] SignupDto NewUser)
    {
        try
        {
            SignupResultDto Result = await auth.Signup(NewUser);
            Response.Cookies.Append(
                "RefreshToken",
                Result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    Path = "/api/Auth/RefreshToken"
                }
            );
            return Created("api/Signup", new
            {
                Message = "ثبت نام با موفقیت انجام شد",
                Email = Result.Email,
                Created_at = Result.dateTime,
                AccessToken = Result.AccessToken
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to signup: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }

    [EnableRateLimiting("RateLimiter")]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto User)
    {
        try
        {
            LoginResultDto Result = await auth.Login(User);
            Response.Cookies.Append(
                "RefreshToken",
                Result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    Path = "/api/Auth/RefreshToken"
                }
            );
            return Ok(new
            {
                Message = "ورود با موفقت انجام شد",
                Email = Result.Email,
                Created_at = Result.dateTime,
                AccessToken = Result.AccessToken
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to login: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    }
    [HttpPost("RefreshToken/Logout")]
    public async Task<IActionResult> Logout()
    {
        string RefreshToken = Request.Cookies["RefreshToken"]!;
        try
        {
            await auth.Logout(RefreshToken);
            Response.Cookies.Delete(
                "RefreshToken",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/Auth/RefreshToken"
                }
            );
            return Ok(new
            {
                Message = "خروج با موفقیت انجام شد",
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to logout: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    } 


    [Authorize]
    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        if (!long.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out long UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            ProfileResultDto Result = await auth.Profile(UserId);
            return Ok(new
            {
                Message = "دخول با موفقیت انجام شد",
                Email = Result.Email,
                Created_at = Result.dateTime
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to get profile: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    } 

    [EnableRateLimiting("RateLimiter")]
    [Authorize]
    [HttpPatch("EditEmail")]
    public async Task<IActionResult> EditEmail([FromBody] EditEmailDto EditEmail)
    {
        if (!long.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out long UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            EditEmailResultDto Result = await auth.EditEmail(UserId, EditEmail);
            return Ok(new
            {
                Message = "ایمیل شما با موفقیت  بروزرسانی شد",
                Email = Result.Email
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to edit email: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    } 

    [EnableRateLimiting("RateLimiter")]
    [Authorize]
    [HttpPost("EditPassword")]
    public async Task<IActionResult> EditPassword([FromBody] EditPasswordDto EditPass)
    {
        if (!long.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out long UserId))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            string RefreshToken = await auth.EditPassword(UserId, EditPass);
            Response.Cookies.Append(
                "RefreshToken",
                RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    Path = "/api/Auth/RefreshToken"
                }
            );
            return Ok(new
            {
                Message = "رمز عبور شما با موفقیت تغییر کرد"
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to edit password: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    } 

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        string? RefreshToken = Request.Cookies["RefreshToken"];
        if (string.IsNullOrWhiteSpace(RefreshToken))
        {
            return Unauthorized("ورود نکرده اید. ابتدا ورود کنید");
        }
        try
        {
            RefreshTokenDto Result = await auth.RefreshToken(RefreshToken);
            Response.Cookies.Append(
                "RefreshToken",
                Result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    Path = "/api/Auth/RefreshToken"
                }
            );
            return Ok(new
            {
                Message = "توکن ارسال شد",
                AccessToken = Result.AccessToken
            });
        }
        catch (AppError ApE)
        {
            return StatusCode(ApE.StatusCode, ApE.Message);
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to refreshtoken: {Err}");
            return StatusCode(500, "اررور از طرف سرور دریافت شد. بعدا دوباره امتحان کنید");
        }
    } 
}











