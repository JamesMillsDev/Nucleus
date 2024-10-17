using Nucleus.Serialization;
using Nucleus.Tests.Common;
using Nucleus.Tests.Common.SampleData;
using Exception = System.Exception;

namespace Nucleus.Tests.Serialization
{
    [Order(1)]
    public class BinarySerializationTests
    {
        private readonly Dictionary<string, string> paths = new()
        {
            { "single-layer", "serialize-single-layer.dat" },
            { "list", "serialize-list.dat" },
            { "inheritance", "serialize-inheritance.dat" }
        };

        private PlayerActor? playerActor;
        private readonly List<Stat> stats = [];

        [SetUp]
        public void Initialize()
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
        public void SerializeSingleLayer()
        {
            Vector3 testVector = new(77, 85, 1);

            try
            {
                BinarySerializer.Serialize(testVector, paths["single-layer"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }

            Assert.Pass();
        }

        [Test, Order(2)]
        public void DeserializeSingleLayer()
        {
            Vector3? testVector = new();

            try
            {
                BinarySerializer.Deserialize(ref testVector, paths["single-layer"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.That(() =>
            {
                if (!testVector.HasValue)
                {
                    return false;
                }
                
                Vector3 expected = new(77, 85, 1);
                Vector3 actual = testVector.Value;
                
                return Comparisons.Compare(expected, actual);
            });
        }

        [Test, Order(3), Description("Tests the deserialization of lists. A wrapper class needs to be used as top-level lists are not supported.")]
        public void SerializeList()
        {
            ListWrapper<Stat> statWrapper = new(stats);

            try
            {
                BinarySerializer.Serialize(statWrapper, paths["list"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }

            Assert.Pass();
        }

        [Test, Order(4), Description("Tests the deserialization of lists. A wrapper class needs to be used as top-level lists are not supported.")]
        public void DeserializeList()
        {
            ListWrapper<Stat>? statWrapper = new();

            try
            {
                BinarySerializer.Deserialize(ref statWrapper, paths["list"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }

            Assert.That(() =>
            {
                if (statWrapper == null)
                {
                    return false;
                }
                
                ListWrapper<Stat> expected = new(stats);
                ListWrapper<Stat> actual = statWrapper;

                if (expected.wrapped == null || actual.wrapped == null)
                {
                    return false;
                }

                return Comparisons.Compare(expected.wrapped, actual.wrapped, CompareStats);
            });
        }

        [Test, Order(5)]
        public void SerializeInheritanceObject()
        {
            try
            {
                BinarySerializer.Serialize(playerActor, paths["inheritance"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                throw;
            }
            
            Assert.Pass();
        }

        [Test, Order(6)]
        public void DeserializeInheritanceObject()
        {
            PlayerActor? actor = new();
            
            try
            {
                BinarySerializer.Deserialize(ref actor, paths["inheritance"]);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                throw;
            }
            
            Assert.That(() =>
            {
                if (actor == null || playerActor == null)
                {
                    return false;
                }
                
                PlayerActor expected = playerActor;
                PlayerActor actual = actor;
                
                return Comparisons.Compare(expected.stats, actual.stats, CompareStats) && 
                       expected.tag.Equals(actual.tag) && 
                       Comparisons.Compare(expected.position, actual.position) &&
                       Comparisons.Compare(expected.rotation, actual.rotation) &&
                       Comparisons.Compare(expected.scale, actual.scale);
            });
        }

        [TearDown]
        public void Cleanup()
        {
            stats.Clear();
        }

        private bool CompareStats(Stat lhs, Stat rhs) => Comparisons.Compare(lhs.value, rhs.value) &&
                                                         lhs.title.Equals(rhs.title);
    }
}