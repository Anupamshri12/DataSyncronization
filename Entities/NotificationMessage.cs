namespace WebApplication4.Entities
{
    public class NotificationMessage
    {
        public int UserId { get; set; }

        public required string Recipient { get; set; }

        public string ?Body { get; set; }

        public required string Type { get; set; }
    }
}
