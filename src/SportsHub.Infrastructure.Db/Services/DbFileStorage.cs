using Microsoft.EntityFrameworkCore;
using SportsHub.Domain.Dto;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Services;

namespace SportsHub.Infrastructure.Db.Services;

internal class DbFileStorage : IFileStorage
{
    private readonly AppDbContext _appDbContext;

    public DbFileStorage(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SaveFile(string fileName, string contentType, byte[] content)
    {
        var normalizedFileName = NormalizeName(fileName);

        if (string.IsNullOrWhiteSpace(normalizedFileName))
        {
            throw new ArgumentException("Filename can't be blank or null", nameof(fileName));
        }

        _appDbContext.FileItems.Add(new FileItem(fileName.Trim().ToLowerInvariant(), contentType, content));
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<FileDataDto> LoadFile(string fileName)
    {
        var fileNameToSearch = fileName.Trim().ToLowerInvariant();
        
        if (string.IsNullOrWhiteSpace(fileNameToSearch))
        {
            throw new ArgumentException("Filename can't be blank or null", nameof(fileName));
        }

        var fileItem = await _appDbContext.FileItems.FirstOrDefaultAsync(f => f.FileName == fileNameToSearch);

        return fileItem == null ? null : new FileDataDto(fileItem.FileName, fileItem.ContentType, fileItem.Content);
    }

    private static string NormalizeName(string fileName)
    {
        return fileName?.Trim()?.ToLowerInvariant();
    }
}
