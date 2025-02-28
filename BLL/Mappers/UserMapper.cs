using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

/// <summary>
/// Provides mapping methods for converting between User entities and DTOs.
/// </summary>
public static class UserMapper
{
    
    /// <summary>
    /// Maps a User entity to a UserDto.
    /// </summary>
    /// <param name="entity">The user entity to map.</param>
    /// <returns>A DTO representing the user.</returns>
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

    
    /// <summary>
    /// Maps a UserDto to a User entity.
    /// </summary>
    /// <param name="dto">The DTO containing user data.</param>
    /// <returns>A user entity.</returns>
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

    
    /// <summary>
    /// Maps a CreateUserDto to a new User entity.
    /// </summary>
    /// <param name="dto">The DTO containing user creation data.</param>
    /// <returns>A new user entity.</returns>
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