using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Services;

namespace SportsHub.Api.Controllers;

[ApiController]
[Route("articles")]
public class ArticlesController : ControllerBase
{
    private readonly ILogger<ArticlesController> _logger;
    private readonly IArticlesService _articlesService;

    public ArticlesController(
        ILogger<ArticlesController> logger,
        IArticlesService articlesService)
    {
        _logger = logger;
        _articlesService = articlesService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var result = await _articlesService.GetArticles();
        return Ok(result);
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> CreateArticle()
    {
        await _articlesService.CreateArticle();
        return Ok();
    }
}