using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.Shared.EntityFramework
{
    /// <summary>
    /// Denotes an entity with a <see cref="Guid"/> <see cref="Id"/>.
    /// </summary>
    public interface IGuidIdEntity
    {
        /// <summary>
        /// The primary key of this entity, as <see cref="Guid"/>.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        Guid Id { get; set; }
    }
}
