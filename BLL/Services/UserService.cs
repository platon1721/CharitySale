using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class UserService: IUserService
{
    
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync();
        
        return users.Select(UserMapper.MapToDto).ToList();
    }

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

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == dto.Login);
    
        if (existingUser != null)
        {
            throw new DuplicateException($"Login {dto.Login} on juba kasutusel");
        }
        
        var user = UserMapper.MapFromCreateDto(dto);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return UserMapper.MapToDto(user);
    }

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