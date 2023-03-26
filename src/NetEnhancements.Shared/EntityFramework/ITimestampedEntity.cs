using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.Shared.EntityFramework
{
    /// <summary>
    /// Denotes an entity with a <see cref="Created"/> and <see cref="Modified"/> property.
    /// </summary>
    public interface ITimestampedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? Modified { get; set; }
    }
}
