using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VacanciesController : ControllerBase
{
    private readonly IVacancyService _vacancyService;
    private readonly ILogger<VacanciesController> _logger;

    public VacanciesController(IVacancyService vacancyService, ILogger<VacanciesController> logger)
    {
        _vacancyService = vacancyService;
        _logger = logger;
    }

    /// <summary>
    /// Get all vacancies for the current user's company
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetVacancies([FromQuery] VacancyFilterDto filter)
    {
        try
        {
            var vacancies = await _vacancyService.GetVacanciesAsync(User.GetUserId(), filter);
            return Ok(vacancies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vacancies");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get vacancy by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVacancy(Guid id)
    {
        try
        {
            var vacancy = await _vacancyService.GetVacancyByIdAsync(id, User.GetUserId());
            if (vacancy == null)
                return NotFound();

            return Ok(vacancy);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vacancy {VacancyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new vacancy
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyDto dto)
    {
        try
        {
            var vacancy = await _vacancyService.CreateVacancyAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(GetVacancy), new { id = vacancy.Id }, vacancy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vacancy");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update vacancy
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> UpdateVacancy(Guid id, [FromBody] UpdateVacancyDto dto)
    {
        try
        {
            var vacancy = await _vacancyService.UpdateVacancyAsync(id, dto, User.GetUserId());
            return Ok(vacancy);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vacancy {VacancyId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete vacancy
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> DeleteVacancy(Guid id)
    {
        try
        {
            await _vacancyService.DeleteVacancyAsync(id, User.GetUserId());
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vacancy {VacancyId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Submit application to vacancy
    /// </summary>
    [HttpPost("{id}/applications")]
    [Authorize(Roles = "recruiter")]
    public async Task<IActionResult> SubmitApplicationToVacancy(Guid id, [FromBody] CreateApplicationDto dto)
    {
        try
        {
            var application = await _vacancyService.SubmitApplicationToVacancyAsync(id, dto, User.GetUserId());
            return CreatedAtAction(nameof(GetVacancy), new { id = id }, application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting application to vacancy {VacancyId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get vacancy statistics
    /// </summary>
    [HttpGet("{id}/statistics")]
    public async Task<IActionResult> GetVacancyStatistics(Guid id)
    {
        try
        {
            var stats = await _vacancyService.GetVacancyStatisticsAsync(id, User.GetUserId());
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vacancy statistics {VacancyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}