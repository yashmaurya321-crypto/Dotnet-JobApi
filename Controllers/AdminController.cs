using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("admin/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JobService _jobService;

        public AdminController(UserService userService, JobService jobService)
        {
            _userService = userService;
            _jobService = jobService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("post-job")]
        public async Task<IActionResult> PostJob([FromBody] Job job)
        {
            if (job == null || string.IsNullOrWhiteSpace(job.Title))
                return BadRequest("Job title is required.");

            var result = await _jobService.CreateJob(job);

            if (result.Contains("successfully"))
                return Ok(result);

            return BadRequest(result);
        }
    }
}
