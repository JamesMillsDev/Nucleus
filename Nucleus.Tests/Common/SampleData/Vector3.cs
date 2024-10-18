using Nucleus.Patterns;

namespace Nucleus.Tests.Common.SampleData
{
    public struct Vector3(float x, float y, float z) : IPrototype<Vector3>
    {
        public float x = x, y = y, z = z;

        public Vector3() : this(0, 0, 0)
        {
            
        }

        public Vector3 Clone()
        {
            return (Vector3)MemberwiseClone();
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}