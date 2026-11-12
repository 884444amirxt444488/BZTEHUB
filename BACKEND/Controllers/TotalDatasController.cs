


using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("")]

public class TotlaDataController : ControllerBase
{
    private readonly PublicDatas publicDatas;

    public TotlaDataController(PublicDatas publicDatas)
    {
        this.publicDatas = publicDatas;
    }


    [HttpGet("univers/members")]
    public async Task<IActionResult> GetUniversMembers()
    {
        try
        {
            List<UniversityMembersDto> Result = await publicDatas.GetUniversityMembers();
            return Ok(new
            {
                UniverseMembers = Result
            });
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to get universe members: {Err}");
            return StatusCode(500, "مشکلی پیش امد. دوباره امتحان کنید");
        }
    }

    [HttpGet("source/links")]
    public async Task<IActionResult> GetSourceLinks()
    {
        try
        {
            List<LinksDto> Result = await publicDatas.GetAllLins();
            return Ok(new
            {
                SourcesLinks = Result
            });
        }
        catch (Exception Err)
        {
            Console.WriteLine($"Error to get universe members: {Err}");
            return StatusCode(500, "مشکلی پیش امد. دوباره امتحان کنید");
        }
    }



}











