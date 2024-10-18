namespace Nucleus.Patterns
{
    /// <summary>
    /// A base class that can be used for implementing the prototype pattern easily.
    /// </summary>
    /// <typeparam name="T">The class that is implementing the prototype pattern.</typeparam>
    public interface IPrototype<out T> where T : IPrototype<T>
    {
        /// <returns>A deep clone of the object.</returns>
        public abstract T Clone();
    }
}