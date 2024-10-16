namespace Nucleus.Tests.Common.SampleData
{
    public struct Vector3(float x, float y, float z) : IEquatable<Vector3>
    {
        public float x = x, y = y, z = z;
        
        public Vector3() : this(0, 0, 0)
        {
            
        }
        
        public bool Equals(Vector3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return Helpers.Compare(lhs.x, rhs.x) && Helpers.Compare(lhs.y, rhs.y) && Helpers.Compare(lhs.z, rhs.z);
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }
    }
}