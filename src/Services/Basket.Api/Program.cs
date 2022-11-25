using Basket.Api;
using Infrastructure.Common;
using Infrastructure.Common.Interfaces;
using Repositories;
using Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddTransient<ISerializeService, SerializeService>();

var connectStr = builder.Configuration.GetSection("CacheSettings");
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = connectStr["ConnectionString"];
});

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

app.Run();
