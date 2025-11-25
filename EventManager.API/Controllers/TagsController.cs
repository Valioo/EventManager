using EventManager.Application.Contracts;
using EventManager.Application.Requests.Categories;
using EventManager.Application.Requests.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagsService;

    public TagsController(ITagService tagsService)
    {
        _tagsService = tagsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _tagsService.GetTags(cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] TagRequest request, CancellationToken cancellationToken)
    {
        var response = await _tagsService.CreateTag(request, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TagRequest request, CancellationToken cancellationToken)
    {
        var response = await _tagsService.UpdateTag(request, id, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _tagsService.DeleteTag(id, cancellationToken);

        if (!response)
        {
            return BadRequest();
        }

        return Ok();
    }
}
