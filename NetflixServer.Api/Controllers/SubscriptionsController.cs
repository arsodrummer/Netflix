using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Api.Mapper;
using NetflixServer.Api.Models.Queries;
using NetflixServer.Api.Models.Requests;
using NetflixServer.Business.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewSubscriptionRequest newUserRequest, CancellationToken cancellationToken)
        {
            var subscription = newUserRequest.ToSubscription();
            await _subscriptionService.CreateSubscriptionAsync(subscription.UserId, subscription.PlanId, subscription.ExpirationDate, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.GetSubscriptionListAsync(cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteSubscriptionByIdQuery deleteSubscriptionByIdQuery, CancellationToken cancellationToken)
        {
            await _subscriptionService.DeleteSubscriptionByIdAsync(deleteSubscriptionByIdQuery.Id, cancellationToken);
            return NoContent();
        }
    }
}
