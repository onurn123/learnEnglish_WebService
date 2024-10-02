using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.ApplicationDatabaseContext;
using WebApplication1.LoggingFactory;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json")
.AddEnvironmentVariables()
.Build();
string connectionString = config["ConnectionStrings:DefaultServer"];
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<dbContext>(options =>
        options.UseSqlServer(connectionString));
builder.Services.AddCors(options =>
         options.AddPolicy(MyAllowSpecificOrigins, p => p.WithOrigins(
                ["https://le.onurcanin.com",
                "http://le.onurcanin.com",
                "http://localhost:3000"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                ));
var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<RequestLoggingMiddleware>>();
builder.Services.AddSingleton(typeof(ILogger), logger);
builder.Services.AddControllers();
var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
