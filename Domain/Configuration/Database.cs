namespace Domain.Entities.Configuration;

public class Database
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string ConnectionKey { get; set; } = null!;

}
