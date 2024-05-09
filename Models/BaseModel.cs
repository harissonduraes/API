using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
