using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.Shared.EntityFramework
{
    /// <summary>
    /// Base entity with created/modified fields.
    /// </summary>
    public abstract class Entity : ITimestampedEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Modified { get; set; }
    }

    /// <summary>
    /// Base entity with a GUID as PK and created/modified fields.
    /// </summary>
    public abstract class GuidIdEntity : Entity, IGuidIdEntity
    {
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Base entity with an int as PK and created/modified fields.
    /// </summary>
    public abstract class IntIdEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
