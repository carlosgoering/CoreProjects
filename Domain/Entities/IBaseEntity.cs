namespace Domain.Entities;

public interface IBaseEntity
{
    public string Id { get; set; }
    public string ExternalId { get; set; }
}

