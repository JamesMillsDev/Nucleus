namespace Nucleus.Tests.Common.SampleData
{
    public class Actor(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        [NonSerialized] public Vector3 velocity;
        [NonSerialized] public Vector3 angularVelocity;

        public Actor() : this(new Vector3(), new Vector3(), new Vector3())
        {
            
        }
    }
}