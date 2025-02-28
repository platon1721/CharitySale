using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;


/// <summary>
/// Service for managing users, including CRUD operations and user retrieval by login.
/// </summary>
public class UserService: IUserService
{
    
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    
    // Retrieves all users from the database.
    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync();
        
        return users.Select(UserMapper.MapToDto).ToList();
    }

    
    // Retrieves a user by his unique identifier.
    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }

        return UserMapper.MapToDto(user);
    }

    // Creates a new user in the database.
    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == dto.Login);
    
        if (existingUser != null)
        {
            throw new DuplicateException($"Login {dto.Login} is already in use");
        }
        
        var user = UserMapper.MapFromCreateDto(dto);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return UserMapper.MapToDto(user);
    }

    // Updates an existing user.
    public async Task<UserDto> UpdateAsync(int id, CreateUserDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        
        user.Firstname = dto.Firstname;
        user.Surname = dto.Surname;
        user.ModifiedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return UserMapper.MapToDto(user);
    }

    // Deletes a user from the database.
    public async Task DeleteAsync(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    // Retrieves a user by their login.
    public async Task<UserDto> GetByLoginAsync(string login)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login);
    
        if (user == null)
        {
            throw new NotFoundException($"User with login {login} not found");
        }
    
        return UserMapper.MapToDto(user);
    }
}