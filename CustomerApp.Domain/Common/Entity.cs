namespace CustomerApp.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public virtual TId Id { get; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id.Equals(entity.Id);
    public bool Equals(Entity<TId>? other) => Equals((object?)other);

    public static bool operator == (Entity<TId> left, Entity<TId> right) => left.Equals(right);
    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !left.Equals(right);

    public override int GetHashCode() => Id.GetHashCode();
}
