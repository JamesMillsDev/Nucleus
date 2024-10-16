using System.Diagnostics.CodeAnalysis;

namespace Nucleus.Tests.Common.SampleData
{
    public class PlayerActor : Actor
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

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((PlayerActor)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(stats, tag);
        }

        public override string ToString()
        {
            return $"Tag: {tag}, {base.ToString()}";
        }

        private bool Equals(PlayerActor other)
        {
            return position.Equals(other.position) && 
                   rotation.Equals(other.rotation) && 
                   scale.Equals(other.scale) && 
                   !stats.Where((t, i) => !t.Equals(other.stats[i])).Any() &&
                   tag.Equals(other.tag);
        }
        
        public static bool operator ==(PlayerActor lhs, PlayerActor rhs)
        {
            return lhs.position.Equals(rhs.position) && 
                   lhs.rotation.Equals(rhs.rotation) && 
                   lhs.scale.Equals(rhs.scale) && 
                   lhs.stats.Equals(rhs.stats) &&
                   lhs.tag.Equals(rhs.tag);
        }

        public static bool operator !=(PlayerActor lhs, PlayerActor rhs) => !(lhs == rhs);
    }
}