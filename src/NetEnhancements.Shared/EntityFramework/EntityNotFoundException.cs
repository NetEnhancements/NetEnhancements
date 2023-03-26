using System;
using System.Linq;

namespace NetEnhancements.Shared.EntityFramework
{
    public class EntityNotFoundException : Exception
    {
        public static EntityNotFoundException FromEntity<TEntity>(object primaryKey, params object[] additionalKeys)
        {
            var primaryKeyString = string.Join("', '", new[] { primaryKey }.Concat(additionalKeys));
            
            var errorString = $"Entity of type {typeof(TEntity).FullName} not found by PK '{primaryKeyString}'";

            return new EntityNotFoundException(errorString);
        }

        private EntityNotFoundException(string message) : base(message) { }
    }
}
