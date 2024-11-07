namespace Domain.Entities;

public interface IBaseEntity
{
    public string id { get; set; }
    public string externalIdentity { get; set; }
}

