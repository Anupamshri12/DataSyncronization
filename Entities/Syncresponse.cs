namespace WebApplication4.Entities
{
    public class Syncresponse
    {
        public bool Success { get; set; } = true;
        public int synced_items { get; set; } = 0;
        public int failed_items { get; set; } = 0;

        public List<ErrorMessage> ?errors { get; set; }
    }

    public class ErrorMessage
    {
        public required string task_id { get; set; }
        public required string operation { get; set; }

        public required string error { get; set; }

        public DateTime timestamp { get; set; }

    }
}
