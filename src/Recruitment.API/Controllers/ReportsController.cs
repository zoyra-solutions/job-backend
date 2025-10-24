using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Get KPI report for recruiter
    /// </summary>
    [HttpGet("kpi")]
    public async Task<IActionResult> GetKpiReport([FromQuery] Guid? recruiterId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var report = await _reportService.GetKpiReportAsync(recruiterId, from, to, User.GetUserId());
            return Ok(report);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving KPI report");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get vacancy report
    /// </summary>
    [HttpGet("vacancy")]
    public async Task<IActionResult> GetVacancyReport([FromQuery] Guid? vacancyId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var report = await _reportService.GetVacancyReportAsync(vacancyId, from, to, User.GetUserId());
            return Ok(report);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vacancy report");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get commission report
    /// </summary>
    [HttpGet("commission")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetCommissionReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var report = await _reportService.GetCommissionReportAsync(from, to);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving commission report");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Export report to CSV
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportReport([FromQuery] string type, [FromQuery] Guid? id, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var exportData = await _reportService.ExportReportAsync(type, id, from, to, User.GetUserId());
            return File(exportData, "text/csv", $"{type}_report_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting report");
            return StatusCode(500, "Internal server error");
        }
    }
}