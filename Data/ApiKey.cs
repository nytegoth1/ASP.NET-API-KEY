public class ApiKey
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public required string Email { get; set; } // New property for storing email
    public DateTime CreatedAt { get; set; }
}
