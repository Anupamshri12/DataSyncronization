namespace WebApplication4.Entities
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string ?LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> Channels = new List<string> { "Email" ,"SMS"};

    }
}
