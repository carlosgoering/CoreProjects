namespace Domain.Entities.Configuration;

public class Database
{
    public const string Section = "MongoDBDatabase";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
