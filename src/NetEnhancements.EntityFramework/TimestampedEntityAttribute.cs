namespace NetEnhancements.EntityFramework
{
    /// <summary>
    /// Entities annotated with this will have their 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TimestampedEntityAttribute : Attribute
    {
    }
}
