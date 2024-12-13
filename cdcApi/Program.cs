using cdcApi.Data;
using cdcApi.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//kafka service

/* Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<ItemService>(); // Registrar ItemService
        services.AddHostedService<KafkaConsumerService>();
    })
    .Build()
    .Run(); */
builder.Services.AddHostedService<KafkaConsumerService>();


builder.Services.AddSingleton<ItemService>();

// MYSQL SERVICE

builder.Services.AddDbContext<contextoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("inventoryDB"),
        ServerVersion.Parse("8.0.34-mysql")
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
