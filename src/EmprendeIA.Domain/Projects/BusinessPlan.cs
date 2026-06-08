using System;

namespace EmprendeIA.Domain.Projects
{
    public class BusinessPlan
    {
        public Guid ProjectId { get; private set; }
        public string Content { get; private set; } = string.Empty;
        public DateTime GeneratedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Navigation
        public Project? Project { get; private set; }

        private BusinessPlan() { }

        public BusinessPlan(Guid projectId, string content)
        {
            ProjectId = projectId;
            Content = content;
            GeneratedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string content)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
