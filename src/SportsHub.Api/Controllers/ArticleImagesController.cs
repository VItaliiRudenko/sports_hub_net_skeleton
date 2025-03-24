using Microsoft.AspNetCore.Mvc;
using SportsHub.Domain.Services;

namespace SportsHub.Api.Controllers;

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

    [HttpGet("{fileName}")]
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