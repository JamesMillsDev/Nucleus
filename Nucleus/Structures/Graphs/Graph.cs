namespace Nucleus.Structures.Graphs
{
    public class Graph<T, TVertex, TEdge, TCollection>
        where TVertex : Vertex<T, TEdge, TCollection> 
        where TEdge : Edge 
        where TCollection : ICollection<TEdge>, new()
    {
        private List<TVertex> vertices = [];
        
        public Graph()
        {
            
        }
    }
}