using Application.Dtos.Request.OverTime;
using Application.Helper;
using Application.Service.OverTime.Commands;
using Application.Service.OverTime.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class OverTimeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OverTimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع سجلات العمل الإضافي مع تفاصيل الموظف ونوع العمل الإضافي
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOverTimesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل سجل عمل إضافي محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetOverTimeByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Overtime record not found" });

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع سجلات العمل الإضافي لموظف محدد برقم الموظف
        /// </summary>
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var result = await _mediator.Send(new GetOverTimesByEmployeeIdQuery(employeeId));
            return Ok(result);
        }

        /// <summary>
        /// إضافة سجل عمل إضافي جديد لموظف
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddOverTimeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateOverTimeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل سجل عمل إضافي مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOverTimeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateOverTimeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف سجل عمل إضافي برقمه
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOverTimeCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
