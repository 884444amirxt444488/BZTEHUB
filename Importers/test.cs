using BZTEHUB.Importers.DBConnection;
using Npgsql;

namespace BZTEHUB.Importers.Test;

public class Test
{
    DataBaseService db = new DataBaseService();

    public async Task AddUniversityMembers()
    {
        
        List<UniversityMembersDto1> strings = new();

        strings.AddRange(new List<UniversityMembersDto1>
        {
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "سمن شیشه چی",
                um_phone = "09034704466",
                um_tel_id = "@Drshishechi",
                um_bale_id = null,
                um_email = "",
                um_sex = "Femail",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"   
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "سید رخمان موسوی",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "غلامرضا صفیعی",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "سید یاشار بنی هاشم",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    },
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "مدیر پژوهش"
                    }
                }        
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "جعفر عبدی بویینی",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "حسین ساسانیان",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "سارا حشمتیان",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "Femail",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "زهرا امیرزاده",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "Femail",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "کشتکار مزوجی",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "MAIL",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
            new UniversityMembersDto1
            {
                id = 1,
                um_name = "ماعده عشرتی",
                um_phone = null,
                um_tel_id = null,
                um_bale_id = null,
                um_email = null,
                um_sex = "Femail",
                um_academic_ranks = new List<UniversityMembersRanksDto1>
                {
                    new UniversityMembersRanksDto1
                    {
                        um_member_id = 0,
                        um_member_academic_rank = "استاد"
                    }
                }
            },
        });


        await using NpgsqlConnection connection = db.CreateDb();
        await connection.OpenAsync();



        foreach (UniversityMembersDto1 Member in strings)
        {
            await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM university_members WHERE um_name = @um_name", connection, transaction))
                {
                    command.Parameters.AddWithValue("um_name", Member.um_name!);

                    Object? IsExxists = await command.ExecuteScalarAsync();

                    if (IsExxists != null)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"{Member.um_name} is exists. next");
                        continue;
                    }
                }

                long MemberId = 0;

                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO university_members(um_name, um_phone, um_tel_id, um_bale_id, um_email, um_sex) VALUES(@um_name, @um_phone, @um_tel_id, @um_bale_id, @um_email, @um_sex) RETURNING id", connection, transaction))
                {
                    command.Parameters.AddWithValue("um_name", (Object?) Member.um_name ?? DBNull.Value);
                    command.Parameters.AddWithValue("um_phone", (Object?) Member.um_phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("um_tel_id", (Object?) Member.um_tel_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("um_bale_id", (Object?) Member.um_bale_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("um_email", (Object?) Member.um_email ?? DBNull.Value);
                    command.Parameters.AddWithValue("um_sex", (Object?) Member.um_sex ?? DBNull.Value);


                    Object? IsAdded = await command.ExecuteScalarAsync();

                    if (IsAdded == null)
                    {
                        Console.WriteLine("Somthing went wrong");
                        throw new Exception("Not added");
                    }
                    else
                    {
                        MemberId = Convert.ToInt64(IsAdded);
                    }
                }


                foreach (UniversityMembersRanksDto1 UM in Member.um_academic_ranks)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO university_members_academic_rank(um_member_id, um_member_academic_rank) VALUES(@um_member_id ,@um_member_academic_rank) RETURNING id", connection, transaction))
                    {
                        command.Parameters.AddWithValue("um_member_id", MemberId);
                        command.Parameters.AddWithValue("um_member_academic_rank", UM.um_member_academic_rank);

                        int IsSet = await command.ExecuteNonQueryAsync();

                        if (IsSet == 0)
                        {
                            Console.WriteLine("Error to add ranks");
                            throw new Exception("Can not add");
                        }
                    }        
                }
                await transaction.CommitAsync();
                Console.WriteLine($"Added to db successflly: {Member.um_name}");
            }
            catch (Exception Err)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error to add univers members: {Err}");
            }
        }
    }
    public async Task AddLinks()
    {
        List<UniversityLinks> universityLinks = new();

        
        universityLinks.AddRange(
            new UniversityLinks
            {
                source_with_name = "جزوه معادلات دیفرانسیل",
                source_link = "https://t.me/amirarshiakoroshFSB/5",
                description = "کتاب معادلات دیفرانسیل نشر پیدا کرده توسط دکتر مهدی رضایی"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه دفاع مقدس",
                source_link = "https://t.me/amirarshiakoroshFSB/6",
                description = "جزوه دفاع مقدس نشر پیدا کرده توسط محمد علی مطلبی کربکندی و حمید ولدی"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه حقوق اجتماعی سیاسی",
                source_link = "https://t.me/amirarshiakoroshFSB/7",
                description = "کتاب حقوق اجتماعی سیاسی نشر پیدا کرده توسط حسین جوان اراسته"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه دانش خانواده و جمعیت",
                source_link = "https://t.me/amirarshiakoroshFSB/8",
                description = "کتاب دانش خانواده و جمعیت نشر پیدا شده توسط نامشخص"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه ریاضی عمومی 1",
                source_link = "https://t.me/amirarshiakoroshFSB/9",
                description = "کتاب ریاضی عمومی یک نشر پیدا کرده توسط دانشگاه پیام نور"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه انتگرال",
                source_link = "https://t.me/amirarshiakoroshFSB/10",
                description = "کتاب انتگرال نشر پیدا کرده توسط حسین ایزن"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه الگوریتم ام ای تی",
                source_link = "https://t.me/amirarshiakoroshFSB/11",
                description = "کتاب الگوریتم ام ای تی نشر پیدا کرده توسط ام ای تی"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه مبانی کامپیوتر",
                source_link = "https://t.me/amirarshiakoroshFSB/13",
                description = "جزوه مبانی کامپیوتر نشر پیدا کرده توسط دانشجو"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه حل المسایل فیزیک هالیدی",
                source_link = "https://t.me/amirarshiakoroshFSB/15",
                description = "کتاب جامع مسایل فیزیک هالیدی نشر پیدا کرده توسط رابرت زرنیک و جرل واکر"
            },
            new UniversityLinks
            {
                source_with_name = "جزوه تمرین های سی پلاس پلاس",
                source_link = "https://t.me/amirarshiakoroshFSB/16",
                description = "کتاب تمرینات سی پلاس پلاس نشر پیدا کرده توسط علامرضا رحیمی"
            }
        );


        using NpgsqlConnection connection = db.CreateDb();

        await connection.OpenAsync();

        foreach (UniversityLinks UL in universityLinks)
        {
            await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();
            try
            {
                await using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT * FROM source_links WHERE source_with_name = @source_with_name", connection, transaction))
                {
                    command1.Parameters.AddWithValue("source_with_name", UL.source_with_name);

                    Object? IsChaned = await command1.ExecuteScalarAsync();

                    if (IsChaned != null)
                    {
                        Console.WriteLine($"This book already exists: {UL.source_with_name}");
                        throw new Exception("Not unique");
                    }
                }
                await using (NpgsqlCommand command2 = new NpgsqlCommand("INSERT INTO source_links(source_with_name, source_link, description) VALUES(@source_with_name, @source_link, @description)", connection, transaction))
                {
                    command2.Parameters.AddWithValue("source_with_name", UL.source_with_name);
                    command2.Parameters.AddWithValue("source_link", UL.source_link);
                    command2.Parameters.AddWithValue("description", UL.description);

                    int IsChaned = await command2.ExecuteNonQueryAsync();

                    if (IsChaned == 0)
                    {
                        Console.WriteLine($"Error to add link: {UL.source_with_name}");
                        throw new Exception("Can not add");
                    }
                }
                await transaction.CommitAsync();
                Console.WriteLine("Added successfully");
            }
            catch (Exception Err)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error to add links: {Err}");
                break;
            }
        }
    }
}