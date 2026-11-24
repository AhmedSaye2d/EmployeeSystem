using Application.Dtos.Request.Payroll;
using Application.Helper;
using Application.Service.Payroll.Commands;
using Application.Service.Payroll.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class PayrollController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع سجلات مسيرات الرواتب في النظام
        /// </summary>
        [HttpGet("all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetPayrollsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل مسير محدد برقم المعرف
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPayrollByIdQuery(id));
            if (result is null)
                return NotFound(new { message = $"Payroll with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع مسيرات الرواتب التاريخية لموظف محدد برقم الموظف
        /// </summary>
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var result = await _mediator.Send(new GetPayrollsByEmployeeIdQuery(employeeId));
            return Ok(result);
        }

        /// <summary>
        /// جلب جميع مسيرات الرواتب لشهر وسنة محددين
        /// </summary>
        [HttpGet("period")]
        public async Task<IActionResult> GetByPeriod([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest(new { message = "Month must be between 1 and 12." });

            if (year < 2000 || year > 2100)
                return BadRequest(new { message = "Year must be a valid year." });

            var result = await _mediator.Send(new GetPayrollsByPeriodQuery(month, year));
            return Ok(result);
        }

        /// <summary>
        /// إنشاء مسير راتب لموظف محدد لشهر وسنة معينة (يُنشأ كمسودة Draft)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePayrollRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreatePayrollCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// توليد المسير الشهري مجمعاً لجميع الموظفين (أو قائمة محددة) دفعة واحدة
        /// </summary>
        [HttpPost("generate-monthly")]
        public async Task<IActionResult> GenerateMonthly([FromBody] GenerateMonthlyPayrollRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GenerateMonthlyPayrollCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل بدلات وملاحظات مسودة المسير قبل الاعتماد (متاح فقط لمسيرات Draft)
        /// </summary>
        [HttpPut("draft")]
        public async Task<IActionResult> UpdateDraft([FromBody] UpdatePayrollDraftRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdatePayrollDraftCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// اعتماد مسير الراتب (تغيير الحالة إلى Approved وإقفاله نهائياً لحفظ السجلات التاريخية)
        /// </summary>
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _mediator.Send(new ApprovePayrollCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// صرف مسير الراتب (تغيير الحالة إلى Paid وتوثيق تاريخ الصرف الفعلي PaymentDate)
        /// </summary>
        [HttpPut("{id:int}/pay")]
        public async Task<IActionResult> Pay(int id, [FromQuery] DateTime? paymentDate = null)
        {
            var result = await _mediator.Send(new PayPayrollCommand(id, paymentDate));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف مسودة مسير (متاح فقط لمسيرات Draft ويُمنع للمسيرات المعتمدة أو المصروفة)
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDraft(int id)
        {
            var result = await _mediator.Send(new DeletePayrollDraftCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
