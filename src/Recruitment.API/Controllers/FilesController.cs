using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly FileStorageService _fileStorageService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(FileStorageService fileStorageService, ILogger<FilesController> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload file
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string type)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var contentType = file.ContentType;

            using var stream = file.OpenReadStream();
            var filePath = await _fileStorageService.UploadFileAsync(fileName, stream, contentType);

            return Ok(new { filePath });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Download file
    /// </summary>
    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        try
        {
            var stream = await _fileStorageService.GetFileAsync(fileName);
            return File(stream, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FileName}", fileName);
            return NotFound();
        }
    }

    /// <summary>
    /// Delete file
    /// </summary>
    [HttpDelete("{fileName}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        try
        {
            await _fileStorageService.DeleteFileAsync(fileName);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName}", fileName);
            return StatusCode(500, "Internal server error");
        }
    }
}