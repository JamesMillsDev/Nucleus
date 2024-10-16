using System.Diagnostics.CodeAnalysis;

namespace Nucleus.Tests.Common.SampleData
{
    [Serializable]
    public class Stat(string title, float value)
    {
        public string title = title;
        public float value = value;
        
        public Stat() : this("", 0f)
        {
            
        }

        public override string ToString() => $"{title}: {value}";
        
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

            return obj.GetType() == GetType() && Equals((Stat)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() => HashCode.Combine(title, value);

        protected bool Equals(Stat other) => title == other.title && value.Equals(other.value);

        public static bool operator ==(Stat lhs, Stat rhs) => lhs.title == rhs.title && Helpers.Compare(lhs.value, rhs.value);
        public static bool operator !=(Stat lhs, Stat rhs) => !(lhs == rhs);
    }
}
