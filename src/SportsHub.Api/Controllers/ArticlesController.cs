using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Articles;
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

    [HttpGet("{articleId}")]
    public async Task<IActionResult> GetArticle(int articleId)
    {
        var result = await _articlesService.GetArticle(articleId);
        
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
    {
        var result = await _articlesService.CreateArticle(request);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{articleId}")]
    public async Task<IActionResult> CreateArticle(int articleId, UpdateArticleRequest request)
    {
        var result = await _articlesService.UpdateArticle(articleId, request);

        return result is null ? NotFound() : Ok(result);
    }
}