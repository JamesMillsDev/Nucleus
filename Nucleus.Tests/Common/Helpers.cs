namespace Nucleus.Tests.Common
{
    public static class Helpers
    {
        public static bool Compare(float a, float b)
        {
            return Math.Abs(a - b) < float.Epsilon;
        }
    }
}