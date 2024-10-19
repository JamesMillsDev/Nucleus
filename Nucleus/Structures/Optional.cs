namespace Nucleus.Structures
{
    /// <summary></summary>
    public class Optional<T>
    {
        /// <summary></summary>
        private T? value;
        /// <summary></summary>
        private bool hasValue;

        /// <summary></summary>
        public Optional() : this(default) { }

        /// <summary></summary>
        /// <param name="initial"></param>
        public Optional(T? initial)
        {
            value = initial;
            hasValue = initial != null;
        }

        /// <summary></summary>
        /// <returns></returns>
        public T? Get() => hasValue ? value : default;

        /// <summary>
        /// Attempts to set the held value to a newly passed value.
        /// </summary>
        /// <returns>
        /// True if the value was set to the new value,
        /// false if the passed value was null or the held value matches the passed one.
        /// </returns>
        public bool Set(T? newValue)
        {
            // If the passed value is null we want to return
            if (newValue == null && !EqualityComparer<T>.Default.Equals(value, newValue))
            {
                return false;
            }

            value = newValue;
            return true;
        }

        /// <summary></summary>
        /// <returns></returns>
        public bool HasValue() => hasValue;

        /// <summary>Resets the internal value to the default value.</summary>
        public void Invalidate()
        {
            value = default;
            hasValue = false;
        }
    }
}