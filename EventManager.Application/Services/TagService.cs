using EventManager.Application.Contracts;
using EventManager.Application.Requests.Tags;
using EventManager.Application.Responses.Tags;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class TagService : ITagService
{
    private readonly AppDbContext _appDbContext;

    public TagService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TagResponse> CreateTag(TagRequest request, CancellationToken cancellationToken)
    {
        var exists = await _appDbContext.Tags.AnyAsync(a => a.Name == request.Name);

        if (exists)
        {
            return null!;
        }

        var tag = new Tag(request.Name);

        await _appDbContext.Tags.AddAsync(tag, cancellationToken);
        await _appDbContext.SaveChangesAsync();

        return new TagResponse(tag);
    }

    public async Task<bool> DeleteTag(int tagId, CancellationToken cancellationToken)
    {
        var tags = await _appDbContext.Tags
                        .Where(x => x.Id == tagId)
                        .ExecuteDeleteAsync(cancellationToken);

        return tags > 0;
    }

    public async Task<IList<TagResponse>> GetTags(CancellationToken cancellationToken)
    {
        return await _appDbContext.Tags
                        .Select(x => new TagResponse(x))
                        .ToListAsync(cancellationToken);
    }

    public async Task<TagResponse> UpdateTag(TagRequest request, int tagId, CancellationToken cancellationToken)
    {
        var tagNameExists = await _appDbContext.Tags
                        .AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (tagNameExists)
        {
            return null!;
        }

        var tag = await _appDbContext.Tags
                        .FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);

        if (tag is null)
        {
            return null!;
        }

        tag.Name = request.Name;
        await _appDbContext.SaveChangesAsync();

        return new TagResponse(tag);
    }
}