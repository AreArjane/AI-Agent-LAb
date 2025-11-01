using Backend.Models;

namespace Backend.Data;

public static class DataStore
{
    public static List<Post> Posts { get; } = new()
    {
        new Post { Title = "Welcome", Content = "First post", Published = true },
        new Post { Title = "News", Content = "Updates soon", Published = true }
    };

    public static List<Carrier> Carriers { get; } = new()
    {
        new Carrier { Name = "Acme Logistics", Description = "Fast and reliable" },
        new Carrier { Name = "Global Freight", Description = "Worldwide shipping" }
    };

    public static List<Review> Reviews { get; } = new()
    {
        new Review { AuthorName = "Alice", Body = "Great service!", Rating = 5, Approved = true },
        new Review { AuthorName = "Bob", Body = "On time delivery", Rating = 4, Approved = true }
    };
}


