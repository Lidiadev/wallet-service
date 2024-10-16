using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Wallet.API.Middleware;
using Wallet.Application.Commands;
using Wallet.Application.Interfaces;
using Wallet.Domain;
using Wallet.Domain.Date;
using Wallet.Infrastructure.Caching;
using Wallet.Infrastructure.Date;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

DotEnv.Load();

// Add services to the container.
builder.Services.AddDbContext<WalletContext>(options =>
    options.UseSqlServer(
        Environment.GetEnvironmentVariable("DEFAULT_CONNECTION_STRING"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(builder.Configuration.GetValue<int>("EfRetryOnFailure"),
                TimeSpan.FromSeconds(5), null);
        }));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
    options.InstanceName = "WalletService";
});

builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWalletCommand).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();