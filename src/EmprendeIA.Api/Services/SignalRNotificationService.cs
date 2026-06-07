using System;
using System.Threading.Tasks;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EmprendeIA.Api.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMatchNotificationAsync(Guid userId, Guid projectId, string projectTitle, decimal matchScore)
    {
        // Broadcasts to the group matching the target user ID
        await _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", new
        {
            id = Guid.NewGuid(),
            projectId = projectId,
            projectTitle = projectTitle,
            investorId = userId, // Represented as target profile reference
            investorName = "Inversor / Mentor Recomendado por IA",
            matchScore = (double)matchScore,
            createdAt = DateTime.UtcNow
        });
    }
}
