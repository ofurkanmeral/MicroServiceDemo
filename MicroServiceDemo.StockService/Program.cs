using MassTransit;
using MicroServiceDemo.Shared;
using MicroServiceDemo.StockService.Consumer;
using MicroServiceDemo.StockService.Models.Entities;
using MicroServiceDemo.StockService.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(_config =>
{
    _config.AddConsumer<OrderCreatedEventConsumer>();
    _config.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMq"]);
        config.ReceiveEndpoint(RabbitMQSetting.Stock_OrderCreatedEventQ, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDbService>();

#region SeedData Insert Mongo

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDbService mongo = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongo.GetCollection<Stock>();
if (!collection.FindSync(s => true).Any())
{
    await collection.InsertOneAsync(new Stock() { ProductId = Guid.NewGuid(), Quantity = 100 });
    await collection.InsertOneAsync(new Stock() { ProductId = Guid.NewGuid(), Quantity = 200 });
    await collection.InsertOneAsync(new Stock() { ProductId = Guid.NewGuid(), Quantity = 300 });
    await collection.InsertOneAsync(new Stock() { ProductId = Guid.NewGuid(), Quantity = 400 });

}

#endregion


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
