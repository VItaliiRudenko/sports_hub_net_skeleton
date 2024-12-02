namespace SportsHub.Domain.Entities;

public abstract class AuditEntity
{
    public int Id { get; init; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public string CreatedByUserId { get; private set; }
    public string UpdatedByUserId { get; private set; }

    public void TrackCreation(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedByUserId = userId;
    }

    public void TrackUpdate(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedByUserId = userId;
    }
}