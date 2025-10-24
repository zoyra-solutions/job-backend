using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(SearchService searchService, ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    /// <summary>
    /// Search vacancies and candidates
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int size = 10)
    {
        try
        {
            var results = await _searchService.SearchAsync(query, size);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Index vacancy
    /// </summary>
    [HttpPost("index/vacancy")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> IndexVacancy([FromBody] object vacancy)
    {
        try
        {
            await _searchService.IndexVacancyAsync(vacancy);
            return Ok(new { message = "Vacancy indexed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error indexing vacancy");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Index candidate
    /// </summary>
    [HttpPost("index/candidate")]
    [Authorize(Roles = "recruiter")]
    public async Task<IActionResult> IndexCandidate([FromBody] object candidate)
    {
        try
        {
            await _searchService.IndexCandidateAsync(candidate);
            return Ok(new { message = "Candidate indexed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error indexing candidate");
            return StatusCode(500, "Internal server error");
        }
    }
}