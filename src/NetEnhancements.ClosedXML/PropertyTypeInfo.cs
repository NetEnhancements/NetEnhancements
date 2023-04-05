using System.Reflection;

namespace NetEnhancements.ClosedXML;
 
internal record PropertyTypeInfo(PropertyInfo PropertyInfo, CellType CellType, bool IsNullable);
