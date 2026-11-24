using Application.Dtos.Request.Doctor;
using Application.Helper;
using Application.Service.Doctor.Commands;
using Application.Service.Doctor.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع سجلات الفحص الطبي مع تفاصيل الموظف
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetDoctorsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل سجل فحص طبي محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDoctorByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Doctor record not found" });

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع سجلات الفحص الطبي لموظف محدد برقم الموظف
        /// </summary>
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var result = await _mediator.Send(new GetDoctorsByEmployeeIdQuery(employeeId));
            return Ok(result);
        }

        /// <summary>
        /// إضافة سجل فحص طبي جديد لموظف
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddDoctorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateDoctorCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل سجل فحص طبي مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDoctorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateDoctorCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف سجل فحص طبي برقمه
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDoctorCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
