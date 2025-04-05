using MongoDB.Bson.Serialization.Attributes;

namespace UsersApi.Entities;

public abstract class Mongeable
{
    [BsonId]
    public String Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public Mongeable()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Mongeable(string id)
    {
        Id = id;
    }
}