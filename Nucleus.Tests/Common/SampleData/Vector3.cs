namespace Nucleus.Tests.Common.SampleData
{
    public struct Vector3(float x, float y, float z)
    {
        public float x = x, y = y, z = z;
        
        public Vector3() : this(0, 0, 0)
        {
            
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}