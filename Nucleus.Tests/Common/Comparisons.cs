using Nucleus.Tests.Common.SampleData;

namespace Nucleus.Tests.Common
{
    public static class Comparisons
    {
        public static bool Compare(float a, float b)
        {
            return Math.Abs(a - b) < float.Epsilon;
        }

        public static bool Compare(Vector3 lhs, Vector3 rhs)
        {
            return Compare(lhs.x, rhs.x) && 
                   Compare(lhs.y, rhs.y) &&
                   Compare(lhs.z, rhs.z);
        }

        public static bool Compare<T>(List<T> lhs, List<T> rhs, Func<T, T, bool> comparer)
        {
            if (lhs.Count != rhs.Count)
            {
                return false;
            }

            return !lhs.Where((t, i) => !comparer(t, rhs[i])).Any();
        }
    }
}