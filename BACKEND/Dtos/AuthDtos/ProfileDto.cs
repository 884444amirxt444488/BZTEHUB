namespace BZTEHUB.Backend.Dtos.AuthDtos;


public class ProfileResultDto
{
    public string Email {get; set;} = string.Empty;
    public DateTime dateTime {get; set;} = DateTime.UtcNow;
}