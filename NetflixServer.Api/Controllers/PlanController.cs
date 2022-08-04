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
    public class PlanController : ControllerBase
    {
        private IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] NewPlanRequest newPlanRequest, CancellationToken cancellationToken)
        {
            var plan = newPlanRequest.ToPlan();
            await _planService.CreatePlanAsync(plan, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetUserQuery getUserQuery, CancellationToken cancellationToken)
        {
            var response = await _planService.GetPlanByIdAsync(getUserQuery.Id, cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            var response = await _planService.GetPlanListAsync(cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchByIdAsync([FromBody] UpdatePlanByIdRequest updatePlanByIdRequest, [FromRoute] UpdatePlanByIdQuery updatePlanByIdQuery, CancellationToken cancellationToken)
        {
            var response = await _planService.UpdatePlanById(updatePlanByIdQuery.Id,
                updatePlanByIdRequest.Price,
                cancellationToken);

            if (response == null)
                return BadRequest();

            return NoContent();
        }
    }
}
