namespace Nucleus.Structures.Graphs
{
    public class Vertex<T, TEdge, TCollection>
        where TEdge : Edge
        where TCollection : ICollection<TEdge>, new()
    {
        public T Value { get; set; }
        public TCollection Edges { get; }

        public Vertex(T value)
        {
            Value = value;
            Edges = [];
        }
    }
}