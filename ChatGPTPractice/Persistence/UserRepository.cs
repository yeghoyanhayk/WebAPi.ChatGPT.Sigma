using ChatGPTPractice.Entities;

namespace ChatGPTPractice.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new List<User>();

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_users);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var existingUser = GetByIdAsync(user.Id, cancellationToken).Result;
        if (existingUser is null)
        {
            return Task.CompletedTask;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var userToRemove = GetByIdAsync(id, cancellationToken).Result;
        if (userToRemove is not null)
        {
            _users.Remove(userToRemove);
        }

        return Task.CompletedTask;
    }
}