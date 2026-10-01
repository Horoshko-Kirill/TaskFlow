using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using TaskFlow.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    
    private readonly TaskFlowDbContext _context;

    public UserRepository(TaskFlowDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.
            FirstOrDefaultAsync(
                u => u.Email == email,
                cancellationToken);
        
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user =  await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        
        return user;
    }

    public async Task<PagedResult<User>> GetUsersAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .AsNoTracking()
            .OrderBy(x => x.Name);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<User>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}