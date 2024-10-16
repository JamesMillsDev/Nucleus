using System.Reflection;

namespace Nucleus.Extensions
{
    public static class Reflection
    {
        public static bool HasAttribute<TAttrib>(this FieldInfo field)
            where TAttrib : Attribute
        {
            return field.GetCustomAttribute<TAttrib>() != null;
        }
    }
}