using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuizBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IGenericRepository<Post> _postRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly ILogger<PostController> _logger;

    public PostController(
        IGenericRepository<Post> postRepository,
        IGenericRepository<User> userRepository,
        ILogger<PostController> logger)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    // GET: api/Post/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetUserPosts(int userId)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }

        var posts = await _postRepository.Find(p => p.UserId == userId).ToListAsync();
        return Ok(posts);
    }

    // GET: api/Post/5/user/{userId}
    [HttpGet("{id}/user/{userId}")]
    public async Task<ActionResult<Post>> GetPost(int id, int userId)
    {
        var post = await _postRepository.GetByIdAsync(id);
        
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found." });
        }

        // Ensure the post belongs to the user
        if (post.UserId != userId)
        {
            return Forbid("This post does not belong to the specified user.");
        }

        return Ok(post);
    }

    // POST: api/Post/user/{userId}
    [HttpPost("user/{userId}")]
    public async Task<ActionResult<Post>> CreatePost(int userId, [FromBody] CreatePostDto dto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            UserId = userId
        };

        await _postRepository.AddAsync(post);
        await _postRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id, userId = userId }, post);
    }

    // PUT: api/Post/5/user/{userId}
    [HttpPut("{id}/user/{userId}")]
    public async Task<IActionResult> UpdatePost(int id, int userId, [FromBody] UpdatePostDto dto)
    {
        var post = await _postRepository.GetByIdAsync(id);
        
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found." });
        }

        // Ensure the post belongs to the user
        if (post.UserId != userId)
        {
            return Forbid("This post does not belong to the specified user.");
        }

        post.Title = dto.Title ?? post.Title;
        post.Content = dto.Content ?? post.Content;

        await _postRepository.Update(post);
        await _postRepository.SaveChangesAsync();

        return Ok(post);
    }

    // DELETE: api/Post/5/user/{userId}
    [HttpDelete("{id}/user/{userId}")]
    public async Task<IActionResult> DeletePost(int id, int userId)
    {
        var post = await _postRepository.GetByIdAsync(id);
        
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found." });
        }

        // Ensure the post belongs to the user
        if (post.UserId != userId)
        {
            return Forbid("This post does not belong to the specified user.");
        }

        _postRepository.Remove(post);
        await _postRepository.SaveChangesAsync();

        return Ok(new { message = $"Post with ID {id} deleted successfully." });
    }
}

// DTOs for Post operations
public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class UpdatePostDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
}

