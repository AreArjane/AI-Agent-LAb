namespace Backend.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string AuthorName { get; set; }
    public required string Body { get; set; }
    public int Rating { get; set; } = 5; // 1-5
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool Approved { get; set; } = true;
}


