using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Languages;
using SportsHub.Api.Services;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Languages management endpoints for creating, reading, updating, and deleting languages
/// </summary>
[ApiController]
[Route("api/languages")]
[Produces("application/json")]
[Tags("Languages")]
public class LanguagesController : ControllerBase
{
    private readonly ILogger<LanguagesController> _logger;
    private readonly ILanguagesService _languagesService;

    public LanguagesController(
        ILogger<LanguagesController> logger,
        ILanguagesService languagesService)
    {
        _logger = logger;
        _languagesService = languagesService;
    }

    /// <summary>
    /// Get all languages
    /// </summary>
    /// <returns>List of all languages with their details</returns>
    /// <response code="200">Languages retrieved successfully</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<LanguageResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _languagesService.GetLanguages();
        return Ok(result);
    }

    /// <summary>
    /// Get a specific language by ID
    /// </summary>
    /// <param name="languageId">The ID of the language to retrieve</param>
    /// <returns>Language details</returns>
    /// <response code="200">Language found and returned</response>
    /// <response code="404">Language not found</response>
    [HttpGet("{languageId}")]
    [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLanguage(int languageId)
    {
        var result = await _languagesService.GetLanguage(languageId);
        
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Create a new language (Admin only)
    /// </summary>
    /// <param name="request">Language creation details</param>
    /// <returns>Created language details</returns>
    /// <response code="200">Language created successfully</response>
    /// <response code="400">Invalid language data or language code already exists</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User is not an admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPost("")]
    [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateLanguage(CreateLanguageRequest request)
    {
        try
        {
            var result = await _languagesService.CreateLanguage(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing language (Admin only)
    /// </summary>
    /// <param name="languageId">The ID of the language to update</param>
    /// <param name="request">Language update details</param>
    /// <returns>Updated language details</returns>
    /// <response code="200">Language updated successfully</response>
    /// <response code="400">Invalid update data or language code already exists</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User is not an admin</response>
    /// <response code="404">Language not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{languageId}")]
    [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLanguage(int languageId, UpdateLanguageRequest request)
    {
        try
        {
            var result = await _languagesService.UpdateLanguage(languageId, request);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a language (Admin only)
    /// </summary>
    /// <param name="languageId">The ID of the language to delete</param>
    /// <returns>Success status</returns>
    /// <response code="204">Language deleted successfully</response>
    /// <response code="400">Cannot delete protected language (English)</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User is not an admin</response>
    /// <response code="404">Language not found</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{languageId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLanguage(int languageId)
    {
        try
        {
            var result = await _languagesService.DeleteLanguage(languageId);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 