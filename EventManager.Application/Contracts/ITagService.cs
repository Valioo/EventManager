using EventManager.Application.Requests.Tags;
using EventManager.Application.Responses.Tags;

namespace EventManager.Application.Contracts;

public interface ITagService
{
    public Task<IList<TagResponse>> GetTags(CancellationToken cancellationToken);
    public Task<TagResponse> CreateTag(TagRequest request, CancellationToken cancellationToken);
    public Task<TagResponse> UpdateTag(TagRequest request, int tagId, CancellationToken cancellationToken);
    public Task<bool> DeleteTag(int tagId, CancellationToken cancellationToken);
}
