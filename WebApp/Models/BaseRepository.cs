namespace WebApp.Models;
public abstract class BaseRepository
{
    protected string connectionString;
    public BaseRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Shop") ?? throw new Exception("Not Found Shop Data");
    }
}