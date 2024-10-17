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
    }
}
