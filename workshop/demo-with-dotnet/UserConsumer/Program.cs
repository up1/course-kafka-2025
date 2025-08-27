using Confluent.Kafka;
using System.Text.Json;

Console.WriteLine("Starting Kafka Consumer...");

// --- Configuration ---
// The configuration is hard-coded here for simplicity,
// but it's better to use a configuration file (e.g., appsettings.json) in a real application.
var config = new ConsumerConfig
{
    // The address of your Kafka broker(s)
    BootstrapServers = "152.42.184.235:9092",

    // Disable automatic offset committing
    EnableAutoCommit = false,

    // A unique identifier for this consumer group.
    // All consumers with the same GroupId will work together to consume a topic.
    GroupId = "user-consumer-group",

    // When a new consumer group is started, this determines where to start reading from.
    // 'Earliest' means it will read all messages from the beginning of the topic.
    // 'Latest' means it will only read new messages produced after it starts.
    AutoOffsetReset = AutoOffsetReset.Earliest
};

const string topic = "users";

// Use a CancellationTokenSource to handle graceful shutdown (e.g., via Ctrl+C)
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => {
    e.Cancel = true; // Prevent the process from terminating immediately
    cts.Cancel();
};

// --- Consumer Loop ---
// The using statement ensures the consumer is properly closed and disposed of on exit.
using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    // Subscribe to the specified topic
    consumer.Subscribe(topic);
    Console.WriteLine($"Subscribed to topic: {topic}");
    Console.WriteLine("Listening for messages... Press Ctrl+C to exit.");

    try
    {
        // Start the consuming loop. This will block until the cancellation token is triggered.
        while (!cts.Token.IsCancellationRequested)
        {
            try
            {
                // Poll for a message. The timeout specifies how long to wait if no messages are available.
                var consumeResult = consumer.Consume(cts.Token);

                if (consumeResult.IsPartitionEOF)
                {
                    // Reached the end of the partition, but more messages may arrive.
                    // This is not an error.
                    Console.WriteLine($"Reached end of partition {consumeResult.Partition}, offset {consumeResult.Offset}. Waiting for more messages...");
                    continue;
                }

                Console.WriteLine($"Received message at offset {consumeResult.Offset}:");

                // --- Process the message ---
                var messageValue = consumeResult.Message.Value;
                try
                {
                    // Deserialize the JSON string into our event object
                    var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(messageValue);
                    if (userEvent != null && userEvent.EventName == "UserCreated")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"--> Event: {userEvent.EventName}, User: {userEvent.FirstName} {userEvent.LastName}, Age: {userEvent.Age}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"--> Received unknown event or malformed message: {messageValue}");
                        Console.ResetColor();
                    }
                    // Manually commit the offset after processing the message
                    consumer.Commit(consumeResult);
                }
                catch (JsonException jsonEx)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to deserialize message. Error: {jsonEx.Message}");
                    Console.ResetColor();
                    Console.WriteLine($"Raw message: {messageValue}");
                }
            }
            catch (ConsumeException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred while consuming: {e.Error.Reason}");
                Console.ResetColor();
            }
        }
    }
    catch (OperationCanceledException)
    {
        // This exception is expected when the consumer is shutting down.
        Console.WriteLine("Consumer is shutting down.");
    }
    finally
    {
        // Ensure the consumer closes its connection to Kafka.
        consumer.Close();
        Console.WriteLine("Consumer closed.");
    }
}
