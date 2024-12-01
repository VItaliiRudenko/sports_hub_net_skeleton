namespace SportsHub.Domain.Entities;

public abstract class AuditEntity
{
    public int Id { get; init; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string CreatedByUserId { get; set; }
    public string UpdatedByUserId { get; set; }
}