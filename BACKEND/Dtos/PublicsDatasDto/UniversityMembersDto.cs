


public class UniversityMembersDto
{
    public long id {get; set;} = 0;
    public string? um_name {get; set;} = string.Empty;
    public string? um_phone {get; set;} = string.Empty;
    public string? um_tel_id {get; set;} = string.Empty;
    public string? um_bale_id {get; set;} = string.Empty;
    public string? um_email {get; set;} = string.Empty;
    public string? um_sex {get; set;} = string.Empty;
    public List<string> um_academic_ranks {get; set;} = new();
}


