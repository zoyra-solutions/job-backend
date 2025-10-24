using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IHubContext<RealTimeService, IRealTimeClient> _hubContext;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(
        IApplicationService applicationService,
        IHubContext<RealTimeService, IRealTimeClient> hubContext,
        ILogger<ApplicationsController> logger)
    {
        _applicationService = applicationService;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all applications for current user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetApplications([FromQuery] ApplicationFilterDto filter)
    {
        try
        {
            var applications = await _applicationService.GetApplicationsAsync(User.GetUserId(), filter);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        try
        {
            var application = await _applicationService.GetApplicationByIdAsync(id, User.GetUserId());
            if (application == null)
                return NotFound();

            return Ok(application);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application {ApplicationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Submit new application (Recruiter only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "recruiter")]
    public async Task<IActionResult> SubmitApplication([FromBody] CreateApplicationDto dto)
    {
        try
        {
            var application = await _applicationService.SubmitApplicationAsync(dto, User.GetUserId());

            // Send real-time notification to vacancy owner
            await _hubContext.Clients.Group($"vacancy_{dto.VacancyId}")
                .NewApplicationReceived(dto.VacancyId, application);

            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting application");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update application status (Employer/Interviewer only)
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> UpdateApplicationStatus(Guid id, [FromBody] UpdateApplicationStatusDto dto)
    {
        try
        {
            var application = await _applicationService.UpdateApplicationStatusAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"vacancy_{application.VacancyId}")
                .ApplicationStatusChanged(application.VacancyId, id, dto.Status);

            return Ok(application);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application status {ApplicationId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Add interview to application
    /// </summary>
    [HttpPost("{id}/interviews")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> ScheduleInterview(Guid id, [FromBody] CreateInterviewDto dto)
    {
        try
        {
            var interview = await _applicationService.ScheduleInterviewAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{id}")
                .InterviewScheduled(id, interview);

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scheduling interview for application {ApplicationId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update interview result
    /// </summary>
    [HttpPatch("{applicationId}/interviews/{interviewId}")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> UpdateInterviewResult(Guid applicationId, Guid interviewId, [FromBody] UpdateInterviewDto dto)
    {
        try
        {
            var interview = await _applicationService.UpdateInterviewResultAsync(applicationId, interviewId, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{applicationId}")
                .InterviewCompleted(applicationId, interview);

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating interview result for application {ApplicationId}", applicationId);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Upload candidate documents
    /// </summary>
    [HttpPost("{id}/documents")]
    [Authorize(Roles = "recruiter")]
    public async Task<IActionResult> UploadDocuments(Guid id, [FromForm] UploadDocumentsDto dto)
    {
        try
        {
            var documents = await _applicationService.UploadDocumentsAsync(id, dto, User.GetUserId());
            return Ok(documents);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading documents for application {ApplicationId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get application timeline
    /// </summary>
    [HttpGet("{id}/timeline")]
    public async Task<IActionResult> GetApplicationTimeline(Guid id)
    {
        try
        {
            var timeline = await _applicationService.GetApplicationTimelineAsync(id, User.GetUserId());
            return Ok(timeline);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving timeline for application {ApplicationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Add note to application
    /// </summary>
    [HttpPost("{id}/notes")]
    [Authorize]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] CreateNoteDto dto)
    {
        try
        {
            var note = await _applicationService.AddNoteAsync(id, dto, User.GetUserId());
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding note to application {ApplicationId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get interviews for application
    /// </summary>
    [HttpGet("{id}/interviews")]
    public async Task<IActionResult> GetApplicationInterviews(Guid id)
    {
        try
        {
            var interviews = await _applicationService.GetApplicationInterviewsAsync(id, User.GetUserId());
            return Ok(interviews);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interviews for application {ApplicationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get application statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetApplicationStatistics()
    {
        try
        {
            var stats = await _applicationService.GetApplicationStatisticsAsync(User.GetUserId());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Bulk update application statuses
    /// </summary>
    [HttpPatch("bulk/status")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkStatusUpdateDto dto)
    {
        try
        {
            var results = await _applicationService.BulkUpdateStatusAsync(dto, User.GetUserId());
            return Ok(results);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk updating application statuses");
            return BadRequest(ex.Message);
        }
    }
}