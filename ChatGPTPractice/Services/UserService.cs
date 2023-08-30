using System.Text.RegularExpressions;
using ChatGPTPractice.Entities;
using ChatGPTPractice.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ChatGPTPractice.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private const string UserPasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new UserNotFoundException();
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var emailUsed = (await _userRepository.GetAllAsync(cancellationToken)).Any(u => u.Email == user.Email);
        if (emailUsed)
        {
            throw new DuplicateEmailException();
        }

        if (!IsPasswordValid(user.PasswordHash))
        {
            throw new InvalidPasswordException();
        }

        user.PasswordHash = HashPassword(user.PasswordHash);

        await _userRepository.AddAsync(user, cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id, cancellationToken);
        if (existingUser == null)
        {
            throw new UserNotFoundException();
        }

        var emailUsed = (await _userRepository.GetAllAsync(cancellationToken)).Any(u => u.Email == user.Email && u.Id != user.Id);
        if (emailUsed)
        {
            throw new DuplicateEmailException();
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;

        if (!string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            if (!IsPasswordValid(user.PasswordHash))
            {
                throw new InvalidPasswordException();
            }

            existingUser.PasswordHash = HashPassword(user.PasswordHash);
        }

        await _userRepository.UpdateAsync(existingUser, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(id, cancellationToken);
    }

    private static string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User?>();
        return passwordHasher.HashPassword(default, password);
    }

    private static bool IsPasswordValid(string password)
    {
        var regex = new Regex(UserPasswordRegex);
        return regex.IsMatch(password);
    }
}