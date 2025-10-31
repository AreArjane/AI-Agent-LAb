namespace Backend.Models;

public class Carrier
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }
    public bool Active { get; set; } = true;
}


