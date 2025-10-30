//using Microsoft.AspNetCore.Mvc;
//using WebApplication4.ApplicationDBContext;
//using WebApplication4.Entities;
//using WebApplication4.RabbitMQ;

//namespace WebApplication4.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : Controller
//    {
//        public RabbitMQPublisher _rabbitMQPublisher;
//        private readonly ApplicationDbContext _dbcontext;

//        public UserController(RabbitMQPublisher rabbitMQPublisher ,ApplicationDbContext context)
//        {
//            _dbcontext = context;
//            _rabbitMQPublisher = rabbitMQPublisher;
//        }
//        [HttpPost]
//        [Route("register")]
//        public async Task<IActionResult> RegisterUser(Register register)
//        {
//            var newregister = new UserModel()
//            {
//                FirstName = register.FirstName,
//                LastName = register.LastName,
//                UserName    = register.UserName,
//                Email = register.Email,
//                Password = register.Password,
//                PhoneNumber = register.PhoneNumber
//            };

//            var newnotification = new NotificationResponse()
//            {
//                Id = newregister.UserId,
//                FirstName = newregister.FirstName,
//                LastName = newregister.LastName,
//                UserName = newregister.UserName,
//                Email = newregister.Email,
//                PhoneNumber = newregister.PhoneNumber
//            };

//            await _dbcontext.User.AddAsync(newregister);
//            await _dbcontext.SaveChangesAsync();  
//             _rabbitMQPublisher.PublishAsync(newnotification).GetAwaiter().GetResult();

//            return Ok(newnotification);
//        }
//    }
//}
