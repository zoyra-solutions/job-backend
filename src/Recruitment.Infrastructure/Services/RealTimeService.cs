using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Recruitment.Infrastructure.Services;

public interface IRealTimeClient
{
    Task NewApplicationReceived(Guid vacancyId, object application);
    Task ApplicationStatusChanged(Guid vacancyId, Guid applicationId, string status);
    Task InterviewScheduled(Guid applicationId, object interview);
    Task InterviewRescheduled(Guid applicationId, object interview);
    Task InterviewCancelled(Guid applicationId, object interview);
    Task InterviewCompleted(Guid applicationId, object interview);
    Task CommissionPaid(Guid recruiterId, object commission);
    Task NotificationReceived(Guid userId, object notification);
}

public class RealTimeService : Hub<IRealTimeClient>
{
    public async Task JoinVacancyGroup(Guid vacancyId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"vacancy_{vacancyId}");
    }

    public async Task LeaveVacancyGroup(Guid vacancyId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"vacancy_{vacancyId}");
    }

    public async Task JoinApplicationGroup(Guid applicationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"application_{applicationId}");
    }

    public async Task LeaveApplicationGroup(Guid applicationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"application_{applicationId}");
    }

    public async Task JoinCommissionGroup(Guid recruiterId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"commissions_{recruiterId}");
    }

    public async Task LeaveCommissionGroup(Guid recruiterId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"commissions_{recruiterId}");
    }

    public async Task JoinUserGroup(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
    }

    public async Task LeaveUserGroup(Guid userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
    }

    public override async Task OnConnectedAsync()
    {
        // Handle user connection
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Handle user disconnection
        await base.OnDisconnectedAsync(exception);
    }
}