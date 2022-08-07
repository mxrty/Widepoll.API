using MongoDB.Entities;

namespace WidepollAPI.Models;

public class DomainEntity : Entity, ICreatedOn
{
    public DateTime CreatedOn { get; set; }
}

