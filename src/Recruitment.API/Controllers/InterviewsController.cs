using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewsController : ControllerBase
{
    private readonly IInterviewService _interviewService;
    private readonly IHubContext<RealTimeService, IRealTimeClient> _hubContext;
    private readonly ILogger<InterviewsController> _logger;

    public InterviewsController(
        IInterviewService interviewService,
        IHubContext<RealTimeService, IRealTimeClient> hubContext,
        ILogger<InterviewsController> logger)
    {
        _interviewService = interviewService;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all interviews for current user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetInterviews([FromQuery] InterviewFilterDto filter)
    {
        try
        {
            var interviews = await _interviewService.GetInterviewsAsync(User.GetUserId(), filter);
            return Ok(interviews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interviews");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get interview by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInterview(Guid id)
    {
        try
        {
            var interview = await _interviewService.GetInterviewByIdAsync(id, User.GetUserId());
            if (interview == null)
                return NotFound();

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interview {InterviewId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Schedule new interview
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> ScheduleInterview([FromBody] CreateInterviewDto dto)
    {
        try
        {
            var interview = await _interviewService.ScheduleInterviewAsync(dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{dto.ApplicationId}")
                .InterviewScheduled(dto.ApplicationId, interview);

            return CreatedAtAction(nameof(GetInterview), new { id = interview.Id }, interview);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scheduling interview");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update interview details
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> UpdateInterview(Guid id, [FromBody] UpdateInterviewDto dto)
    {
        try
        {
            var interview = await _interviewService.UpdateInterviewAsync(id, dto, User.GetUserId());

            // Send real-time notification if rescheduled
            if (dto.ScheduledAt != null)
            {
                await _hubContext.Clients.Group($"application_{interview.ApplicationId}")
                    .InterviewRescheduled(interview.ApplicationId, interview);
            }

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancel interview
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> CancelInterview(Guid id)
    {
        try
        {
            var interview = await _interviewService.CancelInterviewAsync(id, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{interview.ApplicationId}")
                .InterviewCancelled(interview.ApplicationId, interview);

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Complete interview with result
    /// </summary>
    [HttpPatch("{id}/complete")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> CompleteInterview(Guid id, [FromBody] CompleteInterviewDto dto)
    {
        try
        {
            var interview = await _interviewService.CompleteInterviewAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{interview.ApplicationId}")
                .InterviewCompleted(interview.ApplicationId, interview);

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Add interview notes
    /// </summary>
    [HttpPost("{id}/notes")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> AddInterviewNotes(Guid id, [FromBody] CreateInterviewNoteDto dto)
    {
        try
        {
            var note = await _interviewService.AddInterviewNoteAsync(id, dto, User.GetUserId());
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding notes to interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Upload interview recording
    /// </summary>
    [HttpPost("{id}/recording")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> UploadRecording(Guid id, [FromForm] UploadRecordingDto dto)
    {
        try
        {
            var recording = await _interviewService.UploadRecordingAsync(id, dto, User.GetUserId());
            return Ok(recording);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading recording for interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get interview recordings
    /// </summary>
    [HttpGet("{id}/recordings")]
    public async Task<IActionResult> GetInterviewRecordings(Guid id)
    {
        try
        {
            var recordings = await _interviewService.GetInterviewRecordingsAsync(id, User.GetUserId());
            return Ok(recordings);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recordings for interview {InterviewId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Generate interview report
    /// </summary>
    [HttpGet("{id}/report")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> GenerateInterviewReport(Guid id)
    {
        try
        {
            var report = await _interviewService.GenerateInterviewReportAsync(id, User.GetUserId());
            return Ok(report);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating report for interview {InterviewId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get interview statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetInterviewStatistics()
    {
        try
        {
            var stats = await _interviewService.GetInterviewStatisticsAsync(User.GetUserId());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interview statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Bulk schedule interviews
    /// </summary>
    [HttpPost("bulk-schedule")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> BulkScheduleInterviews([FromBody] BulkScheduleDto dto)
    {
        try
        {
            var results = await _interviewService.BulkScheduleInterviewsAsync(dto, User.GetUserId());
            return Ok(results);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk scheduling interviews");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get available time slots for interviewer
    /// </summary>
    [HttpGet("availability/{interviewerId}")]
    public async Task<IActionResult> GetInterviewerAvailability(Guid interviewerId, [FromQuery] DateTime date)
    {
        try
        {
            var availability = await _interviewService.GetInterviewerAvailabilityAsync(interviewerId, date);
            return Ok(availability);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving availability for interviewer {InterviewerId}", interviewerId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Reschedule interview
    /// </summary>
    [HttpPatch("{id}/reschedule")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> RescheduleInterview(Guid id, [FromBody] RescheduleInterviewDto dto)
    {
        try
        {
            var interview = await _interviewService.RescheduleInterviewAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{interview.ApplicationId}")
                .InterviewRescheduled(interview.ApplicationId, interview);

            return Ok(interview);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rescheduling interview {InterviewId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get interview templates
    /// </summary>
    [HttpGet("templates")]
    [Authorize]
    public async Task<IActionResult> GetInterviewTemplates()
    {
        try
        {
            var templates = await _interviewService.GetInterviewTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interview templates");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create custom interview template
    /// </summary>
    [HttpPost("templates")]
    [Authorize(Roles = "employer,interviewer")]
    public async Task<IActionResult> CreateInterviewTemplate([FromBody] CreateInterviewTemplateDto dto)
    {
        try
        {
            var template = await _interviewService.CreateInterviewTemplateAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(GetInterviewTemplates), template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating interview template");
            return BadRequest(ex.Message);
        }
    }
}