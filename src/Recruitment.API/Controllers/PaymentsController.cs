using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(PaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Create payment
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
    {
        try
        {
            var paymentId = await _paymentService.CreatePaymentAsync(dto.Amount, dto.Description, dto.CallbackUrl);
            return Ok(new { paymentId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Verify payment
    /// </summary>
    [HttpGet("{paymentId}/verify")]
    public async Task<IActionResult> VerifyPayment(string paymentId)
    {
        try
        {
            var isVerified = await _paymentService.VerifyPaymentAsync(paymentId);
            return Ok(new { isVerified });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying payment {PaymentId}", paymentId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Transfer money
    /// </summary>
    [HttpPost("transfer")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyDto dto)
    {
        try
        {
            var transactionId = await _paymentService.TransferMoneyAsync(dto.FromAccount, dto.ToAccount, dto.Amount, dto.Description);
            return Ok(new { transactionId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transferring money");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Payment webhook
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentWebhook([FromBody] object webhookData)
    {
        try
        {
            // Process webhook data
            _logger.LogInformation("Payment webhook received: {WebhookData}", webhookData);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment webhook");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class CreatePaymentDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string CallbackUrl { get; set; }
}

public class TransferMoneyDto
{
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}