namespace WebApplication4.Entities
{
    public class Register
    {
        public required string FirstName { get; set; }

        public string ?LastName { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
