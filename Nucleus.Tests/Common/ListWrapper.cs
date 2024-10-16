using System.Text;

namespace Nucleus.Tests.Common
{
    [Serializable]
    public class ListWrapper<T>(List<T> list)
    {
        public readonly List<T>? wrapped = list;

        public ListWrapper() : this([])
        {
        }

        protected bool Equals(ListWrapper<T> other)
        {
            if (wrapped == null || other.wrapped == null)
            {
                return false;
            }

            if (wrapped.Count != other.wrapped.Count)
            {
                return false;
            }

            return !wrapped.Where((t, i) =>
            {
                if (t == null || other.wrapped[i] == null)
                {
                    return false;
                }

                return !t.Equals(other.wrapped[i]);
            }).Any();
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

            return obj.GetType() == GetType() && Equals((ListWrapper<T>)obj);
        }

        public override string ToString()
        {
            if (wrapped == null)
            {
                return "null";
            }

            StringBuilder builder = new();

            builder.Append('[');
            builder.AppendJoin(',', wrapped);
            builder.Append(']');

            return builder.ToString();
        }

        public override int GetHashCode() => wrapped == null ? 0 : wrapped.GetHashCode();

        public static bool operator ==(ListWrapper<T> left, ListWrapper<T> right)
        {
            if (left.wrapped == null || right.wrapped == null)
            {
                return false;
            }

            return left.wrapped.Equals(right.wrapped);
        }

        public static bool operator !=(ListWrapper<T> left, ListWrapper<T> right)
        {
            return !(left == right);
        }
    }
}