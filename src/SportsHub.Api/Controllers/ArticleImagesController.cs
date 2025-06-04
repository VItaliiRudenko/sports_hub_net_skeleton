using Microsoft.AspNetCore.Mvc;
using SportsHub.Domain.Services;

namespace SportsHub.Api.Controllers;

/// <summary>
/// File storage endpoints for retrieving article images
/// </summary>
[ApiController]
[Route("api/article-images")]
[Tags("File Storage")]
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
    /// Retrieve an article image by filename
    /// </summary>
    /// <param name="fileName">The filename of the image to retrieve</param>
    /// <returns>The image file with appropriate content type</returns>
    /// <response code="200">Image file returned successfully</response>
    /// <response code="404">Image file not found</response>
    [HttpGet("{fileName}")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("image/jpeg", "image/png", "image/gif", "image/webp")]
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