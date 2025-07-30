using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class JobService
    {
        private readonly IMongoCollection<Job> _jobs;
        private readonly IMongoCollection<User> _users;

        public JobService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UserCollectionName);
            _jobs = database.GetCollection<Job>(settings.Value.JobCollectionName);
        }

        public async Task<string> ApplyToJob(string jobId, string userId)
        {
            try
            {
                var job = await _jobs.Find(j => j.Id == jobId).FirstOrDefaultAsync();
                if (job == null)
                    return "Job not found";

                var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                    return "User not found";

                job.AppliedUsers ??= new List<string>();
                user.AppliedJobs ??= new List<string>();

                if (job.AppliedUsers.Contains(userId) || user.AppliedJobs.Contains(jobId))
                    return "User has already applied for this job.";

                job.AppliedUsers.Add(userId);
                await _jobs.UpdateOneAsync(j => j.Id == jobId, Builders<Job>.Update.Set(j => j.AppliedUsers, job.AppliedUsers));

                user.AppliedJobs.Add(jobId);
                await _users.UpdateOneAsync(u => u.Id == userId, Builders<User>.Update.Set(u => u.AppliedJobs, user.AppliedJobs));

                return "Application submitted successfully.";

            }
            catch (Exception ex)
            {
                return $"Error occurred while applying to the job: {ex.Message}";
            }
        }

        public async Task<List<Job>> GetJobs()
        {
            try
            {
                return await _jobs.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Job>(); 
            }
        }

        public async Task<object?> GetJobById(string jobId)
        {
            try
            {
                var job = await _jobs.Find(j => j.Id == jobId).FirstOrDefaultAsync();
                if (job == null) return null;

                var users = await _users.Find(u => job.AppliedUsers.Contains(u.Id)).ToListAsync();

                return new
                {
                    JobId = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Skills = job.Skills,
                    Experience = job.Experience,
                    AppliedUsers = users.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Email
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> CreateJob(Job job)
        {
            try
            {
                await _jobs.InsertOneAsync(job);
                return "Job created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error occurred while creating the job: {ex.Message}";
            }
        }
    }
}
