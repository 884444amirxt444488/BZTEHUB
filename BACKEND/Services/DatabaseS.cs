


using Npgsql;

public class DataBaseService
{
    private readonly string connection;

    public DataBaseService(IConfiguration configuration)
    {
        connection = configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("DefaultConnectionString not found");
    }

    public NpgsqlConnection CreateDataBaseConnection()
    {
        return new NpgsqlConnection(connection);
    }
}











