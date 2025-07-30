using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace backend.Models
{
    public class UserProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public List<string> Skills { get; set; } = new();

        public List<Qualification> Qualifications { get; set; } = new();
        public List<Experience> Experience { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
    }

    public class Qualification
    {
        public string Degree { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string YearOfPassing { get; set; } = string.Empty;
        public string GradeOrPercentage { get; set; } = string.Empty;
    }

    public class Experience
    {
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty; // e.g., "Jan 2022 - Mar 2023"
        public string Description { get; set; } = string.Empty;
    }

    public class Project
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public List<string> TechStack { get; set; } = new();
    }
}
