public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    
    // Navigation property for posts
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
