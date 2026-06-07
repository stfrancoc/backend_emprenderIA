using System;
using System.Threading.Tasks;

namespace EmprendeIA.Domain.Interfaces;

public interface INotificationService
{
    Task SendMatchNotificationAsync(Guid userId, Guid projectId, string projectTitle, decimal matchScore);
}
