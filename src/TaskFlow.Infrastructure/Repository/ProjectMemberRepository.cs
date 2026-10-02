using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using TaskFlow.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Infrastructure.Repository;

public class ProjectMemberRepository : IProjectMemberRepository
{
    
    private readonly TaskFlowDbContext _context;
    
    public ProjectMemberRepository(TaskFlowDbContext context)
    {
        _context = context;
    }
    
    public async Task<ProjectMember?> GetAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default)
    {
        var projectMember = await _context.ProjectMembers
            .FirstOrDefaultAsync(
                x => x.ProjectId == projectId &&
                     x.UserId == userId,
                cancellationToken);
        
        return projectMember;
    }

    public async Task<PagedResult<ProjectMember>> GetByProjectIdAsync(Guid projectId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.ProjectMembers
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.JoinedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ProjectMember>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<ProjectMember>> GetByUserIdAsync(Guid userId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.ProjectMembers
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.JoinedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ProjectMember>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(ProjectMember projectMember, CancellationToken cancellationToken = default)
    {
        await _context.ProjectMembers.AddAsync(
            projectMember,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProjectMember projectMember, CancellationToken cancellationToken = default)
    {
        _context.ProjectMembers.Remove(projectMember);
        await _context.SaveChangesAsync(cancellationToken);
    }
}