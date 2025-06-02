namespace SportsHub.Domain.Entities;

/// <summary>
/// Base class for entities that require audit tracking (creation/modification timestamps and user IDs)
/// </summary>
public abstract class AuditEntity
{
    /// <summary>
    /// Gets the unique identifier for the entity
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// Gets the date and time when the entity was last updated (null if never updated)
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the ID of the user who created the entity
    /// </summary>
    public string CreatedByUserId { get; private set; }
    /// <summary>
    /// Gets the ID of the user who last updated the entity (null if never updated)
    /// </summary>
    public string UpdatedByUserId { get; private set; }

    /// <summary>
    /// Records creation audit information
    /// </summary>
    /// <param name="userId">The ID of the user creating the entity</param>
    public void TrackCreation(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedByUserId = userId;
    }

    /// <summary>
    /// Records update audit information
    /// </summary>
    /// <param name="userId">The ID of the user updating the entity</param>
    public void TrackUpdate(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedByUserId = userId;
    }
}