using BLL.DTO;

namespace BLL.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto> UpdateAsync(int id, CreateUserDto dto);
    Task DeleteAsync(int id);
    Task<UserDto> GetByLoginAsync(string login);
}