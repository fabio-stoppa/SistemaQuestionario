using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Domain.Interfaces;
using Application.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]!));
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

// RabbitMQ
builder.Services.AddSingleton<IMessageQueueService>(sp =>
{
    var config = builder.Configuration.GetSection("RabbitMQ");
    return new RabbitMQService(
        config["Host"]!,
        config["Username"]!,
        config["Password"]!
    );
});

// Repositories
builder.Services.AddScoped<IQuestionarioRepository, QuestionarioRepository>();
builder.Services.AddScoped<IRespostaRepository, RespostaRepository>();
builder.Services.AddScoped<IResultadoRepository, ResultadoRepository>();

// Services
builder.Services.AddScoped<QuestionarioService>();
builder.Services.AddScoped<RespostaService>();
builder.Services.AddScoped<ResultadoService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// AUTOMATIC MIGRATIONS ON STARTUP
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        int retries = 10;
        while (retries > 0)
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Database is up to date!");
                }
                break;
            }
            catch (Exception)
            {
                retries--;
                if (retries == 0) throw;
                Console.WriteLine("Waiting for SQL Server to be ready...");
                Thread.Sleep(5000);
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
