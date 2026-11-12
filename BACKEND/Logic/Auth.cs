


using System.Security.Cryptography;
using System.Text;
using BZTEHUB.Backend.Dtos.AuthDtos;
using Npgsql;

public class Auth
{
    private readonly DataBaseService db;
    private readonly JwtService jwt;


    public Auth(DataBaseService db, JwtService jwt)
    {
        this.db = db;
        this.jwt = jwt;
    }


    public async Task<SignupResultDto> Signup(SignupDto NewUser)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();
        await connection.OpenAsync();

        await using(NpgsqlCommand command1 = new NpgsqlCommand("SELECT 1 FROM users WHERE email = @email LIMIT 1", connection))
        {
            command1.Parameters.AddWithValue("email", NewUser.Email);

            Object? UserExists = await command1.ExecuteScalarAsync();

            if (UserExists != null)
            {
                throw new AppError(409, "این ایمیل در حال حاضر متعلق به شخص دیگری است");
            }
        }
        string Email = string.Empty;
        DateTime dateTime = DateTime.UtcNow;
        string AccessToken = string.Empty;
        string RefreshToken = jwt.GenerateRefreshToken();
        await using(NpgsqlCommand command2 = new NpgsqlCommand("INSERT INTO users(email, password, refreshtoken, expires_refreshtoken) VALUES(@email, @password, @refreshtoken, @expires_refreshtoken) RETURNING id, email, created_at", connection))
        {
            command2.Parameters.AddWithValue("email", NewUser.Email);
            command2.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(NewUser.Password));
            command2.Parameters.AddWithValue("refreshtoken", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(RefreshToken))));
            command2.Parameters.AddWithValue("expires_refreshtoken", DateTimeOffset.UtcNow.AddDays(30));

            await using(NpgsqlDataReader reader1 = await command2.ExecuteReaderAsync())
            {
                if (!await reader1.ReadAsync())
                {
                    throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                }
                AccessToken = jwt.GenerateAccessToken(reader1.GetInt64(0));
                Email = reader1.GetString(1);
                dateTime = reader1.GetDateTime(2);
            }
        }

        return new SignupResultDto
        {
            Email = Email,
            AccessToken = AccessToken,
            RefreshToken = RefreshToken,
            dateTime = dateTime
        };
    }

    public async Task<LoginResultDto> Login(LoginDto User)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();
        await connection.OpenAsync();

        string Email = string.Empty;
        DateTime dateTime = DateTime.UtcNow;
        string AccessToken = string.Empty;
        string RefreshToken = string.Empty;
        await using(NpgsqlCommand command1 = new NpgsqlCommand("SELECT id, email, password, created_at FROM users WHERE email = @email", connection))
        {
            command1.Parameters.AddWithValue("email", User.Email);

            await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                if (!await reader1.ReadAsync())
                {
                    throw new AppError(401, "ایمیل یا رمز عبور اشتباه است");
                }
                if (!BCrypt.Net.BCrypt.Verify(User.Password, reader1.GetString(2)))
                {
                    throw new AppError(401, "ایمیل یا رمز عبور اشتباه است");
                }
                Email = reader1.GetString(1);
                dateTime = reader1.GetDateTime(3);
                AccessToken = jwt.GenerateAccessToken(reader1.GetInt64(0));
                RefreshToken = jwt.GenerateRefreshToken();
            }
        }
        await using(NpgsqlCommand command2 = new NpgsqlCommand("UPDATE users SET refreshtoken = @refreshtoken, expires_refreshtoken = @expires_refreshtoken WHERE email = @email", connection))
        {
            command2.Parameters.AddWithValue("refreshtoken", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(RefreshToken))));
            command2.Parameters.AddWithValue("expires_refreshtoken", DateTimeOffset.UtcNow.AddDays(30));
            command2.Parameters.AddWithValue("email", User.Email);

            int IsUpdated = await command2.ExecuteNonQueryAsync();

            if (IsUpdated == 0)
            {
                throw new AppError(500, "مشکلی پیش امد دوباره امتحانکنید");
            }
        }
        return new LoginResultDto
        {
            Email = Email,
            dateTime = dateTime,
            AccessToken = AccessToken,
            RefreshToken = RefreshToken
        };
    }

    public async Task<ProfileResultDto> Profile(long UserId)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        string Email = string.Empty;
        DateTime dateTime = DateTime.UtcNow;

        await using(NpgsqlCommand command1 = new NpgsqlCommand("SELECT email, created_at FROM users WHERE id = @userid", connection))
        {
            command1.Parameters.AddWithValue("userid", UserId);

            await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                if (!await reader1.ReadAsync())
                {
                    throw new AppError(404, "ابتدا ورود کنید");
                }
                Email = reader1.GetString(0);
                dateTime = reader1.GetDateTime(1);
            }
        }
        return new ProfileResultDto
        {
            Email = Email,
            dateTime = dateTime
        };
    }
    public async Task<EditEmailResultDto> EditEmail(long UserId, EditEmailDto UserEmail) 
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT email FROM users WHERE id = @userid", connection))
        {
            command1.Parameters.AddWithValue("userid", UserId);

            Object? UsersEmail = await command1.ExecuteScalarAsync();
            if (UsersEmail is null || UsersEmail == DBNull.Value)
            {
                throw new AppError(401, "کاربری با این ایمیل موجودیت ندارد");
            }
            if (UsersEmail.ToString() == UserEmail.Email)
            {
                return new EditEmailResultDto
                {
                    Email = UsersEmail.ToString()!
                };
            }
        }

        await using(NpgsqlCommand command2 = new NpgsqlCommand("SELECT 1 FROM users WHERE email = @email AND id != @userid", connection))
        {
            command2.Parameters.AddWithValue("email", UserEmail.Email);
            command2.Parameters.AddWithValue("userid", UserId);

            Object? IsUserFound = await command2.ExecuteScalarAsync();

            if (IsUserFound != null)
            {
                throw new AppError(409, "این ایمیل متعلق به کاربر دیگری است");
            }
        }

        string Email = string.Empty;

        await using(NpgsqlCommand command3 = new NpgsqlCommand("UPDATE users SET email = @email WHERE id = @userid RETURNING email", connection))
        {
            command3.Parameters.AddWithValue("email", UserEmail.Email);
            command3.Parameters.AddWithValue("userid", UserId);

            Object? IsChanged = await command3.ExecuteScalarAsync();

            if (IsChanged is null)
            {
                throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
            }
            Email = IsChanged.ToString()!;
        }
        return new EditEmailResultDto
        {
            Email = Email
        };
    }
    public async Task<string> EditPassword(long UserId, EditPasswordDto EditPass) 
    {
        if (EditPass.NewPassword != EditPass.ConfirmPassword)
        {
            throw new AppError(400, "New password is not match with confirm password");
        }

        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            await using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT id, password FROM users WHERE id = @userid", connection, transaction))
            {
                command1.Parameters.AddWithValue("userid", UserId);

                await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
                {
                    if (!await reader1.ReadAsync())
                    {
                        throw new AppError(404, "کاربر پیدا نشد. ابتدا ورود کنید");
                    }
                    if (!BCrypt.Net.BCrypt.Verify(EditPass.OldPassword, reader1.GetString(1)))
                    {
                        throw new AppError(401, "رمز عبور قبلی نا درست است");
                    }
                }
            }
            string RefreshToken = jwt.GenerateRefreshToken();
            await using (NpgsqlCommand command2 = new NpgsqlCommand("UPDATE users SET password = @password, refreshtoken = @refreshtoken, expires_refreshtoken = @expires_refreshtoken WHERE id = @userid", connection, transaction))
            {
                command2.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(EditPass.NewPassword));
                command2.Parameters.AddWithValue("refreshtoken", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(RefreshToken))));
                command2.Parameters.AddWithValue("expires_refreshtoken", DateTimeOffset.UtcNow.AddDays(30));
                command2.Parameters.AddWithValue("userid", UserId);

                
                int IsChanged = await command2.ExecuteNonQueryAsync();

                if (IsChanged == 0)
                {
                    throw new AppError(500, "مشکلی پیش امد. دوباره امتحان کنید");
                }
            }
            await transaction.CommitAsync();
            return RefreshToken;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<bool> Logout(string RefreshToken)
    {
        if (string.IsNullOrWhiteSpace(RefreshToken))
        {
            return false;
        }

        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        await using(NpgsqlCommand command1 = new NpgsqlCommand("UPDATE users SET refreshtoken = @refreshtoken, expires_refreshtoken = @expires_refreshtoken WHERE refreshtoken = @refreshtoken2", connection))
        {
            command1.Parameters.AddWithValue("refreshtoken", "");
            command1.Parameters.AddWithValue("expires_refreshtoken", DateTimeOffset.UtcNow);
            command1.Parameters.AddWithValue("refreshtoken2", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(RefreshToken))));

            await command1.ExecuteNonQueryAsync();

        }
        return true;
    }
    public async Task<RefreshTokenDto> RefreshToken(string RefreshToken)
    {
        await using NpgsqlConnection connection = db.CreateDataBaseConnection();

        await connection.OpenAsync();

        long UserId;
        string NewRefreshToken = jwt.GenerateRefreshToken();

        await using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT id, expires_refreshtoken FROM users WHERE refreshtoken = @refreshtoken", connection))
        {
            command1.Parameters.AddWithValue("refreshtoken", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(RefreshToken))));

            await using(NpgsqlDataReader reader1 = await command1.ExecuteReaderAsync())
            {
                if (!await reader1.ReadAsync())
                {
                    throw new AppError(401, "ابتدا ورود کنید");
                }
                if (DateTimeOffset.UtcNow > reader1.GetDateTime(1))
                {
                    throw new AppError(401, "ابتدا ورود کند");
                }
                UserId = reader1.GetInt64(0);
            }
        }

        string AccessToken = string.Empty;

        await using (NpgsqlCommand command2 = new NpgsqlCommand("UPDATE users SET refreshtoken = @refreshtoken, expires_refreshtoken = @expires_refreshtoken WHERE id = @userid", connection))
        {
            command2.Parameters.AddWithValue("refreshtoken", Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(NewRefreshToken))));
            command2.Parameters.AddWithValue("expires_refreshtoken", DateTimeOffset.UtcNow.AddDays(30));
            command2.Parameters.AddWithValue("userid", UserId);

            int IsChanged = await command2.ExecuteNonQueryAsync();

            if (IsChanged == 0)
            {
                throw new AppError(500, "مشکلی پیش امد. لطفا دوباره امتحان کنید");
            }
            AccessToken = jwt.GenerateAccessToken(UserId);
        }
        return new RefreshTokenDto
        {
            AccessToken = AccessToken,
            RefreshToken = NewRefreshToken
        };
    }
}











