using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Api.Mapper;
using NetflixServer.Api.Models.Queries;
using NetflixServer.Api.Models.Requests;
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetSubscriberQuery getSubscriberQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionPlanService.GetSubscriptionPlanByIdAsync(getSubscriberQuery.Id, cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchByIdAsync([FromBody] UpdateSubscriptionPlanByIdRequest updateSubscriptionPlanByIdRequest, [FromRoute] UpdateSubscriptionPlanByIdQuery updateSubscriptionPlanByIdQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionPlanService.UpdateSubscriptionPlanById(updateSubscriptionPlanByIdQuery.Id,
                updateSubscriptionPlanByIdRequest.Price,
                updateSubscriptionPlanByIdRequest.ExpirationDate,
                cancellationToken);

            if (response == null)
                return BadRequest();

            return NoContent();
        }
    }
}
