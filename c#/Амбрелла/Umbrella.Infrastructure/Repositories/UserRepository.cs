using Microsoft.EntityFrameworkCore;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Domain.Entities;
using Umbrella.Infrastructure.Data;

namespace Umbrella.Infrastructure.Repositories;

public sealed class UserRepository(UmbrellaDbContext context) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
    }

    public Task<User?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken = default)
    {
        return context.Users
            .FirstOrDefaultAsync(u => u.KeycloakId == keycloakId && !u.IsDeleted, cancellationToken);
    }

    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return context.Users
            .Where(u => !u.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Users
            .AnyAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
    }
}