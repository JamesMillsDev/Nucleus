namespace Nucleus.Structures
{
    public interface IBoundaryProvider<in T>
    {
        public bool InsideBoundary(T min, T max, T point);
    }
}