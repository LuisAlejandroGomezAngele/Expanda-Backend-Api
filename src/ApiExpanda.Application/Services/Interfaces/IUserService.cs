using ApiExpanda.Application.DTOs;

namespace ApiExpanda.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<UserDataDto?> RegisterAsync(CreateUserDto createUserDto);
    Task<UserLoginResponseDto?> LoginAsync(UserLoginDto userLoginDto);
    bool IsUniqueUser(string username);
}
