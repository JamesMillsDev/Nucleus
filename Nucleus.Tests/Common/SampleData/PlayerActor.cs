using Nucleus.Patterns;

namespace Nucleus.Tests.Common.SampleData
{
    public class PlayerActor : Actor, IPrototype<PlayerActor>
    {
        public List<Stat> stats = [];
        public string tag;

        public PlayerActor() : this("", new Vector3(), new Vector3(), new Vector3())
        {
            
        }

        public PlayerActor(string tag, Vector3 actorPosition, Vector3 actorRotation, Vector3 actorScale)
            : base(actorPosition, actorRotation, actorScale)
        {
            this.tag = tag;
        }

        public PlayerActor Clone()
        {
            PlayerActor actor = new()
            {
                position = position,
                rotation = rotation,
                scale = scale,
                velocity = velocity,
                angularVelocity = angularVelocity,
                tag = tag,
                stats = stats
            };

            return actor;
        }

        public override string ToString()
        {
            return $"Tag: {tag}, {base.ToString()}";
        }
    }
}