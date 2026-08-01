
namespace Domain.Interfaces;
public interface IEntityMapper<TDomain, TPersisted>
{
    TPersisted ToPersisted(TDomain domain);
    TDomain ToDomain(TPersisted persisted);
}