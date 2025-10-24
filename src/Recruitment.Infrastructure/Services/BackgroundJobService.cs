using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Recruitment.Infrastructure.Services;

public class BackgroundJobService
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;

    public BackgroundJobService(IBus bus, IConfiguration configuration)
    {
        _bus = bus;
        _configuration = configuration;
    }

    public async Task SendEmailJobAsync(string to, string subject, string body)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:send-email"));
        await endpoint.Send(new
        {
            To = to,
            Subject = subject,
            Body = body
        });
    }

    public async Task SendSMSJobAsync(string to, string message)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:send-sms"));
        await endpoint.Send(new
        {
            To = to,
            Message = message
        });
    }

    public async Task ProcessCommissionJobAsync(Guid commissionId)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:process-commission"));
        await endpoint.Send(new
        {
            CommissionId = commissionId
        });
    }

    public async Task GenerateReportJobAsync(string reportType, Guid? id, DateTime? from, DateTime? to)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:generate-report"));
        await endpoint.Send(new
        {
            ReportType = reportType,
            Id = id,
            From = from,
            To = to
        });
    }
}