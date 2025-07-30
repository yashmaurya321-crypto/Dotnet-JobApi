using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("job/[controller]")]
    public class JobController : Controller
    {
        private readonly JobService _jobservice;

        private readonly UserService _userService;
        public JobController(JobService jobservice , UserService userService)
        {
            _jobservice = jobservice;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(string id)
        {
            var jobWithUsers = await _jobservice.GetJobById(id);
            if (jobWithUsers == null)
                return NotFound(new { message = "Job not found" });

            return Ok(jobWithUsers);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToJob([FromBody] JobApplicationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.JobId) || string.IsNullOrWhiteSpace(request.UserId))
            {
                return BadRequest("JobId and UserId are required.");
            }

            var isUser = await _userService.IsUser(request.UserId);
            if (!isUser)
            {
                return NotFound("User not found.");
            }

            var result = await _jobservice.ApplyToJob(request.JobId, request.UserId);

            if (result.Contains("successfully"))
            {
                return Ok(result); 
            }

            return BadRequest(result); 

        }


        public class JobApplicationRequest
        {
            public string JobId { get; set; }
            public string UserId { get; set; }
        }
    }
}
