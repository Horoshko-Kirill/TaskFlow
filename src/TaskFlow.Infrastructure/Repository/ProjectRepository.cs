using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using TaskFlow.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Infrastructure.Repository;

public class ProjectRepository : IProjectRepository
{
    
    private readonly TaskFlowDbContext _context;
    
    public ProjectRepository(TaskFlowDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Projects
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        
        return user;
    }

    public async Task<PagedResult<Project>> GetProjectsAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects
            .AsNoTracking()
            .OrderBy(x => x.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Project>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<Project>> GetByUserIdAsync(Guid userId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Where(project => project.Members
                .Any(member => member.UserId == userId))
            .OrderBy(project => project.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Project>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}