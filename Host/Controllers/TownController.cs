using Application.Dtos.Request.Town;
using Application.Helper;
using Application.Service.Town.Commands;
using Application.Service.Town.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class TownController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TownController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع البلدات مع تفاصيل المدينة والدولة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetTownsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل بلدة محددة برقمها
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetTownByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Town not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة بلدة جديدة
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddTownRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateTownCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل بلدة مسجلة
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTownRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTownCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف بلدة برقمها
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTownCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
