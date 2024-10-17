namespace Nucleus.Patterns
{
    /// <summary>
    /// A thread safe singleton type. It contains the functionality to
    /// create singletons using generic methodology. 
    /// </summary>
    /// <typeparam name="T">The type being converted into a singleton.</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        /// <summary>The thread safe accessor for the singleton instance.</summary>
        public static T Instance
        {
            get
            {
                // if the instance has already been created, we can just return it.
                if (instance != null)
                {
                    return instance;
                }
                
                // We need to create the instance, so we'll lock the singleton on threads
                // and create the instance.
                lock (@lock)
                {
                    instance ??= new T();
                }
                
                // Now that the singleton has been created, return it.
                return instance;
            }
        }
        
        // ReSharper disable once StaticMemberInGenericType
        /// <summary>A simple object that is used to lock threads.</summary>
        private static readonly object @lock = new();
        /// <summary>The internal managed singleton reference.</summary>
        private static T? instance;
    }
}