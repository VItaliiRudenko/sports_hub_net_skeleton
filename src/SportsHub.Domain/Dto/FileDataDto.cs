namespace SportsHub.Domain.Dto;

public record FileDataDto(string FileName,
    string ContentType,
    byte[] Content);
