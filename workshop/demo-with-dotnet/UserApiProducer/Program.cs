using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// 1. Add API Controller support
builder.Services.AddControllers();

// 2. Configure and register the EF Core DbContext for an in-memory database
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseInMemoryDatabase("UserDb"));

// 3. Configure and register the Kafka Producer
// We use a Singleton lifecycle for the producer as it's thread-safe and efficient.
var producerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    CompressionType = CompressionType.Lz4,
    Acks = Acks.Leader,
};
builder.Services.AddSingleton(new ProducerBuilder<string, string>(producerConfig).Build());


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseHttpClientMetrics(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMetricServer();
    app.UseHttpMetrics();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
