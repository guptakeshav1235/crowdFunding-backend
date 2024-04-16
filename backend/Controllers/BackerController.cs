using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Models;
using DataStore.Implementation.Repositories;
using FeatureObject.Abstraction.FeatureObject;
using FeatureObject.Implementation.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class BackerController : ControllerBase
    {
        private readonly IBackerRepository _backerRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUserRepository _userRepository;

        public BackerController(IBackerRepository backerRepository, ICampaignRepository campaignRepository, IUserRepository userRepository)
        {
            _backerRepository = backerRepository;
            _campaignRepository = campaignRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReports(string username, string email, string password)
        {
            try
            {
                // Validate email and password
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return BadRequest("Username, Email, and password are required.");
                }

                // Authenticate user
                IUser user = await _userRepository.GetUser(username, email, password);
                if (user == null)
                {
                    return Unauthorized("Invalid username, email, or password.");
                }

                // Get campaign title
                IActionResult campaignResponse = await GetCampaignTitle();
                if (!(campaignResponse is OkObjectResult campaignResult))
                {
                    return campaignResponse; // Forward error response from campaign controller
                }

                string campaignTitle = campaignResult.Value?.ToString() ?? "Default Campaign";

                // Get backer reports using campaign title and authenticated user
                var backers = await _backerRepository.GetReports(username, email, password);
                if (backers == null || !backers.Any())
                {
                    return NotFound("No contributions found for the user.");
                }

                return Ok(backers);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetReports: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching contribution reports.");
            }
        }

        private async Task<IActionResult> GetCampaignTitle()
        {
            // Call CampaignRepository to get all campaigns
            IEnumerable<ICampaign> campaigns = await _campaignRepository.GetAllCampaigns();

            // For simplicity, return the title of the first campaign
            string campaignTitle = campaigns.FirstOrDefault()?.Title ?? "Default Campaign Title";
            return Ok(campaignTitle);
        }
    }
}
