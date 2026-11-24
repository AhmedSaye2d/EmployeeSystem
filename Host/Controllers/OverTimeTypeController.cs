using Application.Dtos.Request.OverTimeType;
using Application.Helper;
using Application.Service.OverTimeType.Commands;
using Application.Service.OverTimeType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class OverTimeTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OverTimeTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOverTimeTypesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل نوع عمل إضافي محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetOverTimeTypeByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Overtime type not found" });

            return Ok(result);
        }

       
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddOverTimeTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateOverTimeTypeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

       
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOverTimeTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateOverTimeTypeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOverTimeTypeCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
