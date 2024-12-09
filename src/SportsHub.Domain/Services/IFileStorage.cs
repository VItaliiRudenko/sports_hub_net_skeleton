using SportsHub.Domain.Dto;

namespace SportsHub.Domain.Services;

public interface IFileStorage
{
    public Task SaveFile(string fileName, string contentType, byte[] content);
    public Task<FileDataDto> LoadFile(string fileName);
}