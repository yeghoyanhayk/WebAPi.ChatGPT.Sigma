using ChatGPTPractice.Entities;

namespace ChatGPTPractice.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}