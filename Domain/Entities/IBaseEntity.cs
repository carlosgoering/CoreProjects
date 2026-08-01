namespace Domain.Entities;

public interface IBaseEntity
{
    public string Id { get; set; }
    public string ExternalId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}

