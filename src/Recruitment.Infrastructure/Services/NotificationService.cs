using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Recruitment.Infrastructure.Services;

public class NotificationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public NotificationService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var apiKey = _configuration["Notifications:Email:ApiKey"];
        var fromEmail = _configuration["Notifications:Email:FromEmail"];

        var emailData = new
        {
            personalizations = new[]
            {
                new
                {
                    to = new[] { new { email = to } },
                    subject = subject
                }
            },
            from = new { email = fromEmail },
            content = new[] { new { type = "text/plain", value = body } }
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.PostAsJsonAsync("https://api.sendgrid.com/v3/mail/send", emailData);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendSMSAsync(string to, string message)
    {
        var accountSid = _configuration["Notifications:SMS:AccountSid"];
        var authToken = _configuration["Notifications:SMS:AuthToken"];
        var fromNumber = _configuration["Notifications:SMS:FromNumber"];

        var formData = new Dictionary<string, string>
        {
            ["To"] = to,
            ["From"] = fromNumber,
            ["Body"] = message
        };

        var response = await _httpClient.PostAsync($"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json", new FormUrlEncodedContent(formData));
        response.EnsureSuccessStatusCode();
    }

    public async Task SendPushNotificationAsync(string token, string title, string body)
    {
        var serverKey = _configuration["Notifications:Firebase:ServerKey"];

        var notificationData = new
        {
            to = token,
            notification = new
            {
                title = title,
                body = body
            }
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"key={serverKey}");
        _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

        var response = await _httpClient.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", notificationData);
        response.EnsureSuccessStatusCode();
    }
}