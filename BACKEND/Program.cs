


using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var Builder = WebApplication.CreateBuilder(args);

Builder.Services.AddControllers();
Builder.Services.AddScoped<DataBaseService>();
Builder.Services.AddScoped<Auth>();
Builder.Services.AddScoped<JwtService>();
Builder.Services.AddScoped<PublicDatas>();
Builder.Services.AddScoped<Curriculumn>();
Builder.Services.AddAuthorization();

Builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Builder.Configuration["JwtProperties:Issuer"],

        ValidateAudience = true,
        ValidAudience = Builder.Configuration["JwtProperties:Audience"],

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Builder.Configuration["JwtProperties:Key"]!)),

        ClockSkew = TimeSpan.Zero
    };
});

Builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("RateLimiter", limiter =>
    {
        limiter.PermitLimit = 20;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
        limiter.AutoReplenishment = true;
    });
});

var App = Builder.Build();


App.UseRouting();
App.UseRateLimiter();
App.UseAuthentication();
App.UseAuthorization();


App.MapControllers();

App.Run();





