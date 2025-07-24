using Application.Appointment.Commands;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Persistence.UnitOfWork;

namespace FLAWLESS.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly ILogger<AppointmentController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentController(ISender mediator, ILogger<AppointmentController> logger, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("create-appointment-payment")]
        public async Task<IActionResult> CreateAppointmentPayment([FromBody] BookingAppoinmentCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Customer, Artist, Admin")]
        [HttpPost("confirm-complete")]
        public async Task<IActionResult> ConfirmComplete([FromForm] ConfirmCompleteCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("customer-cancel")]
        public async Task<IActionResult> CustomerCancel([FromForm] CancelledBookingCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Artist")]
        [HttpPost("artist-cancel")]
        public async Task<IActionResult> ArtistCancel([FromForm] ArtistCancelledBookingCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("payos-webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> PayOsWebhook([FromBody] WebhookType request)
        {
            if (request == null || request.code != "00")
            {
                _logger.LogInformation("Webhook không hợp lệ: {@Webhook}", request);
                return Ok(new { success = false, message = "Dữ liệu webhook không hợp lệ" });
            }

            var orderCode = request.data?.orderCode.ToString();
            var amount = request.data?.amount ?? 0;

            var transaction = (await _unitOfWork.TransactionRepository
                .FindAsync(t => t.PaymentProviderTxnId == orderCode)).FirstOrDefault();

            if (transaction == null)
            {
                _logger.LogWarning("Không tìm thấy giao dịch: {OrderCode}", orderCode);
                return Ok(new { success = false, message = "Không tìm thấy giao dịch" });
            }

            if (amount < transaction.Amount)
            {
                _logger.LogWarning("Số tiền thanh toán không khớp: {OrderCode}", orderCode);
                return Ok(new { success = false, message = "Số tiền thanh toán không đúng" });
            }

            transaction.TransactionStatus = TransactionStatus.Completed;

            var appointment = (await _unitOfWork.AppointmentRepository
                .FindAsync(a => a.Id == transaction.AppointmentId)).FirstOrDefault();

            if (appointment == null)
            {
                _logger.LogWarning("Không tìm thấy cuộc hẹn cho giao dịch: {TransactionId}", transaction.Id);
                return Ok(new { success = false, message = "Không tìm thấy cuộc hẹn" });
            }

            if (appointment.VoucherId.HasValue)
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(appointment.VoucherId.Value);
                if (voucher != null)
                {
                    voucher.CurrentUsage = (voucher.CurrentUsage ?? 0) + 1;
                    _unitOfWork.VoucherRepository.Update(voucher);
                }
            }

            appointment.Status = AppointmentStatus.Confirmed;

            _unitOfWork.TransactionRepository.Update(transaction);
            _unitOfWork.AppointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true, message = "Xác nhận thanh toán thành công" });
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-appointment")]
        public async Task<IActionResult> GetAppointment([FromForm] GetAppoinmentCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
