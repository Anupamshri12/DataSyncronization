using System;

namespace WebApplication4.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Completed { get; set; } = 0;
        public int IsDeleted { get; set; } = 0;
        public string SyncStatus { get; set; } = "pending"; 
        public string? ServerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastSyncedAt { get; set; }
    }
}
