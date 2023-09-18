using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.EntityFramework
{
    /// <summary>
    /// Base entity with created/modified fields.
    /// </summary>
    public abstract class Entity : ITimestampedEntity
    {
        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset? Modified { get; set; }
    }

    /// <summary>
    /// Base entity with a GUID as PK and created/modified fields.
    /// </summary>
    public abstract class GuidIdEntity : Entity, IGuidIdEntity
    {
        /// <inheritdoc/>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Base entity with an int as PK and created/modified fields.
    /// </summary>
    public abstract class IntIdEntity : Entity
    {
        /// <summary>
        /// The PK of the entity, as <see cref="int"/>.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
