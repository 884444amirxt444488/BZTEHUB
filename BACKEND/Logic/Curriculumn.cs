


using BZTEHUB.Backend.Dtos;
using Npgsql;

public class Curriculumn
{
    private readonly DataBaseService db;

    public Curriculumn(DataBaseService db)
    {
        this.db = db;
    }

    public async Task<List<CurriculumnResultDto>> AddCurriculumn(int UserId, CurriculumnListDto curriculumns)
    {
        if (curriculumns?.curriculumnDtos == null || curriculumns.curriculumnDtos.Count == 0)
        {
            throw new AppError(400, "چیزی برای افزودن وجود ندارد");
        }

        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            await using (NpgsqlCommand lockCommand = new NpgsqlCommand(
                "SELECT id FROM users WHERE id = @userid FOR UPDATE",
                connection, transaction))
            {
                lockCommand.Parameters.AddWithValue("userid", UserId);

                object? LockedUserId = await lockCommand.ExecuteScalarAsync();

                if (LockedUserId == null)
                {
                    throw new AppError(404, "کاربر وجود ندارد");
                }
            }
            await using (NpgsqlCommand command3 = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM unique_curriculumn WHERE user_id = @userid)", connection, transaction))
            {
                command3.Parameters.AddWithValue("userid", UserId);

                bool IsFound = (bool)(await command3.ExecuteScalarAsync() ?? false);

                if (IsFound)
                {
                    throw new AppError(403, "شما برنامه درسی را قبلا ثبت کرده اید");
                }
            }

            await using (NpgsqlCommand command4 = new NpgsqlCommand("INSERT INTO unique_curriculumn(user_id) VALUES(@userid)", connection, transaction))
            {
                command4.Parameters.AddWithValue("userid", UserId);

                int IsAddSuccessfully = await command4.ExecuteNonQueryAsync();

                if (IsAddSuccessfully == 0)
                {
                    throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                }
            }

            List<CurriculumnResultDto> UserCurriculumn = new();

            foreach (CurriculumnDto cr in curriculumns.curriculumnDtos)
            {
                await using (NpgsqlCommand command4 = new NpgsqlCommand("INSERT INTO curriculum(userid, lesson_name, day_of_class, start_class, class_loc) VALUES(@userid, @lesson_name, @day_of_class, @start_class, @class_loc) RETURNING id, lesson_name, day_of_class, start_class, class_loc", connection, transaction))
                {
                    command4.Parameters.AddWithValue("userid", UserId);
                    command4.Parameters.AddWithValue("lesson_name", cr.lession_name);
                    command4.Parameters.AddWithValue("day_of_class", cr.day_of_class);
                    command4.Parameters.AddWithValue("start_class", cr.start_class);
                    command4.Parameters.AddWithValue("class_loc", cr.class_loc);

                    await using (NpgsqlDataReader reader1 = await command4.ExecuteReaderAsync())
                    {
                        if (!await reader1.ReadAsync())
                        {
                            throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                        }
                        UserCurriculumn.Add(new CurriculumnResultDto
                        {
                            Id = reader1.GetInt64(0),
                            lession_name = reader1.GetString(1),
                            day_of_class = reader1.GetInt16(2),
                            start_class = reader1.GetFieldValue<TimeOnly>(3),
                            finish_class = reader1.GetFieldValue<TimeOnly>(3).AddHours(2),
                            class_loc = reader1.GetInt16(4)
                        }
                        );
                    }
                }
            }
            await transaction.CommitAsync();
            return UserCurriculumn;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<bool> DeleteOneCurriculumn(int UserId, CurriculumnDeleteDto CRI)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            await using (NpgsqlCommand lockCommand = new NpgsqlCommand(
                "SELECT id FROM users WHERE id = @userid FOR UPDATE",
                connection, transaction))
            {
                lockCommand.Parameters.AddWithValue("userid", UserId);

                object? LockedUserId = await lockCommand.ExecuteScalarAsync();

                if (LockedUserId == null)
                {
                    throw new AppError(404, "کاربر وجود ندارد");
                }
            }
            await using (NpgsqlCommand command1 = new NpgsqlCommand("DELETE FROM curriculum WHERE userid = @userid AND id = @id", connection, transaction))
            {
                command1.Parameters.AddWithValue("userid", UserId);
                command1.Parameters.AddWithValue("id", CRI.Id);

                int IsChanged = await command1.ExecuteNonQueryAsync();

                if (IsChanged == 0)
                {
                    throw new AppError(404, "این درس وجود ندارد");
                }
            }
            await using (NpgsqlCommand command2 = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM curriculum WHERE userid = @userid)", connection, transaction))
            {
                command2.Parameters.AddWithValue("userid", UserId);

                bool IsFound = (bool)(await command2.ExecuteScalarAsync() ?? false);

                if (IsFound)
                {
                    await transaction.CommitAsync();
                    return true;
                }
            }
            await using (NpgsqlCommand command3 = new NpgsqlCommand("DELETE FROM unique_curriculumn WHERE user_id = @userid", connection, transaction))
            {
                command3.Parameters.AddWithValue("userid", UserId);

                int IsChanged = await command3.ExecuteNonQueryAsync();

                if (IsChanged == 0)
                {
                    throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                }
            }
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<bool> DeleteAllUserCurriculumn(int UserId)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            await using (NpgsqlCommand lockCommand = new NpgsqlCommand(
                "SELECT id FROM users WHERE id = @userid FOR UPDATE",
                connection, transaction))
            {
                lockCommand.Parameters.AddWithValue("userid", UserId);

                object? LockedUserId = await lockCommand.ExecuteScalarAsync();

                if (LockedUserId == null)
                {
                    throw new AppError(404, "کاربر وجود ندارد");
                }
            }
            await using (NpgsqlCommand command1 = new NpgsqlCommand("DELETE FROM curriculum WHERE userid = @userid", connection, transaction))
            {
                command1.Parameters.AddWithValue("userid", UserId);

                int IsChanged = await command1.ExecuteNonQueryAsync();

                if (IsChanged == 0)
                {
                    throw new AppError(404, "برنامه درسی ای وجود ندارد");
                }
            }
            await using (NpgsqlCommand command2 = new NpgsqlCommand("DELETE FROM unique_curriculumn WHERE user_id = @userid", connection, transaction))
            {
                command2.Parameters.AddWithValue("userid", UserId);

                int IsChanged = await command2.ExecuteNonQueryAsync();

                if (IsChanged == 0)
                {
                    throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                }
            }
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<bool> EditUserCurriculumn(int UserId, CurriculumnEditDto CRE) 
    {
        List<string> Updates = new();

        if (!string.IsNullOrWhiteSpace(CRE.lession_name))
        {
            Updates.Add(
                "lesson_name = @lesson_name"
            );
        }
        if (CRE.day_of_class.HasValue)
        {
            Updates.Add(
                "day_of_class = @day_of_class" 
            );   
        }
        if (CRE.start_class.HasValue)
        {
            Updates.Add(
                "start_class = @start_class"
            );
        }
        if (CRE.class_loc.HasValue)
        {
            Updates.Add(
                "class_loc = @class_loc"
            );
        }

        if (Updates.Count == 0)
        {
            throw new AppError(400, "جیزی برای اپدیت ارسال نشده");
        }

        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using (NpgsqlCommand command1 = new NpgsqlCommand($"UPDATE curriculum SET {string.Join(", ", Updates)} WHERE userid = @userid AND id = @id", connection))
        {
            if (!string.IsNullOrWhiteSpace(CRE.lession_name))
            {
                command1.Parameters.AddWithValue("lesson_name", CRE.lession_name);
            }
            if (CRE.day_of_class.HasValue)
            {
                command1.Parameters.AddWithValue("day_of_class", CRE.day_of_class);
            }
            if (CRE.start_class.HasValue)
            {
                command1.Parameters.AddWithValue("start_class", CRE.start_class);
            }
            if (CRE.class_loc.HasValue)
            {
                command1.Parameters.AddWithValue("class_loc", CRE.class_loc);
            }
            command1.Parameters.AddWithValue("userid", UserId);
            command1.Parameters.AddWithValue("id", CRE.Id);

            int IsChanged = await command1.ExecuteNonQueryAsync();

            if (IsChanged == 0)
            {
                throw new AppError(404, "این درس وجود ندارد");
            }
        }
        return true;
    }

    public async Task<List<CurriculumnResultDto>> GetUserCurriculumn(int UserId)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        List<CurriculumnResultDto> CRR = new();

        await using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT id, lesson_name, day_of_class, start_class, class_loc FROM curriculum WHERE userid = @userid ORDER BY day_of_class, start_class", connection))
        {
            command1.Parameters.AddWithValue("userid", UserId);

            await using (NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                while (await reader1.ReadAsync())
                {
                    CRR.Add(new CurriculumnResultDto
                    {
                        Id = reader1.GetInt64(0),
                        lession_name = reader1.GetString(1),
                        day_of_class = reader1.GetInt16(2),
                        start_class = reader1.GetFieldValue<TimeOnly>(3),
                        finish_class = reader1.GetFieldValue<TimeOnly>(3).AddHours(2),
                        class_loc = reader1.GetInt16(4)
                    });
                }
            }
        }
        return CRR;
    }
}











