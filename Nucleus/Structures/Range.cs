namespace Nucleus.Structures
{
    public class Range<T>
    {
        public T Min { get; }
        public T Max { get; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool In(T value, IBoundaryProvider<T> boundaryProvider)
        {
            return boundaryProvider.InsideBoundary(Min, Max, value);
        }
    }
}