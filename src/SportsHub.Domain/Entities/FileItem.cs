namespace SportsHub.Domain.Entities;

public record FileItem(
    string FileName,
    string ContentType,
    byte[] Content)
{
    public int Id { get; init; }
}