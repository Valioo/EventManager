using EventManager.Application.Requests.Categories;
using EventManager.Application.Responses.Categories;

namespace EventManager.Application.Contracts;

public interface ICategoryService
{
    public Task<IList<CategoryResponse>> GetCategories(CancellationToken cancellationToken);
    public Task<CategoryResponse> CreateCategory(CategoryRequest categoryRequest, CancellationToken cancellationToken);
    public Task<CategoryResponse> UpdateCategory(CategoryRequest categoryRequest, int categoryId, CancellationToken cancellationToken);
    public Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken);
}
