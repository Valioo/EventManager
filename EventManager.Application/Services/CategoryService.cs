using EventManager.Application.Contracts;
using EventManager.Application.Requests.Categories;
using EventManager.Application.Responses.Categories;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _appDbContext;

    public CategoryService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CategoryResponse> CreateCategory(CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var category = new Category(categoryRequest.Name);

        await _appDbContext.Categories.AddAsync(category, cancellationToken);
        await _appDbContext.SaveChangesAsync();

        return new CategoryResponse(category);
    }

    public async Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories
                                .Where(x => x.Id == categoryId)
                                .ExecuteDeleteAsync(cancellationToken);

        return category > 0;
    }

    public async Task<IList<CategoryResponse>> GetCategories(CancellationToken cancellationToken)
    {
        return await _appDbContext.Categories
                                .Select(x => new CategoryResponse(x))
                                .ToListAsync(cancellationToken);
    }

    public async Task<CategoryResponse> UpdateCategory(CategoryRequest categoryRequest, int categoryId, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories
                                .FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

        if (category is null)
        {
            return null!;
        }

        category.Name = categoryRequest.Name;
        await _appDbContext.SaveChangesAsync();

        return new CategoryResponse(category);
    }
}
