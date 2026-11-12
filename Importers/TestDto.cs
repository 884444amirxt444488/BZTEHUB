

public class UniversityMembersDto1
{
    public long id {get; set;} = 0;
    public string? um_name {get; set;} = string.Empty;
    public string? um_phone {get; set;} = string.Empty;
    public string? um_tel_id {get; set;} = string.Empty;
    public string? um_bale_id {get; set;} = string.Empty;
    public string? um_email {get; set;} = string.Empty;
    public string? um_sex {get; set;} = string.Empty;
    public List<UniversityMembersRanksDto1> um_academic_ranks {get; set;} = new();
}



public class UniversityMembersRanksDto1
{
    public long um_member_id {get; set;} = 0;
    public string um_member_academic_rank {get; set;} = string.Empty;
}


public class UniversityLinks
{
    public string source_with_name {get; set;} = string.Empty;
    public string source_link {get; set;} = string.Empty;
    public string description {get; set;} = string.Empty;
}




