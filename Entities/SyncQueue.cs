using System;

namespace WebApplication4.Entities
{
    public class SyncQueue
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string? Operation { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public required string SyncStatus { get; set; }
        public int RetryCount { get; set; } = 0;
    }
}
