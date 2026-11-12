
using Npgsql;

public class PublicDatas
{
    private readonly DataBaseService db;

    public PublicDatas(DataBaseService db)
    {
        this.db = db;
    }

    public async Task<List<UniversityMembersDto>> GetUniversityMembers()
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        List<UniversityMembersDto> PublicDatas = new();

        await using(NpgsqlCommand command1 = new NpgsqlCommand("SELECT um.id, um.um_name, um.um_phone, um.um_tel_id, um.um_bale_id, um.um_email, um.um_sex, COALESCE(ARRAY_AGG(umar.um_member_academic_rank::text) FILTER (WHERE umar.id IS NOT NULL), ARRAY[]::text[]) FROM university_members AS um LEFT JOIN university_members_academic_rank AS umar ON um.id = umar.um_member_id GROUP BY um.id, um.um_name, um.um_phone, um.um_tel_id, um.um_bale_id, um.um_email, um.um_sex ORDER BY id ASC", connection))
        {
            await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                while(await reader1.ReadAsync())
                {
                    PublicDatas.Add(new UniversityMembersDto
                    {
                        id = reader1.GetInt64(0),
                        um_name = reader1.IsDBNull(1) ? null :  reader1.GetString(1),
                        um_phone = reader1.IsDBNull(2) ? null :  reader1.GetString(2),
                        um_tel_id = reader1.IsDBNull(3) ? null :  reader1.GetString(3),
                        um_bale_id = reader1.IsDBNull(4) ? null :  reader1.GetString(4),
                        um_email = reader1.IsDBNull(5) ? null : reader1.GetString(5),
                        um_sex = reader1.IsDBNull(6) ? null : reader1.GetString(6),
                        um_academic_ranks = reader1.GetFieldValue<string[]>(7).ToList()
                    }
                    );  
                }
            }
        }
        return PublicDatas;
    }

    public async Task<List<LinksDto>> GetAllLins()
    {
        List<LinksDto> links = new();

        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using(NpgsqlCommand command1 = new NpgsqlCommand("SELECT id, source_with_name, source_link, description FROM source_links", connection))
        {
            await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                while(await reader1.ReadAsync())
                {
                    links.Add(new LinksDto
                    {
                        Id = reader1.GetInt64(0),
                        SourceName = reader1.GetString(1),
                        Link = reader1.GetString(2),
                        Description = reader1.IsDBNull(3) ? null : reader1.GetString(3)
                    });
                }
            }
        }
        return links;
    }
}











