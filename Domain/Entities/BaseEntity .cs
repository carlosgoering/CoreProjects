namespace Domain.Entities;

public class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string externalIdentity { get; set; } = Guid.NewGuid().ToString();
}

