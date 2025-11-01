using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuizBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IGenericRepository<User> _repository;
    private readonly ILogger<UserController> _logger;

    public UserController(IGenericRepository<User> repository, ILogger<UserController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _repository.GetAll().ToListAsync();
        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        return Ok(user);
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto dto)
    {
        // Validate that the code is exactly "0010234"
        if (dto.Code != "0010234")
        {
            return BadRequest(new { message = "Invalid code. Only users with code '0010234' are allowed." });
        }

        // Check if a user with this code already exists
        var existingUser = await _repository.Find(u => u.Code == dto.Code).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return BadRequest(new { message = "A user with code '0010234' already exists. Only one user is allowed." });
        }

        var user = new User
        {
            Name = dto.Name,
            Code = dto.Code
        };

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        // Validate code if provided
        if (dto.Code != null && dto.Code != "0010234")
        {
            return BadRequest(new { message = "Invalid code. Only users with code '0010234' are allowed." });
        }

        user.Name = dto.Name ?? user.Name;
        if (dto.Code != null)
        {
            user.Code = dto.Code;
        }

        await _repository.Update(user);
        await _repository.SaveChangesAsync();

        return Ok(user);
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        _repository.Remove(user);
        await _repository.SaveChangesAsync();

        return Ok(new { message = $"User with ID {id} deleted successfully." });
    }
}

// DTOs for User operations
public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

