using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Articles;
using SportsHub.Api.Services;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Articles management endpoints for creating, reading, updating articles
/// </summary>
[ApiController]
[Route("api/articles")]
[Produces("application/json")]
[Tags("Articles")]
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

    /// <summary>
    /// Get all articles
    /// </summary>
    /// <returns>List of all articles with their details</returns>
    /// <response code="200">Articles retrieved successfully</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _articlesService.GetArticles();
        return Ok(result);
    }

    /// <summary>
    /// Get a specific article by ID
    /// </summary>
    /// <param name="articleId">The ID of the article to retrieve</param>
    /// <returns>Article details</returns>
    /// <response code="200">Article found and returned</response>
    /// <response code="404">Article not found</response>
    [HttpGet("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticle(int articleId)
    {
        var result = await _articlesService.GetArticle(articleId);
        
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Create a new article
    /// </summary>
    /// <param name="request">Article creation details</param>
    /// <returns>Created article details</returns>
    /// <response code="200">Article created successfully</response>
    /// <response code="400">Invalid article data</response>
    /// <response code="401">User not authenticated</response>
    [Authorize]
    [HttpPost("")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
    {
        var result = await _articlesService.CreateArticle(request);
        return Ok(result);
    }

    /// <summary>
    /// Partially update an existing article
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">Article update details</param>
    /// <returns>Updated article details</returns>
    /// <response code="200">Article updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="404">Article not found</response>
    [Authorize]
    [HttpPatch("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchArticle(int articleId, UpdateArticleRequest request)
    {
        var result = await _articlesService.UpdateArticle(articleId, request);

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Fully update an existing article
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">Article update details</param>
    /// <returns>Updated article details</returns>
    /// <response code="200">Article updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="404">Article not found</response>
    [Authorize]
    [HttpPut("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutArticle(int articleId, UpdateArticleRequest request)
    {
        var result = await _articlesService.UpdateArticle(articleId, request);

        return result is null ? NotFound() : Ok(result);
    }
}