using Application.Dtos.Request.Sanction;
using Application.Helper;
using Application.Service.Sanction.Commands;
using Application.Service.Sanction.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class SanctionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SanctionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الجزاءات مع تفاصيل الموظف ونوع الجزاء
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetSanctionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل جزاء محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSanctionByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Sanction not found" });

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع جزاءات موظف محدد برقم الموظف
        /// </summary>
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var result = await _mediator.Send(new GetSanctionsByEmployeeIdQuery(employeeId));
            return Ok(result);
        }

        /// <summary>
        /// إضافة جزاء جديد لموظف
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddSanctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateSanctionCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل جزاء مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSanctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSanctionCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف جزاء برقمه
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSanctionCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
