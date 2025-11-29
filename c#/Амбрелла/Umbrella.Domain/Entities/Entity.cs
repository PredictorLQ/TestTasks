namespace Umbrella.Domain.Entities;

/// <summary>
/// Базовый класс для всех сущностей домена
/// </summary>
public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsDeleted { get; set; }
    public DateTime? ChangedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public long Version { get; set; } = 0;

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public virtual void CascadeDelete()
    {
        Delete();
    }
}