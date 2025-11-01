using Microsoft.EntityFrameworkCore;
using QuizBackend;
using QuizBackend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase for JSON
        options.JsonSerializerOptions.WriteIndented = true; // Pretty print JSON
    });

// Add Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Using InMemory database for development
    // Change to UseSqlServer for production with connection string
    options.UseInMemoryDatabase("AppDb");
    // Uncomment below for SQL Server:
    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register generic repository for User
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();

// Register generic repository for Post
builder.Services.AddScoped<IGenericRepository<Post>, GenericRepository<Post>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
