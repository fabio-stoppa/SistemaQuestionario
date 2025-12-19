using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Domain.Interfaces;
using Worker;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

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

builder.Services.AddHostedService<ProcessadorRespostasWorker>();

var host = builder.Build();
host.Run();
