var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// API endpoints
using Backend.Data;
using Backend.Models;

// Posts (public: published only)
app.MapGet("/api/posts", () => DataStore.Posts.Where(p => p.Published));
app.MapGet("/api/posts/{id}", (Guid id) =>
{
    var post = DataStore.Posts.FirstOrDefault(p => p.Id == id && p.Published);
    return post is null ? Results.NotFound() : Results.Ok(post);
});
app.MapPost("/api/posts", (Post input) =>
{
    input.Id = Guid.NewGuid();
    input.CreatedAtUtc = DateTime.UtcNow;
    DataStore.Posts.Add(input);
    return Results.Created($"/api/posts/{input.Id}", input);
});
app.MapPut("/api/posts/{id}", (Guid id, Post input) =>
{
    var existing = DataStore.Posts.FirstOrDefault(p => p.Id == id);
    if (existing is null) return Results.NotFound();
    existing.Title = input.Title;
    existing.Content = input.Content;
    existing.Published = input.Published;
    existing.UpdatedAtUtc = DateTime.UtcNow;
    return Results.Ok(existing);
});
app.MapDelete("/api/posts/{id}", (Guid id) =>
{
    var existing = DataStore.Posts.FirstOrDefault(p => p.Id == id);
    if (existing is null) return Results.NotFound();
    DataStore.Posts.Remove(existing);
    return Results.NoContent();
});

// Carriers (public: active only)
app.MapGet("/api/carriers", () => DataStore.Carriers.Where(c => c.Active));
app.MapPost("/api/carriers", (Carrier input) =>
{
    input.Id = Guid.NewGuid();
    DataStore.Carriers.Add(input);
    return Results.Created($"/api/carriers/{input.Id}", input);
});
app.MapPut("/api/carriers/{id}", (Guid id, Carrier input) =>
{
    var existing = DataStore.Carriers.FirstOrDefault(c => c.Id == id);
    if (existing is null) return Results.NotFound();
    existing.Name = input.Name;
    existing.Description = input.Description;
    existing.WebsiteUrl = input.WebsiteUrl;
    existing.Active = input.Active;
    return Results.Ok(existing);
});
app.MapDelete("/api/carriers/{id}", (Guid id) =>
{
    var existing = DataStore.Carriers.FirstOrDefault(c => c.Id == id);
    if (existing is null) return Results.NotFound();
    DataStore.Carriers.Remove(existing);
    return Results.NoContent();
});

// Reviews (public: approved only)
app.MapGet("/api/reviews", () => DataStore.Reviews.Where(r => r.Approved));
app.MapPost("/api/reviews", (Review input) =>
{
    input.Id = Guid.NewGuid();
    input.CreatedAtUtc = DateTime.UtcNow;
    DataStore.Reviews.Add(input);
    return Results.Created($"/api/reviews/{input.Id}", input);
});
app.MapPut("/api/reviews/{id}", (Guid id, Review input) =>
{
    var existing = DataStore.Reviews.FirstOrDefault(r => r.Id == id);
    if (existing is null) return Results.NotFound();
    existing.AuthorName = input.AuthorName;
    existing.Body = input.Body;
    existing.Rating = input.Rating;
    existing.Approved = input.Approved;
    return Results.Ok(existing);
});
app.MapDelete("/api/reviews/{id}", (Guid id) =>
{
    var existing = DataStore.Reviews.FirstOrDefault(r => r.Id == id);
    if (existing is null) return Results.NotFound();
    DataStore.Reviews.Remove(existing);
    return Results.NoContent();
});

app.Run();

