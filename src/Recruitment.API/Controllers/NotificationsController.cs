using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Recruitment.Infrastructure.Services;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(NotificationService notificationService, ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Send email notification
    /// </summary>
    [HttpPost("email")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDto dto)
    {
        try
        {
            await _notificationService.SendEmailAsync(dto.To, dto.Subject, dto.Body);
            return Ok(new { message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Send SMS notification
    /// </summary>
    [HttpPost("sms")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SendSMS([FromBody] SendSMSDto dto)
    {
        try
        {
            await _notificationService.SendSMSAsync(dto.To, dto.Message);
            return Ok(new { message = "SMS sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Send push notification
    /// </summary>
    [HttpPost("push")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SendPushNotification([FromBody] SendPushDto dto)
    {
        try
        {
            await _notificationService.SendPushNotificationAsync(dto.Token, dto.Title, dto.Body);
            return Ok(new { message = "Push notification sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending push notification");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class SendEmailDto
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class SendSMSDto
{
    public string To { get; set; }
    public string Message { get; set; }
}

public class SendPushDto
{
    public string Token { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}