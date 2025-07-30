using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class ProfileService
    {
        private readonly IMongoCollection<UserProfile> _profiles;
        private readonly IMongoCollection<User> _users;

        public ProfileService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UserCollectionName);
            _profiles = database.GetCollection<UserProfile>(settings.Value.ProfileCollection);
        }

        public async Task<List<UserProfile>> GetAllAsync()
        {
            try
            {
                return await _profiles.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all profiles: {ex.Message}");
                return new List<UserProfile>();
            }
        }

        public async Task<UserProfile?> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _profiles.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching profile for userId {userId}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsUserRegisteredAsync(string userId)
        {
            try
            {
                var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking user registration for userId {userId}: {ex.Message}");
                return false;
            }
        }

        public async Task<string> CreateOrUpdateAsync(UserProfile profile)
        {
            try
            {
                if (!await IsUserRegisteredAsync(profile.UserId))
                {
                    return "User is not registered. Profile cannot be created.";
                }

                var existing = await GetByUserIdAsync(profile.UserId);
                if (existing == null)
                    await _profiles.InsertOneAsync(profile);
                else
                    await _profiles.ReplaceOneAsync(p => p.UserId == profile.UserId, profile);

                return "Profile saved successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating or updating profile for userId {profile.UserId}: {ex.Message}");
                return "An error occurred while saving the profile.";
            }
        }
    }
}
