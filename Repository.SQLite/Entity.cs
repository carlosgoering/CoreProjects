using Domain.Entities;

namespace Repository.SQLite;

public class Entity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public new int Id { get; set; }
}