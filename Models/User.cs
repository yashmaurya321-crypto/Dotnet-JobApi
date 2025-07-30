using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {  get; set; }
        public string Name { get; set; } = null;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<string>? AppliedJobs { get; set; }
        public bool? isAdmin { get; set; } = false; 
    }
}
