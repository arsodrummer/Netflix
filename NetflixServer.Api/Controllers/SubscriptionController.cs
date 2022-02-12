using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Models.Queries;
using NetflixServer.Models.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        public SubscriptionController()
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewSubscriberRequest newSubscriberRequest, CancellationToken cancellationToken)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetSubscriberQuery getSubscriberQuery, CancellationToken cancellationToken)
        {
            return Ok(new SubscriberByIdResponse());
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchByIdAsync([FromBody] UpdateSubscriberRequest updateSubscriberRequest, CancellationToken cancellationToken)
        {
            return NoContent();
        }
    }
}
