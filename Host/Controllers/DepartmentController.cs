using Application.Dtos.Request.Department;
using Application.Helper;
using Application.Service.Department.Commands;
using Application.Service.Department.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الأقسام مع تفاصيل الإدارة العامة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetDepartmentsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل قسم محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDepartmentByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Department not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة قسم جديد
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddDepartmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateDepartmentCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل قسم مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateDepartmentCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف قسم برقمه
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDepartmentCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
