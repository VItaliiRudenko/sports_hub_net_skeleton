using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Articles;
using SportsHub.Api.Services;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Controller for managing articles in the Sports Hub application
/// </summary>
[ApiController]
[Route("api/articles")]
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
    /// Retrieves all articles
    /// </summary>
    /// <returns>A list of all articles</returns>
    /// <response code="200">Returns the list of articles</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(ArticleResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _articlesService.GetArticles();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific article by its ID
    /// </summary>
    /// <param name="articleId">The ID of the article to retrieve</param>
    /// <returns>The requested article</returns>
    /// <response code="200">Returns the requested article</response>
    /// <response code="404">If the article is not found</response>
    [HttpGet("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticle(int articleId)
    {
        var result = await _articlesService.GetArticle(articleId);
        
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Creates a new article
    /// </summary>
    /// <param name="request">The article creation data</param>
    /// <returns>The newly created article</returns>
    /// <response code="200">Returns the newly created article</response>
    /// <response code="401">If the user is not authorized</response>
    [Authorize]
    [HttpPost("")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
    {
        var result = await _articlesService.CreateArticle(request);
        return Ok(result);
    }

    /// <summary>
    /// Partially updates an existing article
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">The article update data</param>
    /// <returns>The updated article</returns>
    /// <response code="200">Returns the updated article</response>
    /// <response code="404">If the article is not found</response>
    /// <response code="401">If the user is not authorized</response>
    [Authorize]
    [HttpPatch("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PatchArticle(int articleId, UpdateArticleRequest request)
    {
        var result = await _articlesService.UpdateArticle(articleId, request);

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Fully updates an existing article
    /// </summary>
    /// <param name="articleId">The ID of the article to update</param>
    /// <param name="request">The article update data</param>
    /// <returns>The updated article</returns>
    /// <response code="200">Returns the updated article</response>
    /// <response code="404">If the article is not found</response>
    /// <response code="401">If the user is not authorized</response>
    [Authorize]
    [HttpPut("{articleId}")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutArticle(int articleId, UpdateArticleRequest request)
    {
        var result = await _articlesService.UpdateArticle(articleId, request);

        return result is null ? NotFound() : Ok(result);
    }
}