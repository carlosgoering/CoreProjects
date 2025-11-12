namespace Domain.Entities.Configuration;

public interface IDatabase
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ConnectionKey { get; set; }

}
