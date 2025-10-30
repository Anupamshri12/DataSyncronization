using Microsoft.EntityFrameworkCore;
using WebApplication4.ApplicationDBContext;
//using WebApplication4.Implementation;
//using WebApplication4.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddDbContext<ServerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlite("Data Source=localtasks.db"));

//builder.Services.AddScoped<INotificationStrategy, EmailNotificationStrategy>();
//builder.Services.AddScoped<INotificationStrategy, SMSNotificationStrategy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Fix: Await the StartAsync method to ensure proper execution flow
//var worker = new NotificationWorker();
//await worker.StartAsync();

app.Run();
