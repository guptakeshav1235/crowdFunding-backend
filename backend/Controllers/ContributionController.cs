using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Models;
using DataStore.Implementation.Repositories;
using FeatureObject.Abstraction.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/contribution")]
    [ApiController]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _contributionService;
        private readonly ICampaignService _campaignService;
        private readonly IUserService _userService;
        public ContributionController(IContributionService contributionService, ICampaignService campaignService, IUserService userService)
        {
            _contributionService = contributionService;
            _campaignService = campaignService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IContributionDetails>>> GetContributionDetails()
        {
            try
            {
                var contributions = await _contributionService.GetContributionsDetails();
                return Ok(contributions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving contribution details.");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<IContributionDetails>>> GetContributionDetailsByUserId(int userId)
        {
            try
            {
                var contributions = await _contributionService.GetContributionsByUserId(userId);
                return Ok(contributions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving contribution details for the user.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddContribution([FromBody] ContributionRequest request)
        {
            try
            {
                // Fetch the campaign to ensure it exists
                var campaign =  _campaignService.GetCampaignById(request.CampaignId);
                if (campaign == null)
                {
                    return BadRequest("Invalid campaign ID.");
                }

                // Fetch the user to ensure it exists
                var user = _userService.GetUserById(request.UserId);
                if(user==null)
                {
                    return BadRequest("Invalid user ID.");
                };

                var contribution = new ContributionDetails
                {
                    CampaignId = request.CampaignId,
                    UserId = request.UserId,
                    Amount = request.Amount,
                    Date = DateTime.UtcNow
                };

                await _contributionService.AddContribution(contribution);

                // Update current amount and backers count in the campaign
                campaign.CurrentAmount += request.Amount;
                campaign.Backers++;

                await _campaignService.UpdateCampaign(campaign);

                return Ok("Contribution added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the contribution.");
            }
        }
    }
    public class ContributionRequest
    {
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        // Other properties as needed
    }
}
