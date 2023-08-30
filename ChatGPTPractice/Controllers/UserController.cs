using ChatGPTPractice.Entities;
using ChatGPTPractice.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatGPTPractice.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        try
        {
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new {id = user.Id}, user.Id);
        }
        catch (DuplicateEmailException)
        {
            return Conflict("Email already exists");
        }
        catch (InvalidPasswordException)
        {
            return BadRequest("Invalid password");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        try
        {
            user.Id = id;
            await _userService.UpdateAsync(user);
            return Ok(user.Id);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
        catch (DuplicateEmailException)
        {
            return Conflict("Email already exists");
        }
        catch (InvalidPasswordException)
        {
            return BadRequest("Invalid password");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
    }
}