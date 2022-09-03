using MongoDB.Entities;

namespace WidepollAPI.Models;

public abstract class DomainEntity : Entity, ICreatedOn
{
    public DateTime CreatedOn { get; set; }

    public abstract string ToString();
}