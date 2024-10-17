namespace Nucleus.Patterns
{
    /// <summary>
    /// A base class that can be used for implementing the prototype pattern easily.
    /// Contains default functionality for shallow cloning using <see cref="Object.MemberwiseClone"/>
    /// </summary>
    /// <typeparam name="T">The class that is implementing the prototype pattern.</typeparam>
    public abstract class Prototype<T> where T : Prototype<T>
    {
        /// <summary>
        /// Creates a shallow copy of the object using the <see cref="Object.MemberwiseClone"/> functionality
        /// of the .NET system namespace.
        /// </summary>
        /// <returns>A shallow clone of the object.</returns>
        public T ShallowClone() => (T)MemberwiseClone();

        /// <returns>A deep clone of the object.</returns>
        public abstract T DeepClone();
    }
}