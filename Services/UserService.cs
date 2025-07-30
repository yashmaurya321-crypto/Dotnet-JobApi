using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        private readonly IMongoCollection<Job> _jobs;
        public UserService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UserCollectionName);
            _jobs = database.GetCollection<Job>(settings.Value.JobCollectionName);
        }

        public async Task<string> RegisterUser(User newUser)
        {
            try
            {
                var existingUser = await _users.Find(u => u.Email == newUser.Email).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    return "User already exists";
                }

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                await _users.InsertOneAsync(newUser);

                return "User registered successfully";
            }
            catch (Exception ex)
            {
                return $"Error registering user: {ex.Message}";
            }
        }

        public async Task<string> LoginUser(string email, string password)
        {
            try
            {
                var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return "User not found";
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!isPasswordValid)
                {
                    return "Invalid password";
                }

                return $"Login successful. Welcome, {user.Name}!";
            }
            catch (Exception ex)
            {
                return $"Error during login: {ex.Message}";
            }
        }

        public async Task<bool> IsUser(string userId)
        {
            try
            {
                var result = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
                return result != null; // true if user exists
            }
            catch
            {
                return false;
            }
        }

        public async Task<object?> GetUserById(string id)
        {
            try
            {
                var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
                if (user == null) return null;

                user.AppliedJobs ??= new List<string>();

                var jobs = await _jobs.Find(j => user.AppliedJobs.Contains(j.Id)).ToListAsync();

                return new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    isAdmin = user.isAdmin,
                    AppliedJobs = jobs
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _users.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<User>(); 
            }
        }
    }
}
