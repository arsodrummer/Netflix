using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Api.Mapper;
using NetflixServer.Api.Models.Requests;
using NetflixServer.Business.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
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
    }
}
