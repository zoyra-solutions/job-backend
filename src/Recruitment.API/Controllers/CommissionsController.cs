using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommissionsController : ControllerBase
{
    private readonly ICommissionService _commissionService;
    private readonly IHubContext<RealTimeService, IRealTimeClient> _hubContext;
    private readonly ILogger<CommissionsController> _logger;

    public CommissionsController(
        ICommissionService commissionService,
        IHubContext<RealTimeService, IRealTimeClient> hubContext,
        ILogger<CommissionsController> logger)
    {
        _commissionService = commissionService;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all commissions for current user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCommissions([FromQuery] CommissionFilterDto filter)
    {
        try
        {
            var commissions = await _commissionService.GetCommissionsAsync(User.GetUserId(), filter);
            return Ok(commissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving commissions");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get commission by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommission(Guid id)
    {
        try
        {
            var commission = await _commissionService.GetCommissionByIdAsync(id, User.GetUserId());
            if (commission == null)
                return NotFound();

            return Ok(commission);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving commission {CommissionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new commission (System/Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateCommission([FromBody] CreateCommissionDto dto)
    {
        try
        {
            var commission = await _commissionService.CreateCommissionAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(GetCommission), new { id = commission.Id }, commission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating commission");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update commission
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateCommission(Guid id, [FromBody] UpdateCommissionDto dto)
    {
        try
        {
            var commission = await _commissionService.UpdateCommissionAsync(id, dto, User.GetUserId());
            return Ok(commission);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating commission {CommissionId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Process bulk commission payments (Admin only)
    /// </summary>
    [HttpPost("bulk-payment")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ProcessBulkPayments([FromBody] BulkCommissionPaymentDto dto)
    {
        try
        {
            var results = await _commissionService.ProcessBulkCommissionPaymentsAsync(dto, User.GetUserId());

            // Send real-time notifications to recruiters
            foreach (var result in results)
            {
                if (result.Success)
                {
                    await _hubContext.Clients.Group($"commissions_{result.RecruiterId}")
                        .CommissionPaid(result.RecruiterId, new { commissionId = result.CommissionId, result.TransactionId });
                }
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing bulk commission payments");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get recruiter earnings summary
    /// </summary>
    [HttpGet("earnings/{recruiterId}")]
    public async Task<IActionResult> GetRecruiterEarnings(Guid recruiterId, [FromQuery] DateTime? periodStart, [FromQuery] DateTime? periodEnd)
    {
        try
        {
            var earnings = await _commissionService.GetRecruiterEarningsAsync(recruiterId, periodStart, periodEnd);
            return Ok(earnings);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recruiter earnings for {RecruiterId}", recruiterId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get all recruiters earnings (Admin only)
    /// </summary>
    [HttpGet("earnings")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllRecruitersEarnings([FromQuery] DateTime periodStart, [FromQuery] DateTime periodEnd)
    {
        try
        {
            var earnings = await _commissionService.GetAllRecruitersEarningsAsync(periodStart, periodEnd);
            return Ok(earnings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all recruiters earnings");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Calculate commission amount for application
    /// </summary>
    [HttpPost("calculate")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CalculateCommission([FromBody] CalculateCommissionDto dto)
    {
        try
        {
            var amount = await _commissionService.CalculateCommissionAmountAsync(dto.ApplicationId, dto.CommissionRuleId);
            return Ok(new { amount, currency = "VND" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating commission amount");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Dispute commission (Recruiter/Employer/Admin)
    /// </summary>
    [HttpPost("{id}/dispute")]
    [Authorize]
    public async Task<IActionResult> DisputeCommission(Guid id, [FromBody] CreateDisputeDto dto)
    {
        try
        {
            var dispute = await _commissionService.DisputeCommissionAsync(id, dto, User.GetUserId());
            return Ok(dispute);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disputing commission {CommissionId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get commission statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetCommissionStatistics()
    {
        try
        {
            var stats = await _commissionService.GetCommissionStatisticsAsync(User.GetUserId());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving commission statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Export commission data
    /// </summary>
    [HttpGet("export")]
    [Authorize(Roles = "admin,employer")]
    public async Task<IActionResult> ExportCommissions([FromQuery] CommissionFilterDto filter)
    {
        try
        {
            var exportData = await _commissionService.ExportCommissionsAsync(filter, User.GetUserId());
            return File(exportData, "text/csv", $"commissions_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting commission data");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get pending commissions for payment processing
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetPendingCommissions()
    {
        try
        {
            var pendingCommissions = await _commissionService.GetPendingCommissionsAsync();
            return Ok(pendingCommissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending commissions");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Process pending commissions automatically
    /// </summary>
    [HttpPost("process-pending")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ProcessPendingCommissions()
    {
        try
        {
            await _commissionService.ProcessPendingCommissionsAsync();
            return Ok(new { message = "Pending commissions processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending commissions");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Reconcile transactions
    /// </summary>
    [HttpPost("reconcile")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ReconcileTransactions([FromBody] ReconcileRequestDto dto)
    {
        try
        {
            var result = await _commissionService.ReconcileTransactionsAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reconciling transactions");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get reconciliation report
    /// </summary>
    [HttpGet("reconciliation-report")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetReconciliationReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var report = await _commissionService.GetReconciliationReportAsync(from, to);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reconciliation report");
            return StatusCode(500, "Internal server error");
        }
    }
}