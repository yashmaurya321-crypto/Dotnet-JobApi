using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var profile = await _profileService.GetByUserIdAsync(userId);
            if (profile == null)
                return NotFound("Profile not found for this user.");

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateProfile([FromBody] UserProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.UserId))
                return BadRequest("UserId is required.");

            bool isRegistered = await _profileService.IsUserRegisteredAsync(profile.UserId);
            if (!isRegistered)
                return BadRequest("User is not registered. Cannot create profile.");

            var resultMessage = await _profileService.CreateOrUpdateAsync(profile);
            return Ok(resultMessage);
        }
    }
}
