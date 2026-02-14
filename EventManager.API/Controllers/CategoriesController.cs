using EventManager.Application.Contracts;
using EventManager.Application.Requests.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// List all categories
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.GetCategories(cancellationToken));
    }

    /// <summary>
    /// Create a category
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _categoryService.CreateCategory(request, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    /// <summary>
    /// Update category details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> Update([FromRoute] int id,[FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _categoryService.UpdateCategory(request, id, cancellationToken);
        
        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    /// <summary>
    /// Deletes a category. This is not a soft delete. Allowed to Administrators only
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _categoryService.DeleteCategory(id, cancellationToken);

        if (!response)
        {
            return BadRequest();
        }

        return Ok();
    }
}
