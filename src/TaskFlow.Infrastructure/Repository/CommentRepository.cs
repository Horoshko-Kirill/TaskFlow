using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using TaskFlow.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Infrastructure.Repository;

public class CommentRepository : ICommentRepository
{
    
    private readonly TaskFlowDbContext _context;

    public CommentRepository(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        
        return comment;
    }

    public async Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(
            comment,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _context.Comments.Update(comment);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Comment>> GetByTaskIdAsync(Guid taskId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Comments
            .AsNoTracking()
            .Where(x => x.TaskId == taskId)
            .OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Comment>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}