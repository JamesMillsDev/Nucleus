namespace Nucleus.Tests.Common.SampleData
{
    public class Actor
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        
        [NonSerialized] public Vector3 velocity;
        [NonSerialized] public Vector3 angularVelocity;

        protected Actor(Vector3 actorPosition, Vector3 actorRotation, Vector3 actorScale)
        {
            position = actorPosition;
            rotation = actorRotation;
            scale = actorScale;
        }

        public override string ToString()
        {
            return $"Position: {position}, Rotation: {rotation}, Scale: {scale}";
        }
    }
}