using Microsoft.EntityFrameworkCore;
using RailSys.Models;
using RailSys.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<NotificationService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<RailSysDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
