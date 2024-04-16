using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Models;
using FeatureObject.Abstraction.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/campaign")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCampaigns()
        {
            var campaigns = await _campaignService.GetAllCampaigns();
            return Ok(campaigns);
        }

        [HttpGet("{id}")]
        public IActionResult GetCampaignById(int id)
        {
            var campaign = _campaignService.GetCampaignById(id);

            if (campaign == null)
            {
                return NotFound();
            }

            return Ok(campaign);
        }

        [HttpPost]
        public IActionResult AddCampaign([FromBody] Campaign campaign)
        {
            if (campaign == null)
            {
                return BadRequest();
            }
            
            _campaignService.AddCampaign(campaign);

            // Update current amount and backer count
            //var updatedCampaign = _campaignService.GetCampaignById(campaign.Id);
            //updatedCampaign.CurrentAmount += campaign.CurrentAmount; // Assuming contribution amount is stored in CurrentAmount
            //updatedCampaign.Backers++; // Increment backer count

            // Save the updated campaign data
           // _campaignService.UpdateCampaign(updatedCampaign);

            return CreatedAtAction(nameof(GetCampaignById), new { id = campaign.Id }, campaign);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(int id, [FromBody] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return BadRequest();
            }

            var existingCampaign = _campaignService.GetCampaignById(id);
            if (existingCampaign == null)
            {
                return NotFound();
            }

            await _campaignService.UpdateCampaign(campaign);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            try
            {
                await _campaignService.DeleteCampaign(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
