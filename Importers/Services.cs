namespace BZTEHUB.Importers.DBConnection;


using Npgsql;
public class DataBaseService
{
    public NpgsqlConnection CreateDb()
    {
        return new NpgsqlConnection("Host=localhost;Port=5432;Database=BZTEHUB;Username=postgres;Password=*Amirfatt1385");
    }
}