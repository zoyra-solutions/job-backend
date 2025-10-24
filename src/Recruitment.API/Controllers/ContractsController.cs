using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IHubContext<RealTimeService, IRealTimeClient> _hubContext;
    private readonly ILogger<ContractsController> _logger;

    public ContractsController(
        IContractService contractService,
        IHubContext<RealTimeService, IRealTimeClient> hubContext,
        ILogger<ContractsController> logger)
    {
        _contractService = contractService;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all contracts for current user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetContracts([FromQuery] ContractFilterDto filter)
    {
        try
        {
            var contracts = await _contractService.GetContractsAsync(User.GetUserId(), filter);
            return Ok(contracts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contracts");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get contract by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContract(Guid id)
    {
        try
        {
            var contract = await _contractService.GetContractByIdAsync(id, User.GetUserId());
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contract {ContractId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new contract
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
    {
        try
        {
            var contract = await _contractService.CreateContractAsync(dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{dto.ApplicationId}")
                .ContractCreated(dto.ApplicationId, contract);

            return CreatedAtAction(nameof(GetContract), new { id = contract.Id }, contract);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contract");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Sign contract (Candidate signature)
    /// </summary>
    [HttpPost("{id}/sign-candidate")]
    [Authorize(Roles = "recruiter")]
    public async Task<IActionResult> SignContractByCandidate(Guid id, [FromBody] SignContractDto dto)
    {
        try
        {
            var contract = await _contractService.SignContractByCandidateAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{contract.ApplicationId}")
                .ContractSigned(contract.ApplicationId, "candidate", contract);

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing contract by candidate {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Sign contract (Employer signature)
    /// </summary>
    [HttpPost("{id}/sign-employer")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> SignContractByEmployer(Guid id, [FromBody] SignContractDto dto)
    {
        try
        {
            var contract = await _contractService.SignContractByEmployerAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{contract.ApplicationId}")
                .ContractSigned(contract.ApplicationId, "employer", contract);

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing contract by employer {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Upload contract document
    /// </summary>
    [HttpPost("{id}/document")]
    [Authorize]
    public async Task<IActionResult> UploadContractDocument(Guid id, [FromForm] UploadContractDocumentDto dto)
    {
        try
        {
            var document = await _contractService.UploadContractDocumentAsync(id, dto, User.GetUserId());
            return Ok(document);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading contract document {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Generate contract from template
    /// </summary>
    [HttpPost("generate")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> GenerateContract([FromBody] GenerateContractDto dto)
    {
        try
        {
            var contract = await _contractService.GenerateContractFromTemplateAsync(dto, User.GetUserId());
            return Ok(contract);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating contract from template");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update contract status
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateContractStatus(Guid id, [FromBody] UpdateContractStatusDto dto)
    {
        try
        {
            var contract = await _contractService.UpdateContractStatusAsync(id, dto, User.GetUserId());
            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contract status {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Terminate contract
    /// </summary>
    [HttpPatch("{id}/terminate")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> TerminateContract(Guid id, [FromBody] TerminateContractDto dto)
    {
        try
        {
            var contract = await _contractService.TerminateContractAsync(id, dto, User.GetUserId());

            // Send real-time notification
            await _hubContext.Clients.Group($"application_{contract.ApplicationId}")
                .ContractTerminated(contract.ApplicationId, contract);

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error terminating contract {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get contract templates
    /// </summary>
    [HttpGet("templates")]
    [Authorize]
    public async Task<IActionResult> GetContractTemplates()
    {
        try
        {
            var templates = await _contractService.GetContractTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contract templates");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create custom contract template
    /// </summary>
    [HttpPost("templates")]
    [Authorize(Roles = "employer,admin")]
    public async Task<IActionResult> CreateContractTemplate([FromBody] CreateContractTemplateDto dto)
    {
        try
        {
            var template = await _contractService.CreateContractTemplateAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(GetContractTemplates), template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contract template");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get contract statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetContractStatistics()
    {
        try
        {
            var stats = await _contractService.GetContractStatisticsAsync(User.GetUserId());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contract statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Export contract data
    /// </summary>
    [HttpGet("export")]
    [Authorize(Roles = "admin,employer")]
    public async Task<IActionResult> ExportContracts([FromQuery] ContractFilterDto filter)
    {
        try
        {
            var exportData = await _contractService.ExportContractsAsync(filter, User.GetUserId());
            return File(exportData, "text/csv", $"contracts_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting contract data");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Validate contract before signing
    /// </summary>
    [HttpPost("{id}/validate")]
    [Authorize]
    public async Task<IActionResult> ValidateContract(Guid id)
    {
        try
        {
            var validation = await _contractService.ValidateContractAsync(id, User.GetUserId());
            return Ok(validation);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating contract {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Send contract for signature
    /// </summary>
    [HttpPost("{id}/send-for-signature")]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> SendForSignature(Guid id)
    {
        try
        {
            var contract = await _contractService.SendContractForSignatureAsync(id, User.GetUserId());

            // Send real-time notification to candidate
            await _hubContext.Clients.Group($"application_{contract.ApplicationId}")
                .ContractSentForSignature(contract.ApplicationId, contract);

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending contract for signature {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get contract audit trail
    /// </summary>
    [HttpGet("{id}/audit")]
    [Authorize]
    public async Task<IActionResult> GetContractAuditTrail(Guid id)
    {
        try
        {
            var auditTrail = await _contractService.GetContractAuditTrailAsync(id, User.GetUserId());
            return Ok(auditTrail);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit trail for contract {ContractId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Archive contract
    /// </summary>
    [HttpPatch("{id}/archive")]
    [Authorize(Roles = "employer,admin")]
    public async Task<IActionResult> ArchiveContract(Guid id)
    {
        try
        {
            await _contractService.ArchiveContractAsync(id, User.GetUserId());
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error archiving contract {ContractId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get contract by application ID
    /// </summary>
    [HttpGet("by-application/{applicationId}")]
    public async Task<IActionResult> GetContractByApplication(Guid applicationId)
    {
        try
        {
            var contract = await _contractService.GetContractByApplicationIdAsync(applicationId, User.GetUserId());
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contract for application {ApplicationId}", applicationId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Bulk generate contracts
    /// </summary>
    [HttpPost("bulk-generate")]
    [Authorize(Roles = "employer,admin")]
    public async Task<IActionResult> BulkGenerateContracts([FromBody] BulkGenerateContractsDto dto)
    {
        try
        {
            var results = await _contractService.BulkGenerateContractsAsync(dto, User.GetUserId());
            return Ok(results);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk generating contracts");
            return BadRequest(ex.Message);
        }
    }
}