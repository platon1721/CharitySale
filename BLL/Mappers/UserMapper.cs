using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

public static class UserMapper
{
    public static UserDto MapToDto(User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Firstname = entity.Firstname,
            Surname = entity.Surname,
            Login = entity.Login,
            IsAdmin = entity.IsAdmin,
        };
    }

    public static User MapToEntity(UserDto dto)
    {
        return new User
        {
            Firstname = dto.Firstname,
            Surname = dto.Surname,
            Login = dto.Login,
            IsAdmin = dto.IsAdmin
        };
    }

    public static User MapFromCreateDto(CreateUserDto dto)
    {
        return new User
        {
            Firstname = dto.Firstname,
            Surname = dto.Surname,
            Login = dto.Login,
            IsAdmin = dto.IsAdmin,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
}