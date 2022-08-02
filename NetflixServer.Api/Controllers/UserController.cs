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
    public class UserController : ControllerBase
    {
        private IUserService _subscriptionService;

        public UserController(IUserService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            var user = newUserRequest.ToUser();
            await _subscriptionService.CreateUserAsync(user, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetUserQuery getUserQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.GetUserByIdAsync(getUserQuery.Id, cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.GetUserListAsync(cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchByIdAsync([FromBody] UpdateUserRequest updateUserRequest, [FromRoute] UpdateUserByIdQuery updateUserByIdQuery, CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.UpdateUserByIdAsync(updateUserByIdQuery.Id, updateUserRequest.SubscriptionPlanId, updateUserRequest.ExpirationDate, updateUserRequest.Active, cancellationToken);

            if (response == null)
                return BadRequest();

            return NoContent();
        }
    }
}
