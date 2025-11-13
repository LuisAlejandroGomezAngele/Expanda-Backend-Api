using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ApiExpanda.Application.DTOs;
using ApiExpanda.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiExpanda.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")] //http://localhost:5000/api/users
[ApiVersionNeutral]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        var usersDto = await _userService.GetAllUsersAsync();
        return Ok(usersDto);
    }

    [HttpGet("{userId}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(string userId)
    {
        var userDto = await _userService.GetUserByIdAsync(userId);
        if (userDto == null)
        {
            return NotFound("El usuario con el id especificado no existe.");
        }
        return Ok(userDto);
    }

    [AllowAnonymous]
    [HttpPost(Name = "RegisterUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        if (createUserDto == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userService.RegisterAsync(createUserDto);
            return CreatedAtRoute("GetUser", new { userId = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Retornar errores de validación (por ejemplo reglas de contraseña) como 400 Bad Request
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [AllowAnonymous]
    [HttpPost("Login", Name = "LoginUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        if (userLoginDto == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.LoginAsync(userLoginDto);

        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

}
