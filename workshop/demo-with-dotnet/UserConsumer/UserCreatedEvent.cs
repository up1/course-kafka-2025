public class UserCreatedEvent
{
    public required string EventName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
}