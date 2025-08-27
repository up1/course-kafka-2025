public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
}


public class UserCreatedEvent
{
    public string EventName => "UserCreated"; // Constant event name
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
}