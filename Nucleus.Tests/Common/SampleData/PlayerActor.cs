namespace Nucleus.Tests.Common.SampleData
{
    public class PlayerActor(string tag, Vector3 position, Vector3 rotation, Vector3 scale) : Actor(position, rotation, scale)
    {
        public List<Stat> stats = [];

        public PlayerActor() : this("", new Vector3(), new Vector3(), new Vector3())
        {
            
        }
    }
}