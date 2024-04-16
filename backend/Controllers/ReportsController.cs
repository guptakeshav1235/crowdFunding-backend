using FeatureObject.Abstraction.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/all-reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IBackerService _backerService;

        public ReportsController(IBackerService backerService)
        {
            _backerService = backerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetReportsForAllUsers()
        {
            var campaigns = await _backerService.GetReportsForAllUsers();
            return Ok(campaigns);
        }
    }
}
