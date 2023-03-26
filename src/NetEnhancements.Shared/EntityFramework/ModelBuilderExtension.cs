using System.Linq.Expressions;
using System.Reflection;
using NetEnhancements.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace NetEnhancements.Shared.EntityFramework
{
    /// <summary>
    /// From various posts at https://stackoverflow.com/questions/47673524/ef-core-soft-delete-with-shadow-properties-and-query-filters
    /// </summary>
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Apply default Id, Created and Modified values for _all_ <see cref="IGuidIdEntity"/>/<see cref="ITimestampedEntity"/>-derived types known to the context's <paramref name="modelBuilder"/> it's called on.
        /// </summary>
        public static void ApplyDefaultValues(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyPropertyBuilder<IGuidIdEntity>(
                e => e.Id,
                prop => prop.HasDefaultValueSql("NEWID()")
                            .ValueGeneratedOnAdd()
                            // Don't allow updates
                            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw)
            );

            modelBuilder.ApplyPropertyBuilder<ITimestampedEntity>(
                e => e.Created,
                prop => prop.HasDefaultValueSql("SYSDATETIMEOFFSET()")
                            .ValueGeneratedOnAdd()
                            // Don't allow updates
                            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw)
            );

            modelBuilder.ApplyPropertyBuilder<ITimestampedEntity>(
                e => e.Modified,
                
                // Tells EF that the database generates this value, but does not allow setting it automatically. See IdentityDbContext.
                prop => prop.ValueGeneratedOnUpdate()
                            // Allow updates
                            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save)
            );
        }

        /// <summary>
        /// Applies the given <paramref name="expression"/> query filter to all entities that are or inherit from <typeparamref name="TEntity"/>.
        /// 
        /// Usage:
        /// <code>
        ///     builder.ApplyGlobalFilters{BaseEntity, SoftDeleteAttribute}(e => e.Deleted == null);
        /// </code>
        /// </summary>
        public static void ApplyGlobalFilters<TEntity, TAttribute>(this ModelBuilder modelBuilder, Expression<Func<TEntity, bool>> expression)
            where TEntity : class
            where TAttribute : Attribute
        {
            var entities = modelBuilder.GetAnnotatedEntities<TEntity, TAttribute>();

            foreach (var entity in entities)
            {
                var clrType = entity.ClrType;

                var newParam = Expression.Parameter(clrType);

                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);

                modelBuilder.Entity(clrType).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }

        /// <summary>
        /// Calls the <paramref name="expression"/> action with a <see cref="PropertyBuilder"/> for each entity inheriting from <typeparamref name="TBase"/>.
        /// 
        /// Usage:
        /// <code>
        ///     modelBuilder.ApplyPropertyBuilder{IGuidEntity}(
        ///         e => e.Id,
        ///         prop => prop.HasDefaultValueSql("NEWID()")
        ///     );
        /// </code>
        /// </summary>
        public static void ApplyPropertyBuilder<TBase>(this ModelBuilder modelBuilder, Expression<Func<TBase, object?>> propertyExpression, Action<PropertyBuilder> expression)
            where TBase : class
        {
            var entities = modelBuilder.GetInheritingEntities<TBase>();

            foreach (var entity in entities)
            {
                var clrType = entity.ClrType;

                var propertyBuilder = modelBuilder.Entity(clrType).Property(propertyExpression.Body.GetMemberName());

                expression(propertyBuilder);
            }
        }

        /// <summary>
        /// Calls the <paramref name="expression"/> action with a <see cref="ModelBuilder"/> for each entity inheriting from <typeparamref name="TBase"/>.
        /// 
        /// Usage:
        /// <code>
        ///     modelBuilder.ApplyEntityTypeBuilder{OwnedEntity}(
        ///         e => e.HasOne(...)
        ///     );
        /// </code>
        /// </summary>
        public static void ApplyEntityTypeBuilder<TBase>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder> expression)
            where TBase : class
        {
            var entities = modelBuilder.GetInheritingEntities<TBase>();

            foreach (var entity in entities)
            {
                var clrType = entity.ClrType;

                var propertyBuilder = modelBuilder.Entity(clrType);

                expression(propertyBuilder);
            }
        }

        /// <summary>
        /// Restrict ALL the cascades. Don't want one `DELETE FROM Locations` to clear the entire DB.
        /// </summary>
        public static void RestrictAllCascades(this ModelBuilder modelBuilder, Func<IMutableEntityType, bool>? selector = null)
        {
            selector ??= _ => true;

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .Where(selector)
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        /// BROKEN, not covariant.
        ///
        /// Calls the <paramref name="expression"/> action with a <see cref="ModelBuilder"/> for each entity inheriting from <typeparamref name="TBase"/>.
        /// 
        /// Usage:
        /// <code>
        ///     modelBuilder.ApplyEntityTypeBuilder{OwnedEntity}(
        ///         e => e.HasOne(...)
        ///     );
        /// </code>
        /// </summary>
        public static void ApplyEntityTypeBuilder<TBase>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<TBase>> expression)
            where TBase : class
        {
            var entities = modelBuilder.GetInheritingEntities<TBase>();

            // ModelBuilder.Entity<TEntity>()
            var genericEntityMethod = typeof(ModelBuilder)
                .GetTypeInfo()
                .DeclaredMethods
                .Single(m => m.Name == nameof(ModelBuilder.Entity) && m.GetParameters().Length == 0 && m.ContainsGenericParameters && m.GetGenericArguments().Length == 1);

            foreach (var entity in entities)
            {
                var clrType = entity.ClrType;

                var entityMethod = genericEntityMethod.MakeGenericMethod(clrType);

                var propertyBuilder = (EntityTypeBuilder<TBase>)entityMethod.Invoke(modelBuilder, null)!;

                expression(propertyBuilder);
            }
        }

        /// <summary>
        /// Maps an enum member to and from string.
        /// 
        /// TODO: does this work for flags enums?
        /// TODO: the hardcoded 64 is not safe.
        /// </summary>
        public static PropertyBuilder<TEnum> EnumToString<TEnum>(this PropertyBuilder<TEnum> propertyBuilder)
            where TEnum : struct, Enum
        {
            return propertyBuilder.HasMaxLength(64).HasConversion(
               value => value.ToString(),
               value => Enum.Parse<TEnum>(value)
           );
        }

        /// <summary>
        /// Returns entities known to the ModelBuilder that are annotated with the given <typeparamref name="TAttribute"/>.
        /// </summary>
        public static IList<IMutableEntityType> GetAnnotatedEntities<TBase, TAttribute>(this ModelBuilder modelBuilder)
            where TBase : class
            where TAttribute : Attribute
        {
            var entityType = typeof(TBase);

            return modelBuilder.GetInheritingEntities<TBase>()
                               .Where(e => entityType.GetCustomAttributes(inherit: false).Any(a => a is TAttribute))
                               .ToList();
        }

        /// <summary>
        /// Returns entities known to the ModelBuilder that derive from the given <typeparamref name="TBase"/>.
        /// </summary>
        public static IEnumerable<IMutableEntityType> GetInheritingEntities<TBase>(this ModelBuilder modelBuilder)
        {
            var baseType = typeof(TBase);

            return modelBuilder.Model.GetEntityTypes()
                                     .Where(e => baseType.IsAssignableFrom(e.ClrType));
        }
    }
}
