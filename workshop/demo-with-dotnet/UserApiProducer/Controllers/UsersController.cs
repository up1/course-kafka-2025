using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IProducer<string, string> _producer;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserDbContext context,
        IProducer<string, string> producer,
        IConfiguration configuration,
        ILogger<UsersController> logger)
    {
        _context = context;
        _producer = producer;
        _configuration = configuration;
        _logger = logger;
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // --- Step 1: Insert into the database ---
        var newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User '{FirstName} {LastName}' saved to the database.", newUser.FirstName, newUser.LastName);


        // --- Step 2: Send event to Apache Kafka ---
        var kafkaTopic = _configuration["Kafka:Topic"];
        if (string.IsNullOrEmpty(kafkaTopic))
        {
            _logger.LogError("Kafka topic is not configured in appsettings.json.");
            // Decide if this should be a critical failure or not.
            // For this example, we'll proceed but log a severe error.
            return StatusCode(500, "Kafka topic not configured.");
        }

        var userEvent = new UserCreatedEvent
        {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Age = newUser.Age
        };

        // Serialize the event object to a JSON string
        var eventMessage = JsonSerializer.Serialize(userEvent);

        try
        {
            // Produce the message to the Kafka topic 
            // Config message key
            var key = "new-1234";
            var deliveryResult = await _producer.ProduceAsync(
                kafkaTopic,
                new Message<string, string> {
                    Key = key,
                    Value = eventMessage
                });

            _logger.LogInformation(
                "Kafka message sent to topic '{Topic}', partition {Partition}, offset {Offset}",
                deliveryResult.Topic,
                deliveryResult.Partition,
                deliveryResult.Offset);
        }
        catch (ProduceException<Null, string> e)
        {
            _logger.LogError("Failed to deliver message: {Reason}", e.Error.Reason);
            // Optionally, handle the failure, e.g., by adding to a dead-letter queue
            return StatusCode(500, "Failed to send Kafka message.");
        }

        return CreatedAtAction(nameof(CreateUser), new { id = newUser.Id }, newUser);
    }
}
