using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.EntityFramework
{
    /// <summary>
    /// Denotes an entity with a <see cref="Created"/> and <see cref="Modified"/> property.
    /// </summary>
    public interface ITimestampedEntity
    {
        /// <summary>
        /// Saved by the database when the entity is created.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Updated by the database when the entity is updated.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? Modified { get; set; }
    }
}
