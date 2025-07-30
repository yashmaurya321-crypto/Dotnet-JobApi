using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public int Salary { get; set; } = 0!;

        public int Experience { get; set; } = 0!;

        public string JobType { get; set; } = null!;

        public List<string> Skills { get; set; } = new List<string>();

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;


        public List<string>? AppliedUsers { get; set; } = new List<string>();
    }
}
