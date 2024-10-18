using Nucleus.Tests.Common;
using Nucleus.Tests.Common.SampleData;

namespace Nucleus.Tests.Patterns
{
    public class PrototypeTests
    {
        private PlayerActor? playerActor;
        private readonly List<Stat> stats = [];
        
        [SetUp]
        public void Setup()
        {
            stats.Add(new Stat("Health", 10));
            stats.Add(new Stat("Mana", 5));
            stats.Add(new Stat("Stamina", 20));

            playerActor = new PlayerActor(
                "Camera",
                new Vector3(0, 1, -10),
                new Vector3(45, 0, 0),
                new Vector3(1, 1, 1)
            )
            {
                velocity = new Vector3(0, 1, 0),
                angularVelocity = new Vector3(0, 5, 0),
                stats = stats
            };
        }

        [Test, Order(1)]
        public void CloneActorDeep()
        {
            PlayerActor? clone = playerActor?.Clone();
            
            Assert.That(clone, Is.Not.Null);
            Assert.That(() =>
            {
                if (clone == null || playerActor == null)
                {
                    return false;
                }
                
                return Comparisons.Compare(playerActor, clone);
            });
        }

        [Test, Order(2)]
        public void CloneVectorShallow()
        {
            Vector3? clone = playerActor?.position.Clone();
            
            Assert.That(clone, Is.Not.Null);
            Assert.That(() =>
            {
                if (clone == null || playerActor == null)
                {
                    return false;
                }
                
                return Comparisons.Compare(playerActor.position, clone.Value);
            });
        }

        [Test, Order(3)]
        public void ChangeCloneActor()
        {
            PlayerActor? clone = playerActor?.Clone();
            Assert.That(clone, Is.Not.Null);
            
            clone.position = new Vector3(0, 10, 0);
            
            Assert.That(() => playerActor != null && !Comparisons.Compare(playerActor, clone));
        }

        [TearDown]
        public void Cleanup()
        {
            stats.Clear();
        }
    }
}