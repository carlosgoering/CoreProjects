using DataAccess.Abstractions.Attributes;

namespace DataAccess.Abstractions.Models;

public interface IBaseEntity
{
    [PrimaryKey]
    public string Id { get; set; }
    public string ExternalId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}

