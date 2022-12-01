
using Domain.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.Api;
using Product.Api.Repository;
using Product.API.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
builder.Host.ConfigureAppConfiguration((context, config) =>
           {
               var env = context.HostingEnvironment;
               config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();
           });
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings");
var msBuilder = new MySqlConnectionStringBuilder(databaseSettings["ConnectionString"]);

builder.Services.AddDbContext<ProductContext>(m => m.UseMySql(msBuilder.ConnectionString,
    ServerVersion.AutoDetect(msBuilder.ConnectionString), e =>
    {
        e.MigrationsAssembly("Product.Api");
        e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
    }));
//builder.Services.AddSingleton<IHostedService, KafkaConsumerHandler>();

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

