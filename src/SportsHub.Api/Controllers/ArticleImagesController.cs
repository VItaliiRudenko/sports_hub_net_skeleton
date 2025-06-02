using Microsoft.AspNetCore.Mvc;
using SportsHub.Domain.Services;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Controller for managing article images in the Sports Hub application
/// </summary>
[ApiController]
[Route("api/article-images")]
public class ArticleImagesController : ControllerBase
{
    private readonly ILogger<ArticleImagesController> _logger;
    private readonly IFileStorage _fileStorage;

    public ArticleImagesController(
        ILogger<ArticleImagesController> logger,
        IFileStorage fileStorage)
    {
        _logger = logger;
        _fileStorage = fileStorage;
    }

    /// <summary>
    /// Retrieves an article image by its filename
    /// </summary>
    /// <param name="fileName">The name of the image file to retrieve</param>
    /// <returns>The image file content with appropriate content type</returns>
    /// <response code="200">Returns the image file</response>
    /// <response code="404">If the image file is not found</response>
    [HttpGet("{fileName}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string fileName)
    {
        var fileData = await _fileStorage.LoadFile(fileName);
        if (fileData == null)
        {
            return NotFound();
        }

        return File(fileData.Content, fileData.ContentType);
    }
}