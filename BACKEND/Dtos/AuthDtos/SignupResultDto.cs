namespace BZTEHUB.Backend.Dtos.AuthDtos;



public class SignupResultDto
{
    public string Email {get; set;} = string.Empty;
    public string AccessToken {get; set;} = string.Empty;
    public string RefreshToken {get; set;} = string.Empty;
    public DateTime dateTime {get; set;} = DateTime.UtcNow;
}







