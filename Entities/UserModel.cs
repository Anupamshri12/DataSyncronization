using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Entities
{
    [Table("New_User")]
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string ?LastName { get; set; }

        public string UserName { get; set; }


        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }


    }
}
