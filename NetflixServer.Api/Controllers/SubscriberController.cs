using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Api.Mapper;
using NetflixServer.Api.Models.Queries;
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
    public class SubscriberController : ControllerBase
    {
        private ISubscriberService _subscriptionService;

        public SubscriberController(ISubscriberService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewSubscriberRequest newSubscriberRequest, CancellationToken cancellationToken)
        {
            var subscriber = newSubscriberRequest.ToSubscriber();
            await _subscriptionService.CreateSubscriberAsync(subscriber, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetSubscriberQuery getSubscriberQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.GetSubscriberByIdAsync(getSubscriberQuery.Id, cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.GetSubscriberListAsync(cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchByIdAsync([FromBody] UpdateSubscriberRequest updateSubscriberRequest, [FromRoute] UpdateSubscriberByIdQuery updateSubscriberByIdQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.UpdateSubscriberByIdAsync(updateSubscriberByIdQuery.Id, updateSubscriberRequest.SubscriptionPlanId, updateSubscriberRequest.ExpirationDate, updateSubscriberRequest.Active, cancellationToken);

            if (response == null)
                return BadRequest();

            return NoContent();
        }
    }
}
