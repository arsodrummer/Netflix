using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Api.Mapper;
using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Models.Queries;
using NetflixServer.Models.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionPlanController : ControllerBase
    {
        private ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewSubscriptionPlanRequest newSubscriptionPlanRequest, CancellationToken cancellationToken)
        {
            var subscriptionPlan = newSubscriptionPlanRequest.ToSubscriptionPlan();
            await _subscriptionPlanService.CreateSubscriptionPlanAsync(subscriptionPlan, cancellationToken);
            return NoContent();
        }

        //[HttpGet]
        //[Route("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetByIdAsync([FromQuery] GetSubscriberQuery getSubscriberQuery, CancellationToken cancellationToken)
        //{
        //    return Ok(new SubscriberByIdResponse());
        //}

        //[HttpPatch]
        //[Route("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<IActionResult> PatchByIdAsync([FromBody] UpdateSubscriberRequest updateSubscriberRequest, CancellationToken cancellationToken)
        //{
        //    return NoContent();
        //}
    }
}
