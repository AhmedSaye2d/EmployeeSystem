using Application.Dtos.Request.GeneralDepartment;
using Application.Helper;
using Application.Service.GeneralDepartment.Commands;
using Application.Service.GeneralDepartment.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class GeneralDepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeneralDepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الإدارات العامة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetGeneralDepartmentsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل إدارة عامة محددة برقمها
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetGeneralDepartmentByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "General Department not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة إدارة عامة جديدة
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddGeneralDepartmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateGeneralDepartmentCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل إدارة عامة مسجلة
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateGeneralDepartmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateGeneralDepartmentCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف إدارة عامة برقمها
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGeneralDepartmentCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
