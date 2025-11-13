using ApiExpanda.Application.DTOs;
using ApiExpanda.Application.Interfaces;
using ApiExpanda.Application.Services.Interfaces;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await Task.Run(() => _userRepository.GetUsers());
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await Task.Run(() => _userRepository.GetUser(userId));
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDataDto?> RegisterAsync(CreateUserDto createUserDto)
    {
        if (string.IsNullOrWhiteSpace(createUserDto.Username))
        {
            throw new ArgumentException("El nombre de usuario no puede estar vacío.");
        }

        if (!IsUniqueUser(createUserDto.Username))
        {
            throw new InvalidOperationException("El nombre de usuario ya existe.");
        }

        var result = await _userRepository.Register(createUserDto);

        if (result == null)
        {
            throw new InvalidOperationException("Error al registrar el usuario");
        }

        return result;
    }

    public async Task<UserLoginResponseDto?> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _userRepository.Login(userLoginDto);
    }

    public bool IsUniqueUser(string username)
    {
        return _userRepository.IsUniqueUser(username);
    }
}
