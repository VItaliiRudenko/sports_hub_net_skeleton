using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Services;
using SportsHub.Domain.Entities;

namespace SportsHub.Api.Controllers;

[ApiController]
[Route("articles")]
public class ArticlesController : ControllerBase
{
    private readonly ILogger<ArticlesController> _logger;
    private readonly IArticlesService _articlesService;
    private readonly IApplicationMapper _map;

    public ArticlesController(
        ILogger<ArticlesController> logger,
        IArticlesService articlesService,
        IApplicationMapper map)
    {
        _logger = logger;
        _articlesService = articlesService;
        _map = map;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var result = Enumerable.Empty<Article>().Select(a => _map.ToArticleResponse(a)).ToArray();
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateArticle()
    {
        await _articlesService.CreateArticle();
        return Ok();
    }
}