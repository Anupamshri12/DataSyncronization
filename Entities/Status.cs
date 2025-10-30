namespace WebApplication4.Entities
{
    public class Status
    {
        public int pending_sync_count { get; set; } = 0;
        public DateTime? last_sync_timestamp { get; set; }

        public int is_online { get; set; } = 1;

        public int sync_queu_size { get; set; } = 0;

    }
}
